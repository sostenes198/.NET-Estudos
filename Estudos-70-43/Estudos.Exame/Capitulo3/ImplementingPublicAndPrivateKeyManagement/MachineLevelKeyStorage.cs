using System;
using System.Security.Cryptography;

namespace Estudos.Exame.Capitulo3.ImplementingPublicAndPrivateKeyManagement
{
    public class MachineLevelKeyStorage
    {
        public static void Test()
        {
            var cspParams = new CspParameters
            {
                KeyContainerName = "Machine Level Key",
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            
            var rsa = new RSACryptoServiceProvider(cspParams);
            Console.WriteLine(rsa.ToXmlString(false));

            rsa.PersistKeyInCsp = true;
            rsa.Clear();
        }
    }
}