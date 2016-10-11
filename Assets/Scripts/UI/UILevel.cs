using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UILevel : MonoBehaviour {

	public RoundTimer r_RoundTimer;
	public Text[] r_PlayerScoresText = new Text[PlayerManager.MAX_PLAYERS];

    [SerializeField]
    private Text c_timer;

    float m_Mins;
    float m_Secs;

	// Use this for initialization
	void Start ()
    {
        r_RoundTimer = FindObjectOfType<RoundTimer>();
        //r_text.GetComponentInChildren<Text>();
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
    }
}
