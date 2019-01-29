using System;
using System.Security.Cryptography;

namespace Estudos.Exame.Capitulo3.ImplementingPublicAndPrivateKeyManagement
{
    public class UserLevelKeyStorage
    {
        public static void Test()
        {
            var containerName = "MyKeyStore";
            var csp = new CspParameters
            {
                KeyContainerName = containerName
            };
            
            var rsaStore = new RSACryptoServiceProvider(csp);
            Console.WriteLine("Stored keys: {0} \n\n",
                rsaStore.ToXmlString(true));
            

            var rsaLoad = new RSACryptoServiceProvider(csp);
            Console.WriteLine("Loaded keys: {0}",
                rsaLoad.ToXmlString(true));
            
            // To Delete a stored keys
            rsaStore.PersistKeyInCsp = false;
            rsaStore.Clear();
        }
    }
}