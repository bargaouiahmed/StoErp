
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Conversions;

public sealed class UsernameConversion : ValueConverter<Username, string>
{
    public UsernameConversion() : base(
        username => username.Value,
        str => Username.From(str))

    { }
}
