namespace Estudos.Checklist.ConfiguracoesPc.Domain.ScansResult.Registry
{
    public class RegistryScanResult
    {
        public RegistryScanResult(string name, bool found, bool canRead, bool canWrite)
        {
            Name = name;
            Found = found;
            CanRead = canRead;
            CanWrite = canWrite;
        }

        public string Name { get; }

        public bool Found { get; }

        public bool CanRead { get; }

        public bool CanWrite { get; }
    }
}