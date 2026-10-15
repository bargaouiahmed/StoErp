using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoErp.Modules.ProfileModule.Domain.ProfileEntity;

namespace StoErp.Modules.ProfileModule.Infrastructure.Persistence.Conversions;

public sealed class FirstnameConversion : ValueConverter<Firstname,string>
{
    public FirstnameConversion() : base(
        firstname => firstname.Value,
        value => Firstname.From(value))
    {
    }

}
