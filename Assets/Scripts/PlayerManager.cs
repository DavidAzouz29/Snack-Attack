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
using UnityEngine.SceneManagement;
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
    public GameObject r_Player_Rocky_1;
    public GameObject r_Player_Princess_2;
    public GameObject r_Player_Pizza_3;

    [SerializeField]
    GameObject[] r_Players = new GameObject[MAX_PLAYERS]; // Used for camera FOV
    PlayerController[] uiPlayerConArray = new PlayerController[MAX_PLAYERS];
    public PlayerController r_PlayerController; // Referance to a player.
    public GameObject[] blobArray = new GameObject[(int)PlayerBuild.E_BASE_CLASS_STATE.E_BASE_CLASS_STATE_BASE_COUNT];

    [Header("Attack Damage")]
    public int m_LightAttack = 0;
    public int m_HeavyAttack = 0;

    public Color c_ColPlayer2;

    //----------------------------------
    // PRIVATE VARIABLES
    //----------------------------------
    Vector3 v3PlayerPosition = Vector3.zero;
    private GameObject r_Player; // temp

    private List<GameObject> m_PlayerSpawns;

    private GameManager m_GameManager;
    private SpawnManager m_SpawnManager;
    private CameraControl m_CameraControl;
    [SerializeField] private SnackBrain[] m_SnackBrains = new SnackBrain[MAX_PLAYERS];
    public SkinnedMeshRenderer[] m_ModelMeshRenderers;

    // Use this for initialization
    void Start ()
    {
		m_GameManager = FindObjectOfType<GameManager>();
        OnLevelWasLoaded();
    }

    void OnLevelWasLoaded()
    {
        // Return to Menu more than Splash
        if (SceneManager.GetActiveScene().buildIndex != Scene.Menu)
        {
        // Not the splash screen
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                m_SpawnManager = FindObjectOfType<SpawnManager>();
                m_PlayerSpawns = m_SpawnManager.m_PlayerSpawns;
                m_CameraControl = FindObjectOfType<CameraControl>();
            }
        }
    }

    public SnackBrain[] GetSnackBrains()
	{
		return m_SnackBrains;
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
        //GameObject.Find("Scoreboard Canvas").GetComponent<ScoreManager>().PrototypeStartup();
        m_CameraControl.m_Targets = new Transform[MAX_PLAYERS]; //assigns the maximum characters the camera should track
        //Loop through and create our players.
        for (uint i = 0; i < MAX_PLAYERS; ++i)
        {
            m_SnackBrains[i] = GameSettings.Instance.players[(int)i].Brain;// m_GameManager.m_ActiveGameSettings.players[(int)i].Brain;
            //PlayerController.E_CLASS_STATE playerState = PlayerController.E_CLASS_STATE.E_PLAYER_STATE_COUNT;
            SkinnedMeshRenderer[] characterSkinnedRenderers = null;
            PlayerBuild.E_BASE_CLASS_STATE eBaseState = m_SnackBrains[(int)i].GetBaseState();
            switch (eBaseState)
            {
                case PlayerBuild.E_BASE_CLASS_STATE.E_BASE_CLASS_STATE_ROCKYROAD:
                    {
                        r_Player = r_Player_Rocky_1;
                        break;
                    }
                case PlayerBuild.E_BASE_CLASS_STATE.E_BASE_CLASS_STATE_PRINCESSCAKE:
                    {
                        r_Player = r_Player_Princess_2;
                        break;
                    }
                case PlayerBuild.E_BASE_CLASS_STATE.E_BASE_CLASS_STATE_PIZZAPUNK:
                    {
                        r_Player = r_Player_Pizza_3;
                        break;
                    }
                default:
                    {
                        Debug.LogError("PM: No Base Class.");
                        break;
                    }
            }

            characterSkinnedRenderers = r_Player.GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int k = 0; k < (int)PlayerBuild.E_BOSS_STATE.E_BOSS_STATE_MAIN_COUNT; k++)
            {
                // hold onto the position
                Material matSlot = characterSkinnedRenderers[k].sharedMaterial;
                // set the slot a material
                matSlot = m_SnackBrains[(int)i].GetStateMaterial(k);
                /*matSlot.SetTexture("_EmissionMap",
                    GameSettings.Instance.players[(int)i].Brain.GetEmissionMaps()[k]); */
                matSlot.SetColor("_EmissionColor", Color.black);
                characterSkinnedRenderers[k].sharedMaterial = matSlot;
                characterSkinnedRenderers[k].sharedMesh = m_SnackBrains[(int)i].GetStateMesh(k);
            }

            // Position characters randomly on the floor
            // if it's the first player, set them to character 'x', second to 'y' etc.
            if (i == 0)
            {
                r_Player.GetComponentInChildren<UIBossLevel>().c_WheelImage.color = Color.red;
                r_Player.GetComponent<BossBlobs>().PlayerCounter = 0;
            }
            else if (i == 1)
            {
                r_Player.GetComponentInChildren<UIBossLevel>().c_WheelImage.color = Color.blue;
                r_Player.GetComponent<BossBlobs>().PlayerCounter = 1;
            }
            else if (i == 2)
            {
                r_Player.GetComponentInChildren<UIBossLevel>().c_WheelImage.color = Color.green;
                r_Player.GetComponent<BossBlobs>().PlayerCounter = 2;

            }
            else if (i == 3)
            {
                Color Player4Color = new Color(0.4f, 0.18f, 0.58f);
                r_Player.GetComponentInChildren<UIBossLevel>().c_WheelImage.color = Player4Color; // m_playerInfo.Brain.Color;
                r_Player.GetComponent<BossBlobs>().PlayerCounter = 3;
            }

            Object j = Instantiate(r_Player, m_PlayerSpawns[(int)i].transform.position, r_Player.transform.rotation);
            j.name = "Character " + (i + 1);
            // -------------------------------------------------------------
            // This allows each instance the ability to move independently
            // -------------------------------------------------------------
            r_PlayerController = ((GameObject)j).GetComponent<PlayerController>();
            r_PlayerController.SetPlayerID(i);
            r_PlayerController.SetPlayerTag(m_SnackBrains[(int)i].GetClassName());

            uiPlayerConArray[i] = r_PlayerController;
            uiPlayerConArray[i].m_eCurrentClassState = m_SnackBrains[(int)i].GetClassState();
            r_Players[i] = (GameObject)j;
        }
        for (int i = 0; i < MAX_PLAYERS; i++)
        {
            // assign the target transforms for the camera to track
            m_CameraControl.m_Targets[i] = r_Players[i].transform;
        }

        m_GameManager.Init();
    } 
}
