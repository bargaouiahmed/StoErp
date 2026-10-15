
namespace StoErp.Modules.ProfileModule.Domain.ProfileEntity;

public readonly record struct LanguagePreference
{
    public string Value {get;}
    private LanguagePreference(string value) => Value = value;



    public static LanguagePreference From(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Language preference cannot be null or empty.", nameof(value));

        if(value.Length>2)
            throw new ArgumentException("Language preference must be a two-letter ISO 639-1 code.", nameof(value));
        return new LanguagePreference(value);
    }
    

}
