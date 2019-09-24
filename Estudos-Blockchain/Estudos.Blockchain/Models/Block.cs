using System;
using System.Security.Cryptography;
using System.Text;

namespace Estudos.Blockchain.Models
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public string Content { get; set; }

        public Block(DateTime timeStamp, string previousHash, string content)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Content = content;
            Hash = CalculateHash();
        }

        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();

            var inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{Content}");
            var outputBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(outputBytes);
        }
    }
}