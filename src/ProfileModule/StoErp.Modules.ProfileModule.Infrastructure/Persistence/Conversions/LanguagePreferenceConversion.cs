using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoErp.Modules.ProfileModule.Domain.ProfileEntity;

namespace StoErp.Modules.ProfileModule.Infrastructure.Persistence.Conversions;

public sealed class LanguagePreferenceConversion : ValueConverter<LanguagePreference,string>
{
    public LanguagePreferenceConversion() : base(
        languagePreference => languagePreference.Value,
        value => LanguagePreference.From(value))
    {
    }

}
