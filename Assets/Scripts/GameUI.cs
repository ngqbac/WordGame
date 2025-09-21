using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour 
{
	[SerializeField] private InputField letters;
	[SerializeField] private Button playButton;

	[SerializeField] private GameObject gameplay;
	[SerializeField] private InputField word;
	[SerializeField] private Button submitButton;
	[SerializeField] private Button resetButton;

	private List<UIObjectScore> _scores;
	
	private IGame _wordGame;
	private void Start()
	{
		_wordGame = new WordGame();
		_scores = GetComponentsInChildren<UIObjectScore>().ToList();
		
		gameplay.SetActive(false);
		playButton.gameObject.SetActive(false);

		letters.text = "";
		letters.onValueChanged.AddListener(value =>
		{
			playButton.gameObject.SetActive(!string.IsNullOrEmpty(value));
		});

		ResetGame();
		ResetScoreBoard();
	}

	public void UpdateScoreBoard()
	{
		for (var i = 0; i < _scores.Count; i++)
		{
			_scores[i].SetData(_wordGame.GetScoreAtPosition(i), _wordGame.GetWordEntryAtPosition(i));
		}
	}

	public void ResetScoreBoard()
	{
		foreach (var item in _scores)
		{
			item.SetDefault();
		}
	}

	public void Submit() => _wordGame.SubmitWord(word.text);
	public void StartGame() => _wordGame.StartGame(letters.text);
	public void ResetGame() => _wordGame.Reset();
}