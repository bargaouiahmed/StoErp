
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoErp.Modules.Auth.Domain.PasswordResetToken;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Conversions;

public class PasswordResetTokenHashConversion : ValueConverter<PasswordResetTokenHash, string>

{
    public PasswordResetTokenHashConversion() : base
    (
        passwordResetTokenHash => passwordResetTokenHash.Value,
        str => PasswordResetTokenHash.From(str)

    ){}

}
