using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScoreList : MonoBehaviour
{

    public GameObject playerScoreEntryPrefab;

    ScoreManager scoreManager;
    UILevel r_UILevel;

    int lastChangeCounter;
    private string[] namesOgOrder = new string[PlayerManager.MAX_PLAYERS]; // used for colors on the scoreboard
    private bool isFirstTimeRunning = true;

    // Use this for initialization
    void Start()
    {
        scoreManager = GetComponentInParent<ScoreManager>();
        r_UILevel = GameObject.FindObjectOfType<UILevel>(); // TODO: Find is slow - fix for Beta
        lastChangeCounter = scoreManager.GetChangeCounter();
        lastChangeCounter++;
        // Set default values
        for (int i = 0; i < PlayerManager.MAX_PLAYERS; i++)
        {
            r_UILevel.r_PlayerScoresText[i].text = "0";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreManager == null)
        {
            //Debug.LogError("You forgot to add the score manager component to a game object!");
            return;
        }

        if (scoreManager.GetChangeCounter() == lastChangeCounter)
        {
            // No change since last update!
            return;
        }

        lastChangeCounter = scoreManager.GetChangeCounter();
        while (this.transform.childCount > 0)
        {
            Transform c = this.transform.GetChild(0);
            c.SetParent(null);  // Become Batman
            Destroy(c.gameObject);
        }

        string[] names = scoreManager.GetPlayerNames("kills");
        if (isFirstTimeRunning)
        {
            namesOgOrder = scoreManager.GetOriginalOrder(); // used for colors on the scoreboard
            isFirstTimeRunning = false;
        }

        // Used to index an array of text components 
        int i = 0;
        foreach (string name in names)
        {
            GameObject go = (GameObject)Instantiate(playerScoreEntryPrefab);
            go.transform.SetParent(this.transform);
            go.transform.localScale = new Vector3(1, 1);
            // "0th" player etc
            if (name == namesOgOrder[0])
            {
                go.GetComponent<Image>().color = GameSettings.Instance.players[0].Color;
            }
            else if (name == namesOgOrder[1])
            {
                go.GetComponent<Image>().color = GameSettings.Instance.players[1].Color;
            }
            else if (name == namesOgOrder[2])
            {
                go.GetComponent<Image>().color = GameSettings.Instance.players[2].Color;
            }
            else if (name == namesOgOrder[3])
            {
                go.GetComponent<Image>().color = GameSettings.Instance.players[3].Color;
            }
            go.transform.Find("Username").GetComponent<Text>().text = name;
            go.transform.Find("Kills").GetComponent<Text>().text = scoreManager.GetScore(name, "kills").ToString();
            // Set the 'x' text of 'x' U.I. component to Player 'x' kills.
            //r_UILevel.r_PlayerScoresText [i].text = scoreManager.GetScore (name, "kills").ToString ();
            go.transform.Find("Deaths").GetComponent<Text>().text = scoreManager.GetScore(name, "deaths").ToString();
            i++;
        }
    }
}
