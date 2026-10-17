
using Mediator;
using StoErp.Modules.Auth.Application.Ports.Outbox;
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Application.Ports.Security;
using StoErp.Modules.Auth.Domain.PasswordResetToken;
using StoErp.Modules.Auth.Domain.User;
using StoErp.Shared.Contracts.AuthModuleContracts;
using StoErp.Shared.Contracts.ProfileModuleContracts;

namespace StoErp.Modules.Auth.Application.Commands.RequestPasswordReset;

public sealed class RequestPasswordResetCommandHandler : ICommandHandler<RequestPasswordResetCommand, Unit>
{
private readonly IProfileSettingsProvider _profileSettingsProvider;
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
    PasswordResetTokenOptions passwordResetTokenOptions,
    IProfileSettingsProvider profileSettingsProvider
)
    {
    _userReadRepository = userReadRepository;
        _passwordResetTokenWriteRepository = passwordResetTokenWriteRepository;
        _unitOfWork = unitOfWork;
        _passwordResetTokenGenerator = passwordResetTokenGenerator;
        _passwordResetTokenHasher = passwordResetTokenHasher;
        _passwordResetTokenOptions = passwordResetTokenOptions;
        _authOutboxWriter = authOutboxWriter;
        _profileSettingsProvider = profileSettingsProvider;
    }
    public async ValueTask<Unit> Handle(RequestPasswordResetCommand command, CancellationToken cancellationToken)
    {
        var user = await _userReadRepository.GetPasswordResetDataByEmailAsync(Email.From(command.Email), cancellationToken) ?? throw new InvalidOperationException("User not found");
        if (user.Verified == false)
        {
            throw new InvalidOperationException("User is not verified");
        }
        if(user.IsDeleted)
        {
            throw new InvalidOperationException("User is deleted");
        }
        var passwordResetTokenSecret = _passwordResetTokenGenerator.Generate(_passwordResetTokenOptions);
        var passwordResetTokenHash = _passwordResetTokenHasher.Hash(passwordResetTokenSecret);
        var passwordResetTokenExpiresAt = DateTimeOffset.UtcNow.Add(_passwordResetTokenOptions.TokenExpirationTime);

        var passwordResetToken = PasswordResetToken.Create(PasswordResetTokenId.New(),user.Id, passwordResetTokenHash, passwordResetTokenExpiresAt, DateTimeOffset.UtcNow);
        _passwordResetTokenWriteRepository.Add(passwordResetToken);
        var languageSettings = await _profileSettingsProvider.GetLanguageSettingsByUserIdAsync(user.Id.Value, cancellationToken);
        var passwordResetRequestedEvent = new UserRequestedChangePassword(command.Email, passwordResetTokenSecret, user.Username,languageSettings.LanguagePreference);
        _authOutboxWriter.Add(passwordResetRequestedEvent);
        await _unitOfWork.CommitAsync(cancellationToken);
        return Unit.Value;
        
    }
}
