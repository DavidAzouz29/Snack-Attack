using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class WindowManager : MonoBehaviour {

	public GameObject scoreBoard;
	public GameObject c_TimesUpRay;
	public GameObject c_TimesUpText;
    public Button c_ReplayButton;
    public float fScoreboardDelay = 1.0f;

	// Use this for initialization
	void Start () {
        // turns the scoreboard off during playtime.
        scoreBoard.SetActive(true);
        c_TimesUpRay.SetActive(true);
        c_TimesUpText.SetActive(true);
        scoreBoard.SetActive(false);
        c_TimesUpRay.SetActive(false);
        c_TimesUpText.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("Pause"))
        {
			scoreBoard.SetActive( !scoreBoard.activeSelf );
            if (scoreBoard.activeSelf)
            {
                Time.timeScale = 0.01f;
                c_ReplayButton.Select();
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
	}
    
    public void TimesUp()
    {
        Time.timeScale = 1f;
        c_TimesUpRay.SetActive(true);
        c_TimesUpText.SetActive(true);
        GameManager.Instance.transform.GetChild(GameManager.iChInGame).GetChild(10).GetComponent<AudioSource>().Play();
        Invoke("ScoreboardEndGame", fScoreboardDelay);
    }

    void ScoreboardEndGame()
    {
        c_TimesUpRay.SetActive(false);
        c_TimesUpText.SetActive(false);
        scoreBoard.SetActive(true);
        c_ReplayButton.Select();
    }

    public void Replay()
    {
        scoreBoard.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(Scene.Menu);
        Time.timeScale = 1.0f;
    }
}
