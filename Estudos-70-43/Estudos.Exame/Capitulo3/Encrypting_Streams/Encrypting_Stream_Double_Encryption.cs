using System.IO;
using System.Security.Cryptography;

namespace Estudos.Exame.Capitulo3.Encrypting_Streams
{
    public class Encrypting_Stream_Double_Encryption
    {
        public static void Test()
        {
            var plainText = "This is my super secret data";

            byte[] encryptedText, key1, key2, initializationVector1, initializationVector2;

            using (var aes1 = Aes.Create())
            {
                key1 = aes1.Key;
                initializationVector1 = aes1.IV;
                var encryptor1 = aes1.CreateEncryptor();

                using (var encryptMemoryStream = new MemoryStream())
                {
                    using (var cryptoStream1 = new CryptoStream(encryptMemoryStream, encryptor1, CryptoStreamMode.Write))
                    {
                        using (var aes2 = Aes.Create())
                        {
                            key2 = aes2.Key;
                            initializationVector2 = aes1.IV;
                            var encryptor2 = aes1.CreateEncryptor();
                            using (var cryptoStream2 = new CryptoStream(cryptoStream1, encryptor2, CryptoStreamMode.Write))
                            {
                                using (var swEncrypt = new StreamWriter(cryptoStream2))
                                {
                                    swEncrypt.Write(plainText);
                                }

                                encryptedText = encryptMemoryStream.ToArray();
                            }
                        }
                    }
                }
            }
        }
    }
}