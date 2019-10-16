using System;
using System.Collections.Generic;

namespace Estudos.Blockchain.Console
{
    public class Blockchain
    {
        public IList<Transaction> PendingTransactions = new List<Transaction>();
        public int Reward = 1; //1 cryptocurrency

        public IList<Block> Chain { set; get; }
        public int Difficulty { set; get; } = 2;


        public void InitializeChain()
        {
            Chain = new List<Block>();
            AddGenesisBlock();
        }

        public Block CreateGenesisBlock()
        {
            var block = new Block(DateTime.Now, null, PendingTransactions);
            block.Mine(Difficulty);
            PendingTransactions = new List<Transaction>();
            return block;
        }

        public void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }

        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        public void CreateTransaction(Transaction transaction)
        {
            PendingTransactions.Add(transaction);
        }

        public void ProcessPendingTransactions(string minerAddress)
        {
            var block = new Block(DateTime.Now, GetLatestBlock().Hash, PendingTransactions);
            AddBlock(block);

            PendingTransactions = new List<Transaction>();
            CreateTransaction(new Transaction(null, minerAddress, Reward));
        }

        public void AddBlock(Block block)
        {
            var latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            //block.Hash = block.CalculateHash();
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

        public int GetBalance(string address)
        {
            var balance = 0;

            for (var i = 0; i < Chain.Count; i++)
            for (var j = 0; j < Chain[i].Transactions.Count; j++)
            {
                var transaction = Chain[i].Transactions[j];

                if (transaction.FromAddress == address) balance -= transaction.Amount;

                if (transaction.ToAddress == address) balance += transaction.Amount;
            }

            return balance;
        }
    }
}