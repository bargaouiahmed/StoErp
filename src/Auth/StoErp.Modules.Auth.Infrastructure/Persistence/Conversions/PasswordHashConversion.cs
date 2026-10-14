
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Conversions;

public sealed class PasswordHashConversion : ValueConverter<PasswordHash, string>
{
    public PasswordHashConversion() : base(
        passwordHash => passwordHash.Value,
        str => PasswordHash.From(str))
    {
    }


}
