namespace Estudos.Checklist.ConfiguracoesPc.Domain.ScansResult.Directory
{
    public class DirectoryScanResult
    {
        public DirectoryScanResult(string name, bool found, bool canRead, bool canWrite)
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