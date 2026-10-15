
namespace StoErp.Modules.ProfileModule.Domain.ProfileEntity;

public sealed class Profile
{
    public ProfileId Id { get; private set; }
    public UserId UserId { get; private set; }
    public Firstname Firstname { get; private set; }
    public Lastname Lastname { get; private set; }
    public LanguagePreference LanguagePreference { get; private set; }
    public DateTimeOffset DateOfBirth { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }


    private Profile(){}
    private Profile(ProfileId id, UserId userId, Firstname firstname, Lastname lastname, DateTimeOffset dateOfBirth, LanguagePreference languagePreference)
    {
        Id = id;
        UserId = userId;
        Firstname = firstname;
        Lastname = lastname;
        LanguagePreference = languagePreference;
        DateOfBirth = dateOfBirth;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public static Profile Create(UserId userId, Firstname firstname, Lastname lastname, DateTimeOffset dateOfBirth, LanguagePreference languagePreference)
    {
        return new(ProfileId.New(), userId, firstname, lastname, dateOfBirth, languagePreference);
    }

}
