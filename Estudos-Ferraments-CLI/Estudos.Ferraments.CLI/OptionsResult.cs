namespace Estudos.Ferraments.CLI
{
    public class OptionsResult
    {
        public Options Options { get; }
        public int Result { get; }
        
        public OptionsResult(Options options, int result)
        {
            Options = options;
            Result = result;
        }
    }
}