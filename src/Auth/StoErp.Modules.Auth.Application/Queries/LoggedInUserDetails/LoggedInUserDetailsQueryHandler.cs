using System;
using Mediator;
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Domain.User;
using StoErp.Shared.Contracts.ProfileModuleContracts;

namespace StoErp.Modules.Auth.Application.Queries.LoggedInUserDetails;

public sealed class LoggedInUserDetailsQueryHandler : IQueryHandler<LoggedInUserDetailsQuery, LoggedInUserDetailsQueryResult>
{
    private readonly IProfileSettingsProvider _profileSettingsProvider;
    private readonly IUserReadRepository _userReadRepository;
    public LoggedInUserDetailsQueryHandler(IProfileSettingsProvider profileSettingsProvider, IUserReadRepository userReadRepository)
    {
        _profileSettingsProvider = profileSettingsProvider;
        _userReadRepository = userReadRepository;
    }
    public async ValueTask<LoggedInUserDetailsQueryResult> Handle(LoggedInUserDetailsQuery query, CancellationToken cancellationToken)
    {
        var userId = UserId.From(query.UserId);
        var user = await _userReadRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new InvalidOperationException("User not found.");
        }
        if(user.IsDeleted() || !user.Verified)
        {
            throw new InvalidOperationException("User doesn't work");
        }
        var profile = await _profileSettingsProvider.GetProfileByUserIdAsync(query.UserId, cancellationToken);

        return new LoggedInUserDetailsQueryResult
        {
            Username = user.Username.Value,
            Email = user.Email.Value,
            Firstname = profile?.FirstName ?? string.Empty,
            Lastname = profile?.LastName ?? string.Empty,
            LanguagePreference = profile?.LanguagePreference ?? "en"
        };
    }
}
