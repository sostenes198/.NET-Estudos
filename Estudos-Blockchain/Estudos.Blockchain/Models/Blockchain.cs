using System;
using System.Collections.Generic;
using System.Linq;

namespace Estudos.Blockchain.Models
{
    public class Blockchain
    {
        public int Reward { get; set; } = 1;
        public IList<Block> Chain { get; set; }
        IList<Transaction> PendingTransactions = new List<Transaction>();
        public int Difficulty { set; get; } = 2;

        public Blockchain()
        {
            InitializeChain();
            AddGenesisBlock();
        }

        public void InitializeChain()
        {
            Chain = new List<Block>();
        }

        public Block CreateBlockGenesis()
        {
            Block block = new Block(DateTime.Now, null, PendingTransactions);
            block.Mine(Difficulty);
            PendingTransactions = new List<Transaction>();
            return block;
        }


        public void AddGenesisBlock() => Chain.Add(CreateBlockGenesis());


        public Block GetLatestBlock() => Chain[Chain.Count - 1];

        public void AddBlock(Block block)
        {
            var latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Mine(Difficulty);
            Chain.Add(block);
        }

        public bool IsValid()
        {
            for (var i = 1; i < Chain.Count; i++)
            {
                var currentBlock = Chain[i];
                var previousBlock = Chain[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash()) return false;
                if (currentBlock.PreviousHash != previousBlock.Hash) return false;
            }

            return true;
        }

        public void CreateTransaction(Transaction transaction)
        {
            PendingTransactions.Add(transaction);
        }

        public void ProcessPendingTransactions(string mineAdress)
        {
            if (!PendingTransactions.Any())
                return;
            
            PendingTransactions.Add(new Transaction(null, mineAdress, Reward));
            var block = new Block(DateTime.Now, GetLatestBlock().Hash, PendingTransactions);
            AddBlock(block);
            PendingTransactions = new List<Transaction>();
        }
        
        public int GetBalance(string address)
        {
            int balance = 0;

            for (int i = 0; i < Chain.Count; i++)
            {
                for (int j = 0; j < Chain[i].Transactions.Count; j++)
                {
                    var transaction = Chain[i].Transactions[j];

                    if (transaction.FromAddress == address)
                    {
                        balance -= transaction.Amount;
                    }

                    if (transaction.ToAddress == address)
                    {
                        balance += transaction.Amount;
                    }
                }
            }

            return balance;
        }
    }
}