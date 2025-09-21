using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "WordGame/Config")]
[PreferBinarySerialization]
public class GameConfig : ScriptableObject
{
    private static GameConfig _instance;

    public static GameConfig Instance
    {
        get
        {
            if (_instance == null) _instance = Resources.Load<GameConfig>("GameConfig");
            return _instance;
        }
    }

    private int _totalCharacters = -1;

    public int TotalCharacters
    {
        get
        {
            if (_totalCharacters != -1) return _totalCharacters;
            var character = Resources.Load<TextAsset>(characterDataFile);
            _totalCharacters = character.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
            return _totalCharacters;
        }
    }

    public string characterDataFile = "characters";
    public string dataFile = "words";
    public string cacheDataFile = "words_cache";
    public string defaultString = "-";
    public int defaultInt = -1;
    public int scoreEntries = 10;
}