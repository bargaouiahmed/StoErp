using System;
using Mediator;
using StoErp.Modules.Auth.Application.Ports.Outbox;
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Application.Ports.Security;
using StoErp.Modules.Auth.Domain.PasswordResetToken;
using StoErp.Modules.Auth.Domain.User;
using StoErp.Shared.Contracts.AuthModuleContracts;

namespace StoErp.Modules.Auth.Application.Commands.RequestPasswordReset;

public sealed class RequestPasswordResetCommandHandler : ICommandHandler<RequestPasswordResetCommand, Unit>
{

private readonly IUserReadRepository _userReadRepository;
private readonly IPasswordResetTokenWriteRepository _passwordResetTokenWriteRepository;

private readonly IUnitOfWork _unitOfWork;
private readonly IPasswordResetTokenGenerator _passwordResetTokenGenerator;
private readonly IPasswordResetTokenHasher _passwordResetTokenHasher;
private readonly PasswordResetTokenOptions _passwordResetTokenOptions;
private readonly IAuthOutboxWriter _authOutboxWriter;
public RequestPasswordResetCommandHandler(
    IUserReadRepository userReadRepository,
    IPasswordResetTokenWriteRepository passwordResetTokenWriteRepository,
    IAuthOutboxWriter authOutboxWriter,
    IUnitOfWork unitOfWork,
    IPasswordResetTokenGenerator passwordResetTokenGenerator,
    IPasswordResetTokenHasher passwordResetTokenHasher,
    PasswordResetTokenOptions passwordResetTokenOptions
)
    {
    _userReadRepository = userReadRepository;
        _passwordResetTokenWriteRepository = passwordResetTokenWriteRepository;
        _unitOfWork = unitOfWork;
        _passwordResetTokenGenerator = passwordResetTokenGenerator;
        _passwordResetTokenHasher = passwordResetTokenHasher;
        _passwordResetTokenOptions = passwordResetTokenOptions;
        _authOutboxWriter = authOutboxWriter;
    }
    public async ValueTask<Unit> Handle(RequestPasswordResetCommand command, CancellationToken cancellationToken)
    {
        var user = await _userReadRepository.GetPasswordResetDataByEmailAsync(Email.From(command.Email), cancellationToken) ?? throw new InvalidOperationException("User not found");
        
        var passwordResetTokenSecret = _passwordResetTokenGenerator.Generate(_passwordResetTokenOptions);
        var passwordResetTokenHash = PasswordResetTokenHash.From(_passwordResetTokenHasher.Hash(passwordResetTokenSecret));
        var passwordResetTokenExpiresAt = DateTimeOffset.UtcNow.Add(_passwordResetTokenOptions.TokenExpirationTime);

        var passwordResetToken = PasswordResetToken.Create(PasswordResetTokenId.New(),user.Id, passwordResetTokenHash, passwordResetTokenExpiresAt, DateTimeOffset.UtcNow);
        _passwordResetTokenWriteRepository.Add(passwordResetToken);
        var passwordResetRequestedEvent = new UserRequestedChangePassword(command.Email, passwordResetTokenSecret);
        _authOutboxWriter.Add(passwordResetRequestedEvent);
        await _unitOfWork.CommitAsync(cancellationToken);
        return Unit.Value;
        
    }
}
