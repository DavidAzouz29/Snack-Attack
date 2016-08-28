/// <summary>
/// Author: 		David Azouz
/// Date Created: 	12/04/16
/// Date Modified: 	12/04/16
/// --------------------------------------------------
/// Brief: A Player Manager class that handles players.
/// viewed https://drive.google.com/drive/folders/0B67Mvyh-0w-RTlhLX1lsOHRfdDA
/// https://unity3d.com/learn/tutorials/projects/survival-shooter/more-enemies
/// 
/// ***EDIT***
/// - 	- David Azouz 11/04/16
/// -  - David Azouz 11/04/16
/// - Players have unique material - David Azouz 21/06/16
/// 
/// TODO:
/// - change remove const from MAX_PLAYERS
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour//TOOD:, IClass
{
    //----------------------------------
    // PUBLIC VARIABLES
    //----------------------------------
	[Header("Hold Players")]
    public const uint MAX_PLAYERS = 4;
    //Player referances
    public GameObject r_Player_1;
    public GameObject r_Player_2;
    public GameObject r_Player_3;
    public GameObject r_Player_4;

    [SerializeField]
    GameObject[] r_Players = new GameObject[MAX_PLAYERS]; // Used for camera FOV
    public CameraControl m_CameraControl;
    PlayerController[] uiPlayerConArray = new PlayerController[MAX_PLAYERS];
    public PlayerController r_PlayerController; // Referance to a player.
    public GameObject[] blobArray = new GameObject[MAX_PLAYERS];

    [Header("Attack Damage")]
    public int m_LightAttack = 0;
    public int m_HeavyAttack = 0;

    public Color c_ColPlayer2;

    //----------------------------------
    // PRIVATE VARIABLES
    //----------------------------------
    Vector3 v3PlayerPosition = Vector3.zero;
    private GameObject r_Player;

    private List<GameObject> m_PlayerSpawns;

    private SpawnManager m_SpawnManager;
	private GameManager m_GameManager;
    // Use this for initialization
    void Start ()
    {
        m_SpawnManager = FindObjectOfType<SpawnManager>();
		m_GameManager = FindObjectOfType<GameManager>();
        m_PlayerSpawns = m_SpawnManager.m_PlayerSpawns;
    }

	public GameObject GetPlayer(int i)
	{
		return r_Players [i];
	}

    public GameObject[] GetBlobArray()
    {
        return blobArray;
    }

    // TODO: Player Array is 0 - this is being called in (RoundTimer) Update not Start like it once was,
    // as there are 0 players in the array GameManager script is playing up
    public void CreatePlayers()
    {
        GameObject.Find("Scoreboard Canvas").GetComponent<ScoreManager>().PrototypeStartup();
        m_CameraControl.m_Targets = new Transform[MAX_PLAYERS]; //assigns the maximum characters the camera should track
        //Loop through and create our players.
        for (uint i = 0; i < MAX_PLAYERS; ++i)
        {
			PlayerController.E_CLASS_STATE playerState = PlayerController.E_CLASS_STATE.E_PLAYER_STATE_COUNT;
            // Position characters randomly on the floor
            // if it's the first player, set them to character 'x', second to 'y' etc.
            if (i == 0)
            {
                r_Player = r_Player_1;
				playerState = PlayerController.E_CLASS_STATE.E_CLASS_STATE_ROCKYROAD;
                r_Player.GetComponentInChildren<UIBossLevel>().c_WheelImage.color = Color.magenta;
            }
            else if (i == 1)
            {
                r_Player = r_Player_2;
				playerState = PlayerController.E_CLASS_STATE.E_CLASS_STATE_ROCKYROAD;
                r_Player.GetComponentInChildren<UIBossLevel>().c_WheelImage.color = Color.red;
            }
            else if (i == 2)
            {
                r_Player = r_Player_3;
				playerState = PlayerController.E_CLASS_STATE.E_CLASS_STATE_ROCKYROAD;
                r_Player.GetComponentInChildren<UIBossLevel>().c_WheelImage.color = Color.green; //c_ColPlayer2
            }
            else if (i == 3)
            {
                r_Player = r_Player_4; //TODO: r_Player = r_PlayerKaraTea;
                playerState = PlayerController.E_CLASS_STATE.E_CLASS_STATE_ROCKYROAD;
                r_Player.GetComponentInChildren<UIBossLevel>().c_WheelImage.color = Color.cyan;
            }

            Object j = Instantiate(r_Player, m_PlayerSpawns[(int)i].transform.position, r_Player.transform.rotation);
			j.name = "Character " + (i + 1);
			// -------------------------------------------------------------
			// This allows each instance the ability to move independently
			// -------------------------------------------------------------
			r_PlayerController = ((GameObject)j).GetComponent<PlayerController>();
			r_PlayerController.SetPlayerID(i);

			uiPlayerConArray[i] = r_PlayerController;
			uiPlayerConArray[i].m_eCurrentClassState = playerState;
			r_Players[i] = (GameObject)j;
        }
        for (int i = 0; i < MAX_PLAYERS; i++)
        {
            // assign the target transforms for the camera to track
            m_CameraControl.m_Targets[i] = r_Players[i].transform;
        }

		m_GameManager.SetupBoss();
    } 
}
