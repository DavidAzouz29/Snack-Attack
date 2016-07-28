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

//#define MAX_PLAYERS 4

public class PlayerManager : MonoBehaviour//TOOD:, IClass
{
    //----------------------------------
    // PUBLIC VARIABLES
    //----------------------------------
	[Header("Hold Players")]
    public const uint MAX_PLAYERS = 3; // TODO: change to 4
    public GameObject r_PlayerRockyroad;    // Referance to a player.
    public GameObject r_PlayerBroccolion;   // Referance to a player.
    public GameObject r_PlayerWatermelomon; // Referance to a player.
    public GameObject r_PlayerKaraTea;      // Referance to a player.
    [SerializeField]
    GameObject[] r_Players = new GameObject[MAX_PLAYERS]; // Used for camera FOV
	[Header("Materials for different players")]
    public CameraControl m_CameraControl;
    public Material r_Broc1;
    public Material r_Broc2;

    PlayerController[] uiPlayerConArray = new PlayerController[MAX_PLAYERS];
    public PlayerController r_PlayerController; // Referance to a player.

    PlayerShooting[] uiPlayerShootArray = new PlayerShooting[MAX_PLAYERS];
    public PlayerShooting r_PlayerShooting; // Referance to a player.

    public GameObject[] shotArray = new GameObject[MAX_PLAYERS];

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

    public GameObject[] GetShotArray()
    {
        return shotArray;
    }

    // TODO: Player Array is 0 - this is being called in (RoundTimer) Update not Start like it once was,
    // as there are 0 players in the array GameManager script is playing up
    public void CreatePlayers()
    {
        m_CameraControl.m_Targets = new Transform[MAX_PLAYERS]; //assigns the maximum characters the camera should track
        //Loop through and create our players.
        for (uint i = 0; i < MAX_PLAYERS; ++i)
        {
			PlayerController.E_CLASS_STATE playerState = PlayerController.E_CLASS_STATE.E_PLAYER_STATE_COUNT;
            Material curMat = null;
            // Position characters randomly on the floor
            v3PlayerPosition = m_PlayerSpawns[Random.Range(0, (int)MAX_PLAYERS)].transform.position;
            // if it's the first player, set them to character 'x', second to 'y' etc.
            if (i == 0)
            {
                r_Player = r_PlayerRockyroad;
				playerState = PlayerController.E_CLASS_STATE.E_CLASS_STATE_ROCKYROAD;
                //shotArray[i] = 
            }
            else if (i == 1)
            {
                r_Player = r_PlayerBroccolion;
				playerState = PlayerController.E_CLASS_STATE.E_CLASS_STATE_BROCCOLION;
                curMat = r_Broc1;
            }
            else if (i == 2)
            {
                r_Player = r_PlayerWatermelomon;
				playerState = PlayerController.E_CLASS_STATE.E_CLASS_STATE_WATERMELOMON;
            }
            else if (i == 3)
            {
                r_Player = r_PlayerBroccolion; //TODO: r_Player = r_PlayerKaraTea;
                playerState = PlayerController.E_CLASS_STATE.E_CLASS_STATE_KARATEA;
                curMat = r_Broc2;
                //shotArray[3].GetComponentsInChildren<SkinnedMeshRenderer>();
            }
            
			Object j = Instantiate(r_Player, v3PlayerPosition, r_Player.transform.rotation);
			j.name = "Character " + (i + 1);
			// Chooses which mesh to display
			SkinnedMeshRenderer mesh = ((GameObject)j).GetComponentInChildren<SkinnedMeshRenderer>();
            // if the first player
            if (i == 1 || i == 3)
            {
                mesh.material = curMat;
                //mesh.material.mainTexture = r_Player1T;
            } 

			// -------------------------------------------------------------
			// This allows each instance the ability to move independently
			// -------------------------------------------------------------
			r_PlayerController = ((GameObject)j).GetComponent<PlayerController>();
			r_PlayerController.SetPlayerID(i);
			r_PlayerShooting = ((GameObject)j).GetComponent<PlayerShooting>();
			r_PlayerShooting.SetFire("P" + (i + 1) + "_Fire");

			uiPlayerConArray[i] = r_PlayerController;
			uiPlayerConArray[i].m_eCurrentClassState = playerState;
			uiPlayerShootArray[i] = r_PlayerShooting;
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
