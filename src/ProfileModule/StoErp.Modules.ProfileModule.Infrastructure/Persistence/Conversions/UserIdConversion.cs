using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoErp.Modules.ProfileModule.Domain.ProfileEntity;

namespace StoErp.Modules.ProfileModule.Infrastructure.Persistence.Conversions;

public sealed class UserIdConversion : ValueConverter<UserId,Guid>
{
    public UserIdConversion() : base(
        userId => userId.Value,
        value => UserId.From(value))
    {
    }

}
