namespace StoErp.Modules.ProfileModule.Domain.ProfileEntity;

public readonly record struct Firstname
{
    public string Value { get; }
    private Firstname(string value) => Value = value;

    public static Firstname From(string value) => new(value);
    public override string ToString() => Value.ToString();
}
