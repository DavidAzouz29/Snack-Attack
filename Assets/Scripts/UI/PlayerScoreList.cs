using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScoreList : MonoBehaviour {

	public GameObject playerScoreEntryPrefab;

	ScoreManager scoreManager;
	UILevel r_UILevel;

	int lastChangeCounter;

	// Use this for initialization
	void Start () {
		// TODO: Find is slow - fix for Beta
		scoreManager = GameObject.FindObjectOfType<ScoreManager>();
		r_UILevel = GameObject.FindObjectOfType<UILevel>();
		lastChangeCounter = scoreManager.GetChangeCounter();

		// Set default values
		//for (int i = 0; i < PlayerManager.MAX_PLAYERS; i++) {
		//	r_UILevel.r_PlayerScoresText [i].text = "0";
		//}
	}
	
	// Update is called once per frame
	void Update () {
		if (scoreManager == null) {
			//Debug.LogError("You forgot to add the score manager component to a game object!");
			return;
		}

		if (scoreManager.GetChangeCounter () == lastChangeCounter) {
			// No change since last update!
			return;
		}

		lastChangeCounter = scoreManager.GetChangeCounter ();
		while (this.transform.childCount > 0) {
			Transform c = this.transform.GetChild (0);
			c.SetParent (null);  // Become Batman
			Destroy (c.gameObject);
		}

		string[] names = scoreManager.GetPlayerNames ("kills");
		// Used to index an array of text components 
		int i = 0;
		foreach (string name in names) {
			GameObject go = (GameObject)Instantiate (playerScoreEntryPrefab);
			go.transform.SetParent (this.transform);
			go.transform.Find ("Username").GetComponent<Text> ().text = name;
			go.transform.Find ("Kills").GetComponent<Text> ().text = scoreManager.GetScore (name, "kills").ToString ();
			// Set the 'x' text of 'x' U.I. component to Player 'x' kills.
			//r_UILevel.r_PlayerScoresText [i].text = scoreManager.GetScore (name, "kills").ToString ();
			go.transform.Find ("Deaths").GetComponent<Text> ().text = scoreManager.GetScore (name, "deaths").ToString ();
			go.transform.Find ("Assists").GetComponent<Text> ().text = scoreManager.GetScore (name, "assists").ToString ();
			i++;
		}
	}
}
