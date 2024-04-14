using System.Text;

namespace CryptographyLabs.DES;

public class DES
{
    private readonly int _shiftKey;
    private readonly int _sizeOfBlock;
    private readonly int _sizeOfChar;
    private readonly int _quantityOfRounds;

    public DES(int shiftKey = 2, int sizeOfBlock = 128, int sizeOfChar = 16, int quantityOfRounds = 16)
    {
        _shiftKey = shiftKey;
        _sizeOfChar = sizeOfChar;
        _sizeOfBlock = sizeOfBlock;
        _quantityOfRounds = quantityOfRounds;
    }
    
    public string Encrypt(string decryptedString, string key)
    {
        var rightLenghtString = StringToRightLength(decryptedString);
        var blocks = CutStringIntoBlocks(rightLenghtString);

        var correctKeyWord = StringToBinaryFormat(CorrectKeyWord(key, rightLenghtString.Length / (2 * blocks.Count)));
        
        for (var j = 0; j < _quantityOfRounds; j++)
        {
            for (var i = 0; i < blocks.Count; i++)
            {
                blocks[i] = EncodeOneRound(blocks[i], correctKeyWord);
            }
 
            correctKeyWord = KeyToNextRound(correctKeyWord);
        }

        correctKeyWord = KeyToPrevRound(correctKeyWord);

        return StringFromBinaryToNormalFormat(correctKeyWord);
    }

    public string Decrypt(string encryptedString, string key)
    {
        var binaryString = StringToBinaryFormat(encryptedString);
        var blocks = CutBinaryStringIntoBlocks(binaryString);
        var correctKeyWord = StringToBinaryFormat(CorrectKeyWord(key, encryptedString.Length / (2 * blocks.Count)));
        
        for (var j = 0; j < _quantityOfRounds; j++)
        {
            for (var i = 0; i < blocks.Count; i++)
            {
                blocks[i] = DecodeOneRound(blocks[i], correctKeyWord);
            }
 
            correctKeyWord = KeyToPrevRound(correctKeyWord);
        }
 
        correctKeyWord = KeyToNextRound(correctKeyWord);

        return StringFromBinaryToNormalFormat(correctKeyWord);
    }
    
    private string DecodeOneRound(string input, string key)
    {
        var l = input[..(input.Length / 2)];
        var r = input.Substring(input.Length / 2, input.Length / 2);
 
        return Xor(Xor(l, key), r) + l;
    }
    
    private List<string> CutBinaryStringIntoBlocks(string input)
    {
        var blocks = new string[input.Length / _sizeOfBlock];
 
        var lengthOfBlock = input.Length / blocks.Length;

        for (var i = 0; i < blocks.Length; i++)
        {
            blocks[i] = input.Substring(i * lengthOfBlock, lengthOfBlock);
        }

        return blocks.ToList();
    }
    
    private string StringFromBinaryToNormalFormat(string input)
    {
        var output = new StringBuilder();
 
        while (input.Length > 0)
        {
            var charBinary = input[.._sizeOfChar];
            input = input.Remove(0, _sizeOfChar);

            var degree = charBinary.Length - 1;

            var a = charBinary.Sum(c => Convert.ToInt32(c.ToString()) * (int)Math.Pow(2, degree--));

            output.Append((char)a);
        }
 
        return output.ToString();
    }
    
    private string KeyToPrevRound(string key)
    {
        for (var i = 0; i < _shiftKey; i++)
        {
            key += key[0];
            key = key.Remove(0, 1);
        }
 
        return key;
    }
    
    private string KeyToNextRound(string key)
    {
        for (var i = 0; i < _shiftKey; i++)
        {
            key = key[^1] + key;
            key = key.Remove(key.Length - 1);
        }
 
        return key;
    }
    
    private string EncodeOneRound(string input, string key)
    {
        var l = input[..(input.Length / 2)];
        var r = input.Substring(input.Length / 2, input.Length / 2);
 
        return r + Xor(l, Xor(r, key));
    }
    
    private string Xor(string s1, string s2)
    {
        var result = new StringBuilder();
 
        for (var i = 0; i < s1.Length; i++)
        {
            var a = Convert.ToBoolean(Convert.ToInt32(s1[i].ToString()));
            var b = Convert.ToBoolean(Convert.ToInt32(s2[i].ToString()));

            if (a ^ b)
            {
                result.Append('1');
            }
            else
            {
                result.Append('0');
            }
        }
        return result.ToString();
    }
    
    private string CorrectKeyWord(string input, int lengthKey)
    {
        var output = new StringBuilder(input);
        
        if (output.Length > lengthKey)
        {
            output.Append(input[..lengthKey]);
        }
        else
        {
            while (output.Length < lengthKey)
            {
                output.Insert(0, "0");
            }
        }
 
        return output.ToString();
    }

    private List<string> CutStringIntoBlocks(string input)
    {
        var blocks = new string[input.Length * _sizeOfChar / _sizeOfBlock];

        var lengthOfBlock = input.Length / blocks.Length;

        for (var i = 0; i < blocks.Length; i++)
        {
            blocks[i] = input.Substring(i * lengthOfBlock, lengthOfBlock);
            blocks[i] = StringToBinaryFormat(blocks[i]);
        }

        return blocks.ToList();
    }

    private string StringToBinaryFormat(string input)
    {
        var output = new StringBuilder();

        foreach (var currentChar in input)
        {
            var charBinary = new StringBuilder(Convert.ToString(currentChar, 2));

            while (charBinary.Length < _sizeOfChar)
            {
                charBinary.Insert(0, "0");
            }

            output.Append(charBinary);
        }

        return output.ToString();
    }

    private string StringToRightLength(string input)
    {
        var rightLenghtString = new StringBuilder(input);

        while (rightLenghtString.Length * _sizeOfChar % _sizeOfBlock != 0)
        {
            rightLenghtString.Append('#');
        }

        return rightLenghtString.ToString();
    }
}