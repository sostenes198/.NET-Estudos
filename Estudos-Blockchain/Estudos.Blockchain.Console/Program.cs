using System;
using System.Text.Json;
using Estudos.Blockchain.Models;

namespace Estudos.Blockchain.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var startTime = DateTime.Now;

            Models.Blockchain phillyCoin = new Models.Blockchain();
            
            System.Console.WriteLine(JsonSerializer.Serialize(phillyCoin, new JsonSerializerOptions {WriteIndented = true}));
            
            phillyCoin.CreateTransaction(new Transaction("Henry", "MaHesh", 10));  
            phillyCoin.ProcessPendingTransactions("Bill");  
            System.Console.WriteLine(JsonSerializer.Serialize(phillyCoin, new JsonSerializerOptions {WriteIndented = true}));  
  
            phillyCoin.CreateTransaction(new Transaction("MaHesh", "Henry", 5));  
            phillyCoin.CreateTransaction(new Transaction("MaHesh", "Henry", 5));  
            phillyCoin.ProcessPendingTransactions("Bill");  
            phillyCoin.ProcessPendingTransactions("Bill");  
            phillyCoin.ProcessPendingTransactions("Bill");  
            phillyCoin.ProcessPendingTransactions("Bill");  
            phillyCoin.ProcessPendingTransactions("Bill");  
            System.Console.WriteLine(JsonSerializer.Serialize(phillyCoin, new JsonSerializerOptions {WriteIndented = true}));
            
            System.Console.WriteLine("=========================");  
            System.Console.WriteLine($"Henry' balance: {phillyCoin.GetBalance("Henry")}");  
            System.Console.WriteLine($"MaHesh' balance: {phillyCoin.GetBalance("MaHesh")}");  
            System.Console.WriteLine($"Bill' balance: {phillyCoin.GetBalance("Bill")}");  
  
            System.Console.WriteLine("=========================");  
            System.Console.WriteLine($"phillyCoin");  
        }
    }
}