using UnityEngine;
using UnityEngine.UI;

public class UIObjectScore : MonoBehaviour
{
    [SerializeField] private Text score;
    [SerializeField] private Text word;

    public void SetData(int scoreIn, string wordIn)
    {
        score.text = scoreIn == GameConfig.Instance.defaultInt ? GameConfig.Instance.defaultString : $"{scoreIn}";
        word.text = string.IsNullOrEmpty(wordIn) ? GameConfig.Instance.defaultString : wordIn;
    }

    public void SetDefault()
    {
        score.text = GameConfig.Instance.defaultString;
        word.text = GameConfig.Instance.defaultString;
    }
}