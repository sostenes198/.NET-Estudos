namespace Estudos.NSubstitute.Lookup
{
    public interface ILookup
    {
        bool TryLookup(string key, out string value);
    }
}