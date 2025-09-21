using System.Linq;

public static class WordSignatureUtils
{
    public static int[] GetSignature(string word)
    {
        var signature = new int[GameConfig.Instance.TotalCharacters];
        foreach (var c in word.Where(char.IsLetter))
        {
            signature[c - 'a']++;
        }
        return signature;
    }
    
    public static bool CanBuildWord(int[] wordSig, int[] poolSig)
    {
        for (var i = 0; i < GameConfig.Instance.TotalCharacters; i++)
        {
            if (wordSig[i] > poolSig[i]) return false;
        }
        return true;
    }
}