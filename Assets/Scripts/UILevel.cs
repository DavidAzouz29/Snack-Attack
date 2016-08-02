using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UILevel : MonoBehaviour {

    public RoundTimer r_RoundTimer;

    [SerializeField]
    private Text r_text;

    float m_Mins;
    float m_Secs;

	// Use this for initialization
	void Start ()
    {
        //r_text.GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        GetTime();
        r_text.text = (m_Mins.ToString() + ":" + m_Secs.ToString() );
    }

    void GetTime()
    {
        m_Mins = Mathf.Floor(r_RoundTimer.GetTimeRemaining() / 60);
        m_Secs = Mathf.Round(r_RoundTimer.GetTimeRemaining() % 60);
    }
}
