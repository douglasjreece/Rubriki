using System.Security.Cryptography;
using System.Text;

namespace Rubriki.Website.Authentication;

public static class Encrypter
{
    /// <summary>
    /// Encrypts a stream using AES encryption
    /// </summary>
    /// <param name="inputStream">Stream to encrypt</param>
    /// <param name="outputStream">Stream to write encrypted data to</param>
    /// <param name="key">Encryption key</param>
    /// <param name="iv">Initialization vector</param>
    public static void Encrypt(Stream inputStream, Stream outputStream, byte[] key, byte[] iv)
    {
        if (inputStream == null)
            throw new ArgumentNullException(nameof(inputStream));
        if (outputStream == null)
            throw new ArgumentNullException(nameof(outputStream));
        if (key == null || key.Length <= 0)
            throw new ArgumentNullException(nameof(key));
        if (iv == null || iv.Length <= 0)
            throw new ArgumentNullException(nameof(iv));

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (CryptoStream cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write))
            {
                inputStream.CopyTo(cryptoStream);
            }
        }
    }

    /// <summary>
    /// Decrypts a stream using AES encryption
    /// </summary>
    /// <param name="inputStream">Stream containing encrypted data</param>
    /// <param name="outputStream">Stream to write decrypted data to</param>
    /// <param name="key">Encryption key</param>
    /// <param name="iv">Initialization vector</param>
    public static void Decrypt(Stream inputStream, Stream outputStream, byte[] key, byte[] iv)
    {
        if (inputStream == null)
            throw new ArgumentNullException(nameof(inputStream));
        if (outputStream == null)
            throw new ArgumentNullException(nameof(outputStream));
        if (key == null || key.Length <= 0)
            throw new ArgumentNullException(nameof(key));
        if (iv == null || iv.Length <= 0)
            throw new ArgumentNullException(nameof(iv));

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (CryptoStream cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
            {
                cryptoStream.CopyTo(outputStream);
            }
        }
    }

    /// <summary>
    /// Helper method to encrypt a string and return a Base64 encoded string
    /// </summary>
    public static string EncryptStringToBase64(string plainText, byte[] key, byte[] iv)
    {
        if (string.IsNullOrEmpty(plainText))
            throw new ArgumentException("Value cannot be null or empty", nameof(plainText));

        using (MemoryStream msInput = new MemoryStream(Encoding.UTF8.GetBytes(plainText)))
        using (MemoryStream msOutput = new MemoryStream())
        {
            Encrypt(msInput, msOutput, key, iv);
            return Convert.ToBase64String(msOutput.ToArray());
        }
    }

    public static string EncryptStringToBase64(string plainText, Guid key, Guid iv)
    {
        return EncryptStringToBase64(plainText, key.ToByteArray(), iv.ToByteArray());
    }

    /// <summary>
    /// Helper method to decrypt a Base64 encoded string back to plain text
    /// </summary>
    public static string DecryptStringFromBase64(string cipherText, byte[] key, byte[] iv)
    {
        if (string.IsNullOrEmpty(cipherText))
            throw new ArgumentException("Value cannot be null or empty", nameof(cipherText));

        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        using (MemoryStream msInput = new MemoryStream(cipherBytes))
        using (MemoryStream msOutput = new MemoryStream())
        {
            Decrypt(msInput, msOutput, key, iv);
            return Encoding.UTF8.GetString(msOutput.ToArray());
        }
    }

    public static string DecryptStringFromBase64(string cipherText, Guid key, Guid iv)
    {
        return DecryptStringFromBase64(cipherText, key.ToByteArray(), iv.ToByteArray());
    }
}
