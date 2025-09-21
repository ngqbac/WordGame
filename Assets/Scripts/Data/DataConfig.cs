using System;

namespace Data
{
    [Serializable]
    public class WordEntry
    {
        public string word;
        public int[] signature;
    }
    
    [Serializable]
    public class GameScore
    {
        public string word;
        public int score;

        public GameScore(string wordIn)
        {
            word = wordIn;
            score = wordIn.Length;
        }
    }
}