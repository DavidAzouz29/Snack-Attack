using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UILevel : MonoBehaviour {

	public RoundTimer r_RoundTimer;
	public Text[] r_PlayerScoresText = new Text[PlayerManager.MAX_PLAYERS];

    [SerializeField]
    private Text c_timer;

    [SerializeField]
    private Image[] c_playerIcons = new Image[PlayerManager.MAX_PLAYERS];

    float m_Mins;
    float m_Secs;

	// Use this for initialization
	void Start ()
    {
        r_RoundTimer = FindObjectOfType<RoundTimer>();
        //r_text.GetComponentInChildren<Text>();
        for (int i = 0; i < PlayerManager.MAX_PLAYERS; i++)
        {
            // Get the neut icon as the default
            c_playerIcons[i].sprite = GameSettings.Instance.players[i].Brain.GetIcon(1);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        SetClockText();

    }

    void SetClockText()
    {
        m_Mins = Mathf.Floor(r_RoundTimer.GetTimeRemaining() / 60); // Get Minutes Remaining
        m_Secs = Mathf.Floor(r_RoundTimer.GetTimeRemaining() % 60);  // Get Seconds Remaining

        c_timer.text = (m_Mins.ToString() + ":" + m_Secs.ToString()); // Display the time remaining
        if (m_Secs < 10.0f) // If seconds aren't in the 10s, display a 0 before the second.
        {
            c_timer.text = (m_Mins.ToString() + ":" + "0" + m_Secs.ToString());
        }
        else if (m_Secs <= 0.3f)
        {
            c_timer.text = "!!!!";
        }
    }

    /// <summary>
    /// Switches Icons when their state changes: i.e. evolution or damaged.
    /// </summary>
    /// <param name="a_chosenPlayer">Player's icon to change.</param>
    /// <param name="a_eTransitionState">State to return to.</param>
    /// <param name="a_isDead">Are they dead?</param>
    /// <returns></returns>
    public IEnumerator UpdateIcon(int a_chosenPlayer, BossBlobs.TransitionState a_eTransitionState, bool a_isDead)
    {
        c_playerIcons[a_chosenPlayer].sprite = GameSettings.Instance.players[a_chosenPlayer].Brain.GetIcon(3);
        // If they're dead
        if(a_isDead)
        {
            //"deactivate" their icon: i.e. apply a red tint.
            c_playerIcons[a_chosenPlayer].color = GameSettings.Instance.players[0].Color;
        }
        yield return new WaitForSeconds(0.7f);
        c_playerIcons[a_chosenPlayer].color += Color.white;
        c_playerIcons[a_chosenPlayer].sprite = GameSettings.Instance.players[a_chosenPlayer].Brain.GetIcon((int)a_eTransitionState);
        yield return null;
    }
}
