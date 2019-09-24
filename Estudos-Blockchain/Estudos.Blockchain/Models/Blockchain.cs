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
    }
}