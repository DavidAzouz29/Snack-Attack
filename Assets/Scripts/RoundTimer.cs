///<summary>
///
/// Timer selectable - David Azouz 1/10/16
/// </summary>

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RoundTimer : MonoBehaviour {

    private float m_TimePerRound = 180.0f;

    [SerializeField]
    private float m_TimeRemaining;

    public float m_RoundCountdownTime = 5.0f;

    private bool m_RoundStarted = false;
    private bool m_Spawned = false;

    private List<GameObject> m_PlayerSpawns;

    private int m_PlayerCount;
    [SerializeField]
    private PlayerManager m_PlayerManager;

    public GameObject m_ScoreBoardWindow;
    public Text c_CountdownText;

    void Start()
    {
        m_PlayerManager = GetComponent<PlayerManager>();

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            OnLevelWasLoaded();
        }
    }

    void OnLevelWasLoaded()
    {
        // Return to Menu more than Splash
        if (SceneManager.GetActiveScene().buildIndex != Scene.Menu)
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                //m_TimeRemaining = GameSettings.Instance.iRoundTimerChoice;
                m_PlayerSpawns = FindObjectOfType<SpawnManager>().m_PlayerSpawns;
                m_ScoreBoardWindow = FindObjectOfType<WindowManager>().gameObject;
                m_TimeRemaining = m_TimePerRound;
                m_Spawned = false;
                m_RoundStarted = true; //TODO: set m_RoundStarted to false for "3,2,1"
                //c_CountdownText = FindObjectOfType<UILevel>().gameObject.GetComponent<Text>();
                //StartCoroutine(RoundCountdown());
            }
        }
    }

    IEnumerator RoundCountdown()
    {
        float delay = 2;
        c_CountdownText.text = "3";
        yield return new WaitForSeconds(delay);
        c_CountdownText.text = "2";
        yield return new WaitForSeconds(delay);
        c_CountdownText.text = "1";
        yield return new WaitForSeconds(delay);
        c_CountdownText.text = "FIGHT!";
        yield return new WaitForSeconds(delay);
        m_RoundStarted = true;
        //yield return null;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            if (SceneManager.GetActiveScene().buildIndex != Scene.Menu)
            {
                if (SceneManager.GetActiveScene().buildIndex != 0)
                {
                    m_RoundStarted = true;
                }
            }
        }

        if(m_RoundStarted)
        {
            m_TimeRemaining -= Time.deltaTime;
            // Time's up and Scoreboard
            if (m_TimeRemaining <= 0.0f)
            {
                m_RoundStarted = false;
                for (int i = 0; i < PlayerManager.MAX_PLAYERS; i++)
                {
                    m_PlayerManager.GetPlayer(0).GetComponent<PlayerController>().enabled = false;
                }
                m_ScoreBoardWindow.GetComponent<WindowManager>().TimesUp();
                Time.timeScale = 0.5f;
            }
            else if(!m_Spawned)
            {
            // Allow the game to run here

                m_Spawned = true;
                m_PlayerManager.CreatePlayers();
            }
        }

        if (GetTimeRemaining() <= 0)
        {
            if (Input.GetButton("Pause"))
            {
                //m_Window.SetActive(false);
                m_Spawned = false;
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                //Time.timeScale = 1.0f;
            }
        }
    }

    public float GetTimeRemaining()
    {
        return m_TimeRemaining;
    }

    // Selects time duration for a round
    public void SetTimerSelection(int a_time)
    {
        switch (a_time)
        {
            // One minute
            case 0:
                {
                    //GameSettings.Instance.SetRoundTimer(60);
                    m_TimePerRound = 60.0f;
                    break;
                }
            // Three Minutes
            case 1:
                {
                    //GameSettings.Instance.SetRoundTimer(180); Debug.Log("MS.cs Time Sel: " + GameSettings.Instance.iRoundTimerChoice);
                    m_TimePerRound = 180.0f;
                    break;
                }
            // Five Minutes
            case 2:
                {
                    //GameSettings.Instance.SetRoundTimer(300);
                    m_TimePerRound = 300.0f;
                    break;
                }
            default:
                {
                    //GameSettings.Instance.SetRoundTimer(60);
                    m_TimePerRound = 60.0f;
                    break;
                }
        }
    }
}
