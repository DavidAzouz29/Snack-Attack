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

    //private float m_TimePerRound = 180.0f;

    [SerializeField]
    private float m_TimeRemaining;

    public float m_RoundCountdownTime = 5.0f;

    private bool m_RoundStarted = false;
    private bool m_Spawned = false;

    [SerializeField]
    //private List<GameObject> m_PlayerSpawns;

    private int m_PlayerCount;
    [SerializeField]
    private PlayerManager m_PlayerManager;

    public GameObject m_ScoreBoardWindow;
    //public Text c_CountdownText;

    void Awake()
    {
        m_ScoreBoardWindow = FindObjectOfType<WindowManager>().gameObject;
        m_TimeRemaining = 180.0f;// m_TimePerRound;
        m_Spawned = false;
        m_RoundStarted = true;
    }

    void Start()
    {
        m_PlayerManager = PlayerManager.Instance;
        SetTimerSelection(GameManager.Instance.m_ActiveGameSettings.iRoundTimerChoice);
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
            m_TimeRemaining = Mathf.Max( m_TimeRemaining - Time.deltaTime, 0.0f );

            // Time's up and Scoreboard
            if (m_TimeRemaining <= 0.0f)
            {
                m_RoundStarted = false;
                for (int i = 0; i < PlayerManager.MAX_PLAYERS; i++)
                {
                    m_PlayerManager.GetPlayer(i).GetComponent<PlayerController>().enabled = false;
                }
                m_ScoreBoardWindow.GetComponent<WindowManager>().TimesUp();
                Time.timeScale = 0.5f;
            }
            else if(!m_Spawned)
            {
            // Allow the game to run here

                m_Spawned = true;
                m_PlayerManager.CreatePlayers();
                // Disable movement so Princess Anims can sync up.
                /*for (int i = 0; i < PlayerManager.MAX_PLAYERS; i++)
                {
                    //m_PlayerManager.GetPlayer(i).GetComponent<PlayerController>().enabled = false;
                }*/
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
                    m_TimeRemaining = 60.0f;
                    break;
                }
            // Three Minutes
            case 1:
                {
                    m_TimeRemaining = 180.0f;
                    break;
                }
            // Five Minutes
            case 2:
                {
                    m_TimeRemaining = 300.0f;
                    break;
                }
            default:
                {
                    m_TimeRemaining = 180.0f;
                    break;
                }
        }
    }
}
