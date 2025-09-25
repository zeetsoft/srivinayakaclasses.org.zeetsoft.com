using System.Security.Cryptography;
using System.Text;

namespace StairwayDesigns.Models.SecureContent;

public static class EncryptionHelper
{
    public static string Encrypt(string clearText)
    {
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        clearText = clearText.Replace("!", "#1#");
        clearText = clearText.Replace("@", "#2#");
        clearText = clearText.Replace("$", "#4#");
        clearText = clearText.Replace("%", "#5#");
        clearText = clearText.Replace("^", "#6#");
        clearText = clearText.Replace("&", "#7#");
        clearText = clearText.Replace("*", "#8#");
        clearText = clearText.Replace("(", "#9#");
        clearText = clearText.Replace(")", "#10#");
        clearText = clearText.Replace("-", "#11#");
        clearText = clearText.Replace("+", "#12#");
        clearText = clearText.Replace("/", "#13#");
        clearText = clearText.Replace("\\", "#14#");
        clearText = clearText.Replace("{", "#15#");
        clearText = clearText.Replace("}", "#16#");
        return clearText;
    }

    public static string Decrypt(string cipherText, string ArgCharToFind, string ArgCharToReplaceWith)
    {
        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText.Replace(ArgCharToFind, ArgCharToReplaceWith);
    }
}