using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Estudos.Exame.Capitulo1.GerenciaFluxoPrograma.Threads.Collections
{
    public class BlockingCollectionStudy
    {
        public static void BlockingCollectionTest()
        {
            BlockingCollection<int> Data = new BlockingCollection<int>(5);

            Task.Run(() =>
            {
                for (int i = 0; i < 11; i++)
                {
                    Data.Add(i);
                    Console.WriteLine($"Data {i} added sucessfully");
                }

                Data.CompleteAdding();
            });

            Console.ReadKey();
            Console.WriteLine("Reading Collection");

            Task.Run(() =>
            {
                while (Data.IsCompleted == false)
                {
                    try
                    {
                        int v = Data.Take();
                        Console.WriteLine($"Data {v} taken sucessfully");
                    }
                    catch (InvalidOperationException)
                    {
                    }
                }
                
                Console.WriteLine(Data.Count);
            });
        }
    }
}