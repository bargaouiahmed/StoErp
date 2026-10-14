
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoErp.Modules.Auth.Domain.VerificationToken;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Conversions;

public sealed class VerificationTokenHashConversion : ValueConverter<VerificationTokenHash, string>
{
    public VerificationTokenHashConversion() : base(
        hash => hash.Value,
        str => VerificationTokenHash.From(str))
    { }


}
