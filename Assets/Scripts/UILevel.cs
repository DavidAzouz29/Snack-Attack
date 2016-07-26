using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UILevel : MonoBehaviour {

    public RoundTimer r_RoundTimer;

    [SerializeField]
    private Text r_text;

	// Use this for initialization
	void Start ()
    {
        //r_text.GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        r_text.text = r_RoundTimer.GetTimeRemaining().ToString("f2");
    }
}
