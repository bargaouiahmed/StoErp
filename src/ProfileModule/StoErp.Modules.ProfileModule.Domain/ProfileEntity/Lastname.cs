namespace StoErp.Modules.ProfileModule.Domain.ProfileEntity;

public readonly record struct Lastname
{
    public string Value { get; }
    private Lastname(string value) => Value = value;

    public static Lastname From(string value) => new(value);
    public override string ToString() => Value.ToString();

}
