using System.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace Application.Helpers;

public class EncryptionService
{
    private byte[] _key;
    private byte[] _iv;
    private AesCryptoServiceProvider _aes;


    public EncryptionService()
    {
        _key = Convert.FromHexString(AppSettings.Key); //E.g. 256 bit hex string
        _iv = Convert.FromHexString(AppSettings.Iv); //E.g. 128 bit hex string
        _aes = new AesCryptoServiceProvider();


        _aes.Key = _key;
        _aes.IV = _iv;
    }


    public string EncryptMessage(string message)
    {
        byte[] encrypted;

        ICryptoTransform encryptor = _aes.CreateEncryptor();
        // Create the streams used for encryption.
        using (MemoryStream msEncrypt = new MemoryStream())
        {
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    //Write all data to the stream.
                    swEncrypt.Write(message);
                }

                encrypted = msEncrypt.ToArray();
                msEncrypt.Flush();
            }
        }

        return Convert.ToBase64String(encrypted);
    }

    public string DecryptMessage(string encryptedMessage)
    {
        string clearText;
        //Decryption
        ICryptoTransform decrypter = _aes.CreateDecryptor();
        using (MemoryStream
               msDecrypt = new MemoryStream(
                   Convert.FromBase64String(encryptedMessage))) //Remember that encrypted is a byte[]
        {
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decrypter, CryptoStreamMode.Read))
            {
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    clearText = srDecrypt.ReadToEnd();
                }
            }
        }

        return clearText;
    }
}