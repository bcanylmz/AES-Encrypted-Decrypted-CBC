using System.Security.Cryptography;
using System.Text;

class Program
{
    static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
    {
        byte[] encrypted;
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }
        return encrypted;
    }

    static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
    {
        string plaintext = null;
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        return plaintext;
    }

    static void Main()
    {
        try
        {
            string originalText = "Barış Can YILMAZ"; // Şifrelenecek metin

            byte[] key;
            byte[] iv;

            Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = Encoding.UTF8.GetBytes("2398736455f36f14");
            aes.IV = Encoding.UTF8.GetBytes("41f63f5546378932");

            // Anahtar ve IV oluşturma
            using (Aes aesAlg = aes)
            {
                key = aesAlg.Key;
                iv = aesAlg.IV;
            }

            byte[] encryptedBytes = EncryptStringToBytes(originalText, key, iv);
            Console.WriteLine("Şifrelenmiş Veri: " + Convert.ToBase64String(encryptedBytes));

            Console.WriteLine("********************************************************");

            string decryptedText = DecryptStringFromBytes(encryptedBytes, key, iv);
            Console.WriteLine("Çözümlenmiş Veri: " + decryptedText);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hata: " + ex.Message);
        }
    }
}
