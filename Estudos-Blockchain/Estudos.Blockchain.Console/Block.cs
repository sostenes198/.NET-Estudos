using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Estudos.Blockchain.Console
{
    public class Block
    {
        public Block(DateTime timeStamp, string previousHash, IList<Transaction> transactions)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Transactions = transactions;
        }

        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public IList<Transaction> Transactions { get; set; }
        public int Nonce { get; set; }

        public string CalculateHash()
        {
            var sha256 = SHA256.Create();

            var inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{JsonConvert.SerializeObject(Transactions)}-{Nonce}");
            var outputBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }

        public void Mine(int difficulty)
        {
            var tempo = new Stopwatch();
            tempo.Start();
            var leadingZeros = new string('0', difficulty);
            while (Hash == null || Hash.Substring(0, difficulty) != leadingZeros)
            {
                Nonce++;
                Hash = CalculateHash();
            }
            tempo.Stop();
            System.Console.WriteLine(tempo.Elapsed);
        }
    }
}