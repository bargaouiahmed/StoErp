
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoErp.Modules.Auth.Domain.VerificationToken;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Conversions;

public sealed class VerificationTokenIdConversion : ValueConverter<VerificationTokenId, Guid>
{
    public VerificationTokenIdConversion() : base(
        id => id.Value,
        guid => VerificationTokenId.From(guid))
    { }
}
