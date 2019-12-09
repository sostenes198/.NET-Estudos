using System.Collections.Generic;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Estudos.Blockchain.Console
{
    public class P2PClient
    {
        private readonly IDictionary<string, WebSocket> wsDict = new Dictionary<string, WebSocket>();

        public void Connect(string url)
        {
            if (!wsDict.ContainsKey(url))
            {
                var ws = new WebSocket(url);
                ws.OnMessage += (sender, e) =>
                {
                    if (e.Data == "Hi Client")
                    {
                        System.Console.WriteLine(e.Data);
                    }
                    else
                    {
                        var newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);
                        if (newChain.IsValid() && newChain.Chain.Count > Program.ChainCoin.Chain.Count)
                        {
                            var newTransactions = new List<Transaction>();
                            newTransactions.AddRange(newChain.PendingTransactions);
                            newTransactions.AddRange(Program.ChainCoin.PendingTransactions);

                            newChain.PendingTransactions = newTransactions;
                            Program.ChainCoin = newChain;
                        }
                    }
                };
                ws.Connect();
                ws.Send("Hi Server");
                ws.Send(JsonConvert.SerializeObject(Program.ChainCoin));
                wsDict.Add(url, ws);
            }
        }

        public void Send(string url, string data)
        {
            foreach (var item in wsDict)
                if (item.Key == url)
                    item.Value.Send(data);
        }

        public void Broadcast(string data)
        {
            foreach (var item in wsDict) item.Value.Send(data);
        }

        public IList<string> GetServers()
        {
            IList<string> servers = new List<string>();
            foreach (var item in wsDict) servers.Add(item.Key);
            return servers;
        }

        public void Close()
        {
            foreach (var item in wsDict) item.Value.Close();
        }
    }
}