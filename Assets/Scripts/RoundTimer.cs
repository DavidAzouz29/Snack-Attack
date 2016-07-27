using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoundTimer : MonoBehaviour {

    public float m_TimePerRound = 60.0f;

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

    void Start()
    {
        m_PlayerManager = FindObjectOfType<PlayerManager>();
        m_PlayerSpawns = FindObjectOfType<SpawnManager>().m_PlayerSpawns;

        m_TimeRemaining = m_TimePerRound;

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
            }
            else if(!m_Spawned)
            {
            // Allow the game to run here

                m_Spawned = true;
                m_PlayerManager.CreatePlayers();
            }
        }
    }

    public float GetTimeRemaining()
    {
        return m_TimeRemaining;
    }

    //void SetUpCameraAndSpawns()
    //{

    //    m_PlayerManager.m_CameraControl.m_Targets = new Transform[2]; //assigns the maximum characters the camera should track

    //    for (int i = 0; i < 2; i++)
    //    {
    //        // assign the target transforms for the camera to track
    //        m_PlayerManager.m_CameraControl.m_Targets[i] = m_Players[i].transform;
    //    }
    //}

}
