using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace App
{
    class Crypto
    {
        public static string DecryptAsyemetricMessage(string encrypted, RSAParameters privateKey)
        {
            try
            {
                byte[] cyphertext = Convert.FromBase64String(encrypted);

                RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                csp.ImportParameters(privateKey);

                byte[] plainText = csp.Decrypt(cyphertext, false);

                return Encoding.UTF8.GetString(plainText);
            } catch
            {
                return null;
            }
        }

        public static string EncryptSyemetricMessage(string message, string sharedKey)
        {
            byte[] array;  
  
            using (Aes aes = Aes.Create())  
            {  
                aes.Key = Encoding.UTF8.GetBytes(sharedKey.Substring(16));  
                aes.IV = Encoding.UTF8.GetBytes(sharedKey.Substring(0, 16));  
  
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);  
  
                using (MemoryStream ms = new MemoryStream())  
                {  
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))  
                    {  
                        using (StreamWriter writer = new StreamWriter(cs))  
                        {  
                            writer.Write(message);  
                        }  
  
                        array = ms.ToArray();  
                    }  
                }  
            }  
  
            return Convert.ToBase64String(array);
        }

        public static string DecryptSyemetricMessage(string cipherText, string sharedKey) {
            try
            {
                Aes aes = Aes.Create();
                aes.IV = Encoding.UTF8.GetBytes(sharedKey.Substring(0, 16));
                aes.Key = Encoding.UTF8.GetBytes(sharedKey.Substring(16));
                
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cipherText));
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cryptoStream);

                string message = reader.ReadToEnd();

                reader.Dispose();
                cryptoStream.Dispose();
                memoryStream.Dispose();
                decryptor.Dispose();

                return message;
            }
            catch
            {
                return null;
            }
        }

        public static string HashSha256(string input)
        {
            SHA256 s = SHA256.Create();
            input = BitConverter.ToString(s.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", string.Empty);
            s.Dispose();

            return input.ToLower();
        }
    }

    class RSAKeys
    {
        private RSACryptoServiceProvider csp;
        private RSAParameters privateKey;
        private RSAParameters publicKey;
        public RSAParameters PrivateKey { get => privateKey; }
        public RSAParameters PublicKey { get => publicKey; }

        public RSAKeys()
        {
            csp = new RSACryptoServiceProvider(2048);

            privateKey = csp.ExportParameters(true);
            publicKey = csp.ExportParameters(false);
        }

        //https://gist.github.com/therightstuff/aa65356e95f8d0aae888e9f61aa29414
        public string GetPublicKey()
        {
            StringWriter outputStream = new StringWriter();
            RSAParameters parameters = csp.ExportParameters(false);

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            MemoryStream innerStream = new MemoryStream();
            BinaryWriter innerWriter = new BinaryWriter(innerStream);

            MemoryStream bitStringStream = new MemoryStream();
            BinaryWriter bitStringWriter = new BinaryWriter(bitStringStream);

            MemoryStream paramsStream = new MemoryStream();
            BinaryWriter paramsWriter = new BinaryWriter(paramsStream);

            writer.Write((byte)0x30);
            innerWriter.Write((byte)0x30);

            EncodeLength(innerWriter, 13);
            innerWriter.Write((byte)0x06);

            EncodeLength(innerWriter, 9);
            innerWriter.Write(new byte[] { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01 });
            innerWriter.Write((byte)0x05);

            EncodeLength(innerWriter, 0);
            innerWriter.Write((byte)0x03);
            bitStringWriter.Write((byte)0x00);
            bitStringWriter.Write((byte)0x30);
            EncodeIntegerBigEndian(paramsWriter, parameters.Modulus);
            EncodeIntegerBigEndian(paramsWriter, parameters.Exponent);

            EncodeLength(bitStringWriter, (int)paramsStream.Length);
            bitStringWriter.Write(paramsStream.GetBuffer(), 0, (int)paramsStream.Length);

            EncodeLength(innerWriter, (int)bitStringStream.Length);
            innerWriter.Write(bitStringStream.GetBuffer(), 0, (int)bitStringStream.Length);

            EncodeLength(writer, (int)innerStream.Length);
            writer.Write(innerStream.GetBuffer(), 0, (int)innerStream.Length);

            char[] base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();

            outputStream.Write("-----BEGIN PUBLIC KEY-----\n");

            for (int i = 0; i < base64.Length; i += 64)
            {
                outputStream.Write(base64, i, Math.Min(64, base64.Length - i));
                outputStream.Write("\n");
            }

            outputStream.Write("-----END PUBLIC KEY-----");

            string pem = outputStream.ToString();

            stream.Dispose();
            writer.Dispose();

            innerStream.Dispose();
            innerWriter.Dispose();

            bitStringStream.Dispose();
            bitStringWriter.Dispose();

            paramsStream.Dispose();
            paramsWriter.Dispose();

            return pem;
        }

        private static void EncodeLength(BinaryWriter stream, int length)
        {
            if (length < 0x80)
                stream.Write((byte)length);
            else
            {
                int temp = length;
                int bytesRequired = 0;

                while (temp > 0)
                {
                    temp >>= 8;
                    bytesRequired++;
                }

                stream.Write((byte)(bytesRequired | 0x80));

                for (int i = bytesRequired - 1; i >= 0; i--)
                    stream.Write((byte)(length >> (8 * i) & 0xff));
            }
        }

        private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value)
        {
            stream.Write((byte)0x02);

            int prefixZeros = 0;

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] != 0)
                    break;

                prefixZeros++;
            }

            if (value.Length - prefixZeros == 0)
            {
                EncodeLength(stream, 1);
                stream.Write((byte)0);
            }
            else
            {
                if (value[prefixZeros] > 0x7f)
                {
                    EncodeLength(stream, value.Length - prefixZeros + 1);
                    stream.Write((byte)0);
                }
                else
                    EncodeLength(stream, value.Length - prefixZeros);

                for (int i = prefixZeros; i < value.Length; i++)
                    stream.Write(value[i]);
            }
        }
    }
}
