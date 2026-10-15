
namespace StoErp.Modules.ProfileModule.Domain.ProfileEntity;

public readonly record struct ProfileId
{
    public Guid Value {get;}
    private ProfileId(Guid value) => Value = value;

    public static ProfileId New() => new(Guid.CreateVersion7());
    public static ProfileId From(Guid value) => new(value);
    public override string ToString() => Value.ToString();
}
