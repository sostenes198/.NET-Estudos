using CommandLine;

namespace Estudos.Ferraments.CLI
{
    public class Options
    {
        [Option("verbose", Required = true, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option("version", Required = true, HelpText = "Set output to version messages.")]
        public string Version { get; set; }
        
        [Option("int_test", Required = true, HelpText = "Set output to version messages.")]
        public int IntTest { get; set; }
        
        [Option("test", Required = true, HelpText = "Set output to version messages.")]
        public string Test { get; set; }
    }
}