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

	[SerializeField] private UIObjectScoreBoard scoreBoard;
	
	private IGame _wordGame;
	private void Start()
	{
		_wordGame = new WordGame();
		
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

	public void UpdateScoreBoard() => scoreBoard.UpdateScoreBoard(_wordGame);
	public void ResetScoreBoard() => scoreBoard.ResetScoreBoard();
	public void Submit() => _wordGame.SubmitWord(word.text);
	public void StartGame() => _wordGame.StartGame(letters.text);
	public void ResetGame() => _wordGame.Reset();
}