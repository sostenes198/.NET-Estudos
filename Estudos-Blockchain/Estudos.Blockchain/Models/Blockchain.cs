using System;
using System.Collections.Generic;

namespace Estudos.Blockchain.Models
{
    public class Blockchain
    {
        public IList<Block> Chain { get; set; }

        public Blockchain()
        {
            InitializeChain();  
            AddGenesisBlock();  
        }

        public void InitializeChain()
        {
            Chain = new List<Block>();
        }

        public Block CreateBlockGenesis() => new Block(DateTime.Now, null, "{}");


        public void AddGenesisBlock() => Chain.Add(CreateBlockGenesis());


        public Block GetLatestBlock() => Chain[Chain.Count - 1];
        
        public void AddBlock(Block block)
        {
            var latestBlock = GetLatestBlock();
            block.Index = ++latestBlock.Index;
            block.PreviousHash = latestBlock.Hash;
            block.Hash = block.CalculateHash();
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
    }
}