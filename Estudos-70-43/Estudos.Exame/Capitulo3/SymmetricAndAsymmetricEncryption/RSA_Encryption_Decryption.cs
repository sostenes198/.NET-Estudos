using System;
using System.Security.Cryptography;
using System.Text;

namespace Estudos.Exame.Capitulo3.SymmetricAndAsymmetricEncryption
{
    public class RSA_Encryption_Decryption
    {
        private static void DumpBytes(string title, byte[] bytes)
        {
            Console.Write(title);
            foreach (var b in bytes)
            {
                Console.Write("{0:X} ", b);
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        public static void Test()
        {
            var plainText = "This is my syper secret data";
            Console.WriteLine("Plain text: {0}", plainText);

            var converter = new ASCIIEncoding();
            var plainBytes = converter.GetBytes(plainText);
            DumpBytes("Plain bytes: ", plainBytes);

            var rsaEncrypt = new RSACryptoServiceProvider();

            var publicKey = rsaEncrypt.ToXmlString(false);
            Console.WriteLine("Public key: {0}", publicKey);
            Console.WriteLine();

            var privateKey = rsaEncrypt.ToXmlString(true);
            Console.WriteLine("Private key: {0}", privateKey);
            Console.WriteLine();

            // Now tell to encryptor to use the public key to encrypt the data
            rsaEncrypt.FromXmlString(publicKey);

            /*
             * Use the encryptor to encrypt the data. The F0AEP parameter
             * specifies how the output is "padded" with extra bytes
             * for maximum compatibility with receiving systems, set this as false
            */
            var encryptedBytes = rsaEncrypt.Encrypt(plainBytes, false);
            DumpBytes("Encrypted bytes: ", encryptedBytes);

            /*
             * Now do the decode - use the private key for this
             * we have sent someone our public key and they
             * have used this to encrypt data that they are sending to us
            */
            var rsaDecrypt = new RSACryptoServiceProvider();
            rsaDecrypt.FromXmlString(privateKey);

            var decryptedBytes = rsaDecrypt.Decrypt(encryptedBytes, false);
            DumpBytes("Decrypted bytes: ", decryptedBytes);
            
            Console.WriteLine("Decrypted string: {0}", converter.GetString(decryptedBytes));
        }
    }
}