/// <summary>
/// Score manager.
/// TODO: watch the video on how this should be used 
/// viewed: Quill18 https://youtu.be/gjFsrVWQaQw 
/// Notes:
/// - Look in PlayerScoreList for displaying scores on the U.I.
/// </summary>

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScoreManager : MonoBehaviour {

    Dictionary<string, Dictionary<string, int>> playerScores;
	int changeCounter = -1;
    private string[] namesOgOrder = new string[PlayerManager.MAX_PLAYERS]; // used for colors on the scoreboard

    void Start()
    {
        Init();

        //Player Setup
        for (int i = 0; i < PlayerManager.MAX_PLAYERS; i++)
        {
            // This is used so two characters don't share the same name.
            string sPlayerTag = "P" + (i + 1) + " " + GameSettings.Instance.players[i].ClassName; // sPlayerTag;
            
            SetScore(sPlayerTag, "kills", 0);
            SetScore(sPlayerTag, "deaths", 0);
        }
        changeCounter++; // Updates the scoreboard once.
        namesOgOrder = GetPlayerNames();
    }

    // Init this instance.
    void Init() {
        if (playerScores != null)
			return;

		playerScores = new Dictionary<string, Dictionary<string, int>>();
	}

	public void Reset() {
		changeCounter++;
		playerScores = null;
	}

	//------------------------------------------------------
	// How the outside world will interact with our program
	//------------------------------------------------------

	public int GetScore(string username, string scoreType) {
		Init ();

		if(playerScores.ContainsKey(username) == false) {
			// We have no score record at all for this username
			return 0;
		}

		if(playerScores[username].ContainsKey(scoreType) == false) {
			return 0;
		}

		return playerScores[username][scoreType];
	}

	public void SetScore(string username, string scoreType, int value) {
		Init ();

		changeCounter++;

		if(playerScores.ContainsKey(username) == false) {
			playerScores[username] = new Dictionary<string, int>();
		}

		playerScores[username][scoreType] = value;
	}

	// Changes the score.
	public void ChangeScore(string username, string scoreType, int amount) {
		Init ();
		int currScore = GetScore(username, scoreType);
		SetScore(username, scoreType, (currScore + amount));
	}

	public string[] GetPlayerNames() {
		Init ();
		return playerScores.Keys.ToArray();
	}
	
	public string[] GetPlayerNames(string sortingScoreType) {
		Init ();
		return playerScores.Keys.OrderByDescending( n => GetScore(n, sortingScoreType) ).ToArray();
	}

    public string[] GetOriginalOrder()
    {
        return namesOgOrder;
    }

	public int GetChangeCounter() {
		return changeCounter;
	}

    /*public void PrototypeStartup()
    {
        for (int i = 0; i < 4; i++)
        {
            //Player Setup
            SetScore("Player "+ (i+ 1), "kills", 0);
            SetScore("Player " + (i + 1), "deaths", 0);
        }
    } */

}
