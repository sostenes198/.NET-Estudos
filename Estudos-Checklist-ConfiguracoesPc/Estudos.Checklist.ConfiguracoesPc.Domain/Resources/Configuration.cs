namespace Estudos.Checklist.ConfiguracoesPc.Domain.Resources
{
    public class Configuration
    {
        public string Host { get; set; }
        public int[] Ports { get; set; }
        
        public string[] RegistriesWindows { get; set; }
        
        public string[] Directories { get; set; }
    }
}