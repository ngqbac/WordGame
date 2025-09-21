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

    public string dataFile = "words";
    public string cacheDataFile = "words_cache";
    public string defaultString = "-";
    public int defaultInt = -1;
    public int scoreEntries = 10;
}