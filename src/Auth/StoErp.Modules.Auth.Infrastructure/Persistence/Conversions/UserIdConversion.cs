
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Conversions;

public sealed class UserIdConversion : ValueConverter<UserId, Guid>
{

    public UserIdConversion() : base(
        userId => userId.Value,
        guid => UserId.From(guid))
    {
    }

}
