using System;
using Estudos.Blockchain.Models;
using Newtonsoft.Json;

namespace Estudos.Blockchain.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var phillyCoin = new Models.Blockchain();
            phillyCoin.AddBlock(new Block(DateTime.Now, null, "{sender:Henry,receiver:MaHesh,amount:10}"));
            phillyCoin.AddBlock(new Block(DateTime.Now, null, "{sender:MaHesh,receiver:Henry,amount:5}"));
            phillyCoin.AddBlock(new Block(DateTime.Now, null, "{sender:Mahesh,receiver:Henry,amount:5}"));

            System.Console.WriteLine(JsonConvert.SerializeObject(phillyCoin, Formatting.Indented));
            
            System.Console.WriteLine($"Is Chain Valid: {phillyCoin.IsValid()}");  
  
            System.Console.WriteLine($"Update amount to 1000");  
            phillyCoin.Chain[1].Content = "{sender:Henry,receiver:MaHesh,amount:1000}";  
  
            System.Console.WriteLine($"Is Chain Valid: {phillyCoin.IsValid()}");  
        }
    }
}