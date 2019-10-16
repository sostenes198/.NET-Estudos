using System.Collections.Generic;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Estudos.Blockchain.Console
{
    public class P2PServer : WebSocketBehavior
    {
        private bool chainSynched;
        private WebSocketServer wss;

        public void Start()
        {
            wss = new WebSocketServer($"ws://127.0.0.1:{Program.Port}");
            wss.AddWebSocketService<P2PServer>("/Blockchain");
            wss.Start();
            System.Console.WriteLine($"Started server at ws://127.0.0.1:{Program.Port}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "Hi Server")
            {
                System.Console.WriteLine(e.Data);
                Send("Hi Client");
            }
            else
            {
                var newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);

                if (newChain.IsValid() && newChain.Chain.Count > Program.PhillyCoin.Chain.Count)
                {
                    var newTransactions = new List<Transaction>();
                    newTransactions.AddRange(newChain.PendingTransactions);
                    newTransactions.AddRange(Program.PhillyCoin.PendingTransactions);

                    newChain.PendingTransactions = newTransactions;
                    Program.PhillyCoin = newChain;
                }

                if (!chainSynched)
                {
                    Send(JsonConvert.SerializeObject(Program.PhillyCoin));
                    chainSynched = true;
                }
            }
        }
    }
}