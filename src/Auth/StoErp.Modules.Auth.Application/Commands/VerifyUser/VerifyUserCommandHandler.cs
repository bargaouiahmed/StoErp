
using Mediator;
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Application.Ports.Security;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Application.Commands.VerifyUser;

public sealed class VerifyUserCommandHandler : ICommandHandler<VerifyUserCommand, Unit>
{
    private readonly IUserReadRepository _userReadRepository;
    private readonly IVerificationTokenReadRepository _verificationTokenReadRepository;
    private readonly IVerificationTokenWriteRepository _verificationTokenWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVerificationTokenHasher _verificationTokenHasher;
    public VerifyUserCommandHandler(
        IUserReadRepository userReadRepository,
        IVerificationTokenReadRepository verificationTokenReadRepository,
        IVerificationTokenWriteRepository verificationTokenWriteRepository,
        IUnitOfWork unitOfWork,
        IVerificationTokenHasher verificationTokenHasher
    )
    {
        _userReadRepository = userReadRepository;
        _verificationTokenReadRepository = verificationTokenReadRepository;
        _verificationTokenWriteRepository = verificationTokenWriteRepository;
        _unitOfWork = unitOfWork;
        _verificationTokenHasher = verificationTokenHasher;
    }
    public async ValueTask<Unit> Handle(VerifyUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userReadRepository.GetByEmailForUpdateAsync(Email.From(command.Email), cancellationToken);
        if (user is null)
        {
            throw new InvalidOperationException("User not found");
        }
        if (user.Verified)
        {
            throw new InvalidOperationException("User already verified");
        }
        if (user.DeletedAt is not null)
        {
            throw new InvalidOperationException("User is deleted");
        }
        var verificationToken = await _verificationTokenReadRepository.GetByUserIdForUpdateAsync(user.Id, cancellationToken);
        if (verificationToken is null)
        {
            throw new InvalidOperationException("Verification token not found");
        }
        if (verificationToken.IsUsed)
        {
            throw new InvalidOperationException("Verification token already used");
        }
        if (verificationToken.ExpiresAt < DateTimeOffset.UtcNow)
        {
            throw new InvalidOperationException("Verification token expired");
        }

        if (!_verificationTokenHasher.Verify(command.TokenSecret, verificationToken.TokenHash))
        {
            throw new InvalidOperationException("Invalid verification token");
        }

        //Mark the token as used and the user as verified
        verificationToken.MarkAsUsed();
        user.MarkVerified();
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
