using System;
using Mediator;

namespace StoErp.Modules.Auth.Application.Queries.LoggedInUserDetails;

public sealed record LoggedInUserDetailsQuery(
    Guid UserId
): IQuery<LoggedInUserDetailsQueryResult>;
