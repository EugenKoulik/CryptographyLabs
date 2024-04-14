using System.Text;

namespace CryptographyLabs.VigenereCipher;

public static class VigenereCipher 
{
    public static string Encrypt(string decryptedString, string key)
    {
        var encryptedString = new StringBuilder();
        var numberOfCharInKey = 0;

        foreach (var currentChar in decryptedString)
        {
            if (GlobalVariables.RuAlphabet.Contains(currentChar))
            {
                var lastIndex = GlobalVariables.RuAlphabet.IndexOf(currentChar);
                var offset = GlobalVariables.RuAlphabet.IndexOf(key[numberOfCharInKey % key.Length]);
                var newIndex = (lastIndex + offset) % GlobalVariables.RuAlphabet.Count;
                
                encryptedString.Append(GlobalVariables.RuAlphabet[newIndex]);
                
                numberOfCharInKey++;
            }
            else
            {
                encryptedString.Append(currentChar);
            }
        }
        
        return encryptedString.ToString();
    }
    
    public static string Decrypt(string encryptedString, string key)
    {
        var decryptedString = new StringBuilder();
        var numberOfCharInKey = 0;

        foreach (var currentChar in encryptedString)
        {
            if (GlobalVariables.RuAlphabet.Contains(currentChar))
            {
                var lastIndex = GlobalVariables.RuAlphabet.IndexOf(currentChar);
                var offset = GlobalVariables.RuAlphabet.IndexOf(key[numberOfCharInKey % key.Length]);
                var newIndex = (lastIndex - offset + GlobalVariables.RuAlphabet.Count) % GlobalVariables.RuAlphabet.Count;
                
                decryptedString.Append(GlobalVariables.RuAlphabet[newIndex]);
                
                numberOfCharInKey++;
            }
            else
            {
                decryptedString.Append(currentChar);
            }
        }
        
        return decryptedString.ToString();
    }
}