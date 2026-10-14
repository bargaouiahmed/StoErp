
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Conversions;

public sealed class EmailConversion : ValueConverter<Email, string>
{
    public EmailConversion() : base(
        email => email.Value,
        str => Email.From(str))
    {
    }


}
