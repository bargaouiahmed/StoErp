using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoErp.Modules.ProfileModule.Domain.ProfileEntity;

namespace StoErp.Modules.ProfileModule.Infrastructure.Persistence.Conversions;

public sealed class LastnameConversion : ValueConverter<Lastname,string>
{
    public LastnameConversion() : base(
        lastname => lastname.Value,
        value => Lastname.From(value))
    {
    }

}
