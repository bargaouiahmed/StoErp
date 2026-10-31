using Mediator;
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Application.Ports.Security;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Application.Commands.ResetPassword;

public sealed class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, Unit>
{   
    private readonly IPasswordResetTokenReadRepository _passwordResetTokenReadRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserReadRepository _userReadRepository;
    private readonly IPasswordResetTokenHasher _passwordResetTokenHasher;
    private readonly IUnitOfWork _unitOfWork;
    public ResetPasswordCommandHandler(
        IPasswordResetTokenReadRepository passwordResetTokenReadRepository,
        IUserReadRepository userReadRepository,
        IPasswordResetTokenHasher passwordResetTokenHasher,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher
    )
    {
        _passwordResetTokenReadRepository = passwordResetTokenReadRepository;
        _userReadRepository = userReadRepository;
        _passwordResetTokenHasher = passwordResetTokenHasher;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }
    public async ValueTask<Unit> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await _userReadRepository.GetByEmailForUpdateAsync(Email.From(command.Email), cancellationToken) ?? throw new InvalidOperationException("User not found");
        if (user.Verified == false)
        {
            throw new InvalidOperationException("User is not verified");
        }
        if(user.IsDeleted())
        {
            throw new InvalidOperationException("User is deleted");
        }
        var passwordResetToken = await _passwordResetTokenReadRepository.GetByUserIdForUpdateAsync(user.Id, cancellationToken) ?? throw new InvalidOperationException("Password reset token not found");
        if (passwordResetToken.ExpiresAt < DateTimeOffset.UtcNow || passwordResetToken.IsUsed)
        {
            throw new InvalidOperationException("Password reset token is expired or already used");
        }
        if (!_passwordResetTokenHasher.Verify(command.ResetTokenSecret, passwordResetToken.TokenHash))
        {
            throw new InvalidOperationException("Password reset token is invalid");
        }

        var newPasswordHash = _passwordHasher.Hash(command.NewPassword);
        user.UpdatePassword(newPasswordHash, DateTimeOffset.UtcNow);

        passwordResetToken.MarkAsUsed();

        await _unitOfWork.CommitAsync(cancellationToken);
        return Unit.Value;
    }
}
