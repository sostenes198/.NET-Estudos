using Newtonsoft.Json;

namespace Estudos.Blockchain.Console
{
    internal class Program
    {
        public static int Port;
        public static P2PServer Server;
        public static P2PClient Client = new P2PClient();
        public static Blockchain ChainCoin = new Blockchain();
        public static string name = "Unknown";

        private static void Main(string[] args)
        {
            ChainCoin.InitializeChain();

            if (args.Length >= 1)
                Port = int.Parse(args[0]);
            if (args.Length >= 2)
                name = args[1];

            if (Port > 0)
            {
                Server = new P2PServer();
                Server.Start();
            }

            if (name != "Unkown") System.Console.WriteLine($"Current user is {name}");

            System.Console.WriteLine("=========================");
            System.Console.WriteLine("1. Connect to a server");
            System.Console.WriteLine("2. Add a transaction");
            System.Console.WriteLine("3. Display Blockchain");
            System.Console.WriteLine("4. Exit");
            System.Console.WriteLine("=========================");

            var selection = 0;
            while (selection != 4)
            {
                switch (selection)
                {
                    case 1:
                        System.Console.WriteLine("Please enter the server URL");
                        var serverURL = System.Console.ReadLine();
                        Client.Connect($"{serverURL}/Blockchain");
                        break;
                    case 2:
                        System.Console.WriteLine("Please enter the receiver name");
                        var receiverName = System.Console.ReadLine();
                        System.Console.WriteLine("Please enter the amount");
                        var amount = System.Console.ReadLine();
                        ChainCoin.CreateTransaction(new Transaction(name, receiverName, int.Parse(amount)));
                        ChainCoin.ProcessPendingTransactions(name);
                        Client.Broadcast(JsonConvert.SerializeObject(ChainCoin));
                        break;
                    case 3:
                        System.Console.WriteLine("Blockchain");
                        System.Console.WriteLine(JsonConvert.SerializeObject(ChainCoin, Formatting.Indented));
                        break;
                }

                System.Console.WriteLine("Please select an action");
                var action = System.Console.ReadLine();
                selection = int.Parse(action);
            }

            Client.Close();
        }
    }
}