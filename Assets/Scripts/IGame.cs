public interface IGame 
{ 
	/// <summary>
	/// Submit a word on behalf of a user. A word is accepted if its letters are 
	/// contained in the original string submitted in the constructor, and if it 
	/// is in the word list at CanYouCodeEasy/words
	/// If the word is accepted and its score is high enough, the submission 
	/// should be added to the high score list. If there are multiple submissions 
	/// with the same score, all are accepted, but the first submission with that 
	/// score should rank higher. 
	/// </summary>
	/// <param name="word">Word submitted by the user</param>
	void SubmitWord(string word); 
	
	/// <summary>
	/// Return word entry at given position in the high score list, 0 being the 
	/// highest (best score) and 9 the lowest. 
	/// </summary>
	/// <returns>The word entry at the position in the high score list, 0 being the 
	/// highest (best score) and 9 the lowest, or null if there is no entry at that position</returns>
	/// <param name="position">Position index inot the high score list.</param>
	string GetWordEntryAtPosition(int position);

	/// <summary>
	/// Return the score at given position in the high score list, 0 being the 
	/// highest (best score) and 9 the lowest.
	/// </summary>
	/// <returns>The score at the position in the high score list, or -1 if there is no entry at that position.</returns>
	/// <param name="position">Position index in the high score list.</param>
	int GetScoreAtPosition(int position); 

	/// <returns>
	/// Returns the characters available to compose words with
	/// </returns>
	string GetAvailableLetters(); 

	/// <summary>
	/// Resets the full game. Generating a new list of available characters and cleaning the score board.
	/// </summary>
	void Reset();

	void StartGame(string letters);
}