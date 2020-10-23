using System;
using System.IO;
using System.Security.Cryptography;

namespace Estudos.Exame.Capitulo3.SymmetricAndAsymmetricEncryption
{
    public class AES_Encryption_Decryption
    {
        private static void DumpBytes(string title, byte[] bytes)
        {
            Console.Write(title);
            foreach (var b in bytes)
            {
                Console.Write("{0:X} ", b);
            }

            Console.WriteLine();
        }

        public static void Test()
        {
            var result = Encryption("Test mensagem para criptografar");
            Decryption(result.encryptText, result.key, result.initializationVector);
        }

        private static (byte[] encryptText, byte[] key, byte[] initializationVector) Encryption(string text)
        {
            // Byte array to ecripted message
            byte[] cipherText;

            // Byte array to hold the key that was used for encryption
            byte[] key;

            // Byte array to hold the initialization vector that was used for encryption
            byte[] initializationVector;

            // Create an Aes instance
            // This create a random key and initilization vector
            using (var aes = Aes.Create())
            {
                // Copy the key and the initialization vector
                key = aes.Key;
                initializationVector = aes.IV;

                // Create an ecryptor to encrypt some data
                // should be wrapped in using for production code
                var encryptor = aes.CreateEncryptor();

                // Create a memory stream to receive tha ecrypted data
                using (var encryptMemoryStream = new MemoryStream())
                {
                    // Create a CryptoStream, tell it the stream to write to 
                    // and the encryptor to use. Also set the mode
                    using (var encryptCryptoStream = new CryptoStream(encryptMemoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        // make a stream write from the cryptostream
                        using (var swEncrypt = new StreamWriter(encryptCryptoStream))
                        {
                            swEncrypt.Write(text);
                        }
                        cipherText = encryptMemoryStream.ToArray();
                    }
                }
            }

            // Dumpr out our data
            Console.WriteLine("String to encrypt: {0}", text);
            DumpBytes("Key: ", key);
            DumpBytes("Initilization Vector: ", initializationVector);
            DumpBytes("Encrypted: ", cipherText);

            return (cipherText, key, initializationVector);
        }

        private static void Decryption(byte[] encryptText, byte[] key, byte[] initializationVector)
        {
            using (var aes = Aes.Create())
            {
                // Configure the aes instances with the key and 
                // initialization vector to use for the decryption
                aes.Key = key;
                aes.IV = initializationVector;
                var decryptor = aes.CreateDecryptor();
                using (var decryptStream = new MemoryStream(encryptText))
                {
                    using (var decryptCryptoStream = new CryptoStream(decryptStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(decryptCryptoStream))
                        {
                            var decryptedText = srDecrypt.ReadToEnd();
                            Console.WriteLine(decryptedText);
                        }
                    }
                }
            }
        } 
    }
}