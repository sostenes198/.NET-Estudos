using System;
using Wrapper.Interacao_Proxy;
using Wrapper.Proxy;

namespace Wrapper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //ProxyTeste();
            InteracaoProxyTeste();
        }

        private static void ProxyTeste()
        {
            IDude joe = new NormalDude();
            joe.Name = "Avarage Joe";
            IDude clark = new NormalDude();
            clark.Name = "Clark";
            clark = new SuperDude(clark);
            Console.WriteLine("HERE COME THE BAD-GUYS -- BULLETS ARE FLYING EVERYWHERE\n\n");
            joe.GotShot("Pea Shot");
            clark.GotShot("Bazzoka");
            Console.WriteLine(joe.ToString());
            Console.WriteLine(clark.ToString());
        }

        private static void InteracaoProxyTeste()
        {
            IDude Joe = new NormalDude();
            Joe.Name = "Average Joe";
            var Matt = new Me();
            var JoesHouse = new House();
            JoesHouse.LookAtThing(Matt); // I'm looking at what's in JoesHouse
            Console.WriteLine(Matt.ToString());
            Console.WriteLine(JoesHouse.ToString());
            Matt.ChangeTo = "duck";
            JoesHouse.ChangeThing(Matt);
            Console.WriteLine(Matt.ToString());
            Console.WriteLine(JoesHouse.ToString());
            Console.WriteLine("\nJoe is upset -- his vase is a duck.\n----------------------------------\n\n");
            var Apparition = new MeProxy(Matt);
            Matt.ChangeTo = "bag-o-money";
            JoesHouse.ChangeThing(Apparition);
            Console.WriteLine(Matt.ToString());
            Console.WriteLine(JoesHouse.ToString());
            Console.WriteLine(Apparition.ToString());
        }
    }
}