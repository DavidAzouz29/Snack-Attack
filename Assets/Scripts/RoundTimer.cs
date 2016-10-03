///<summary>
///
/// Timer selectable - David Azouz 1/10/16
/// </summary>

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class RoundTimer : MonoBehaviour {

    //public float m_TimePerRound = 60.0f;

    [SerializeField]
    private float m_TimeRemaining;

    public float m_RoundCountdownTime = 5.0f;

    private bool m_RoundStarted = false;
    private bool m_Spawned = false;

    private List<GameObject> m_PlayerSpawns;

    public List<GameObject> m_Players;

    private int m_PlayerCount;
    [SerializeField]
    private PlayerManager m_PlayerManager;

    public GameObject m_Window;

    void Start()
    {
        m_PlayerManager = FindObjectOfType<PlayerManager>();
        m_PlayerSpawns = FindObjectOfType<SpawnManager>().m_PlayerSpawns;

        m_TimeRemaining = 60.0f;
    }

    void OnLevelWasLoaded()
    {
        m_TimeRemaining = GameSettings.Instance.iRoundTimerChoice;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            m_RoundStarted = true;
        }

        if(m_RoundStarted)
        {
            m_TimeRemaining -= Time.deltaTime;
            if (m_TimeRemaining <= 0.0f)
            {
                m_RoundStarted = false;
                Time.timeScale = 0.0f;
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
}
