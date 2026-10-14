
using Mediator;
using StoErp.Modules.Auth.Application.Ports.Outbox;
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Application.Ports.Security;
using StoErp.Modules.Auth.Domain.User;
using StoErp.Modules.Auth.Domain.VerificationToken;
using StoErp.Shared.Application.Events;
using StoErp.Shared.Contracts.AuthModuleContracts;

namespace StoErp.Modules.Auth.Application.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegisterUserResult>
{
    private readonly IUserWriteRepository _userWriteRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthOutboxWriter _outboxWriter;

    private readonly IVerificationTokenGenerator _verificationTokenGenerator;
    private readonly IVerificationTokenHasher _verificationTokenHasher;
    private readonly VerificationTokenOptions _verificationTokenOptions;
    private readonly IVerificationTokenWriteRepository _verificationTokenWriteRepository;
    public RegisterUserCommandHandler(
        IUserWriteRepository userWriteRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        IAuthOutboxWriter outboxWriter,
        IVerificationTokenGenerator verificationTokenGenerator,
        IVerificationTokenHasher verificationTokenHasher,
        IVerificationTokenWriteRepository verificationTokenWriteRepository,
        VerificationTokenOptions verificationTokenOptions)
    {
        _userWriteRepository = userWriteRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _outboxWriter = outboxWriter;
        _verificationTokenGenerator = verificationTokenGenerator;
        _verificationTokenHasher = verificationTokenHasher;
        _verificationTokenWriteRepository = verificationTokenWriteRepository;
        _verificationTokenOptions = verificationTokenOptions;
    }

    public async ValueTask<RegisterUserResult> Handle(RegisterUserCommand command, CancellationToken cancellationToken = default)
    {
        var username = Username.From(command.Username);
        var email = Email.From(command.Email);

        if (await _userWriteRepository.ExistsByUsernameAsync(username, cancellationToken))
        {
            throw new InvalidOperationException($"Username '{command.Username}' is already taken.");
        }

        if (await _userWriteRepository.ExistsByEmailAsync(email, cancellationToken))
        {
            throw new InvalidOperationException($"Email '{command.Email}' is already registered.");
        }
        var userId = UserId.New();
        var passwordHash = _passwordHasher.Hash(command.Password);
        var user = User.Create(userId, username, email, passwordHash, DateTimeOffset.UtcNow);
        var verificationTokenResult = _verificationTokenGenerator.GenerateVerificationToken(_verificationTokenOptions, cancellationToken);
        var verificationTokenHash = _verificationTokenHasher.Hash(verificationTokenResult.Secret);
        var verificationToken = VerificationToken.Create(
            VerificationTokenId.New(),
            userId,
            VerificationTokenHash.From(verificationTokenHash),
            DateTimeOffset.UtcNow,
            verificationTokenResult.ExpiresAt);
        _userWriteRepository.Add(user);
        _verificationTokenWriteRepository.Add(verificationToken);
        var userRegisteredEvent = new UserRegisteredEvent(
            userId.Value,
            username.Value,
            email.Value,
            user.CreatedAt,
            verificationTokenResult.Secret,
            command.LanguagePreference
        );
        _outboxWriter.Add(userRegisteredEvent);

        await _unitOfWork.CommitAsync(cancellationToken);
        return new RegisterUserResult(userId.Value, username.Value, email.Value);
    }

}
