namespace Estudos.ShortGuids;

public sealed class ShortGuid
{
    private readonly Guid _guid;
    private readonly string _value;

    public ShortGuid(Guid guid)
    {
        if (guid == default)
        {
            throw new ArgumentNullException(nameof(guid));
        }

        _guid = guid;

        _value = Convert.ToBase64String(guid.ToByteArray())
            .Substring(0, 22)
            .Replace("/", "_")
            .Replace("+", "-");
    }

    public override string ToString() => _value;

    public Guid ToGuid() => _guid;

    public static ShortGuid Parse(string shortGuid)
    {
        if (string.IsNullOrWhiteSpace(shortGuid))
        {
            throw new ArgumentNullException(nameof(shortGuid));
        }

        if (shortGuid.Length != 22)
        {
            throw new FormatException("Input is not in the correct format");
        }

        return new ShortGuid(new Guid(Convert.FromBase64String
            (shortGuid.Replace("_", "/").Replace("-", "+") + "==")));
    }

    public static ShortGuid NewShortGuid() => new(Guid.NewGuid());

    public static implicit operator string(ShortGuid guid) => guid.ToString();

    public static implicit operator Guid(ShortGuid shortGuid) => shortGuid._guid;
}