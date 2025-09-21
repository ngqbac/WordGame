using System.Collections.Generic;
using System.Linq;
using ArcherGame.Game3.Helper;
using OrganicBeing.Integration;

public class UIObjectScoreBoard : MonoOrganic
{
    private List<UIObjectScore> _scores;

    public void UpdateScoreBoard(IGame game) => WhenReady(() =>
    {
        ItemListHelper.For(_scores).WithCount(GameConfig.Instance.scoreEntries).WithConfiguration((item, index) =>
        {
            item.SetData(game.GetScoreAtPosition(index), game.GetWordEntryAtPosition(index));
        }).Adjust();
    });

    public void ResetScoreBoard() => WhenReady(() =>
    {
        ItemListHelper.For(_scores).WithCount(GameConfig.Instance.scoreEntries).WithConfiguration((item, _) =>
        {
            item.SetDefault();
        }).Adjust();
    });

    protected override void OnGrow()
    {
        _scores = GetComponentsInChildren<UIObjectScore>().ToList();
    }
}