using System;
using System.Collections.Generic;
using System.Linq;
using Data;

public class WordGame : IGame
{
	private readonly List<WordEntry> _allWords;
	private readonly HashSet<string> _usedWords = new();
	private HashSet<string> _validWordsSet = new();
	private List<GameScore> _highScores = new();
	private int[] _letterPool = Array.Empty<int>();

	public WordGame()
	{
		_allWords = DataManager.LoadData();
		Reset();
	}
	
	public void StartGame(string letters)
	{
		_letterPool = WordSignatureUtils.GetSignature(letters);
		
		_usedWords.Clear();
		_highScores.Clear();

		_validWordsSet = _allWords
			.Where(entry => WordSignatureUtils.CanBuildWord(entry.signature, _letterPool))
			.Select(entry => entry.word)
			.ToHashSet();
	}
	
	public void SubmitWord(string word)
	{
		word = word.ToLowerInvariant();
		if (_usedWords.Contains(word) || !_validWordsSet.Contains(word)) return;

		var signature = WordSignatureUtils.GetSignature(word);
		if (!WordSignatureUtils.CanBuildWord(signature, _letterPool)) return;

		_usedWords.Add(word);
		// for (var i = 0; i < GameConfig.Instance.TotalCharacters; i++) _letterPool[i] -= signature[i];

		_highScores.Add(new GameScore(word));
		_highScores = _highScores
			.OrderByDescending(s => s.score)
			.ThenBy(s => _usedWords.ToList().IndexOf(s.word))
			.Take(GameConfig.Instance.scoreEntries)
			.ToList();
	}

	public string GetWordEntryAtPosition(int position) => position >= 0 && position < _highScores.Count ? _highScores[position].word : null;

	public int GetScoreAtPosition(int position) => position >= 0 && position < _highScores.Count ? _highScores[position].score : -1;

	public string GetAvailableLetters ()
	{
		return "AvailableCharactersNotImplementedYet";
	}

	public void Reset()
	{
		_usedWords.Clear();
		_highScores.Clear();
		_validWordsSet.Clear();
		_letterPool = Array.Empty<int>();
	}
}