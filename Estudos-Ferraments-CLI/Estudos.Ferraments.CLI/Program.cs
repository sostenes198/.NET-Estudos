using System;
using CommandLine;

namespace Estudos.Ferraments.CLI
{
    class Program
    {
        static int Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args)
                .MapResult(o =>
                {
                    try
                    {
                        // We have the parsed arguments, so let's just pass them down
                        return new OptionsResult(o, 1);
                    }
                    catch
                    {
                        Console.WriteLine("Unhandled error!");
                        return new OptionsResult(o, -3);
                    }
                }, errs => new OptionsResult(default, -1));

            if (result.Result <= 0)
                return result.Result;


            return 1;
        }
    }
}