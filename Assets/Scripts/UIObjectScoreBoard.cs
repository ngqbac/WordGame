using System.Collections.Generic;
using System.Linq;
using ArcherGame.Game3.Helper;
using UnityEngine;

public class UIObjectScoreBoard : MonoBehaviour
{
    private bool _initialized;
    
    private List<UIObjectScore> _scores;

    public void Initialize()
    {
        _scores = GetComponentsInChildren<UIObjectScore>().ToList();
        _initialized = true;
    }

    public void UpdateScoreBoard(IGame game)
    {
        if (!_initialized) Initialize();
        ItemListHelper.For(_scores).WithCount(GameConfig.Instance.scoreEntries).WithConfiguration((item, index) =>
        {
            item.SetData(game.GetScoreAtPosition(index), game.GetWordEntryAtPosition(index));
        }).Adjust();
    }

    public void ResetScoreBoard()
    {
        if (!_initialized) Initialize();
        ItemListHelper.For(_scores).WithCount(GameConfig.Instance.scoreEntries).WithConfiguration((item, _) =>
        {
            item.SetDefault();
        }).Adjust();
    }
}