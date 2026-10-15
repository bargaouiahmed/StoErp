
using StoErp.Modules.Auth.Domain.PasswordResetToken;

namespace StoErp.Modules.Auth.Application.Ports.Persistence;

public interface IPasswordResetTokenWriteRepository
{
    void Add(PasswordResetToken passwordResetToken);
}
