
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoErp.Modules.Auth.Domain.PasswordResetToken;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Conversions;

public class PasswordResetTokenIdConversion : ValueConverter<PasswordResetTokenId, Guid>
{
    public PasswordResetTokenIdConversion() : base(
        passwordResetTokenId=> passwordResetTokenId.Value,
        guid=>PasswordResetTokenId.From(guid)
            ){}
    
}

