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

public class PlayerManager : MonoBehaviour
{
    //----------------------------------
    // PUBLIC VARIABLES
    //----------------------------------
	[Header("Hold Players")]
    public const uint MAX_PLAYERS = 2; // TODO: change in Player Controller
    public GameObject r_PlayerRockyroad;    // Referance to a player.
    public GameObject r_PlayerBroccolion;   // Referance to a player.
    public GameObject r_PlayerWatermelomon; // Referance to a player.
    public GameObject r_PlayerKaraTea;      // Referance to a player.
    public GameObject[] r_Players = new GameObject[MAX_PLAYERS]; // Used for camera FOV
    public Vector3 v3PlayerPosition = Vector3.zero;
	[Header("Materials for different players")]
    public CameraControl m_CameraControl;
    public Material r_Player1;
    public Material r_Player2;

    public PlayerController[] uiPlayerConArray = new PlayerController[MAX_PLAYERS]; //TODO: private
    public PlayerController r_PlayerController; // Referance to a player.

    public PlayerShooting[] uiPlayerShootArray = new PlayerShooting[MAX_PLAYERS]; //TODO: private
    public PlayerShooting r_PlayerShooting; // Referance to a player.

    //----------------------------------
    // PRIVATE VARIABLES
    //----------------------------------
    private float fTerrRadius = 5;
    private GameObject r_Player;

    private List<GameObject> m_PlayerSpawns;

    private SpawnManager m_SpawnManager;
    // Use this for initialization
    void Start ()
    {
        m_SpawnManager = FindObjectOfType<SpawnManager>();
        m_PlayerSpawns = m_SpawnManager.m_PlayerSpawns;
    }

    public void CreatePlayers()
    {
        m_CameraControl.m_Targets = new Transform[MAX_PLAYERS]; //assigns the maximum characters the camera should track
        //Loop through and create our players.
        for (uint i = 0; i < MAX_PLAYERS; ++i)
        {
            // Position characters randomly on the floor
            v3PlayerPosition = m_PlayerSpawns[Random.Range(0, (int)MAX_PLAYERS)].transform.position; //
            // if it's the first player, set them to character 'x', second to 'y' etc.
            if (i == 0)
            {
                r_Player = r_PlayerRockyroad;
            }
            else if (i == 1)
            {
                r_Player = r_PlayerBroccolion;
            }
            else if (i == 2)
            {
                r_Player = r_PlayerWatermelomon;
            }
            else if (i == 3)
            {
                r_Player = r_PlayerKaraTea;
            }
            Object j = Instantiate(r_Player, v3PlayerPosition, r_Player.transform.rotation);
            j.name = "Character " + (i + 1);

            // -------------------------------------------------------------
            // This allows each instance the ability to move independently
            // -------------------------------------------------------------
            r_PlayerController = ((GameObject)j).GetComponent<PlayerController>();
            r_PlayerController.SetPlayerID(i);
            r_PlayerShooting = ((GameObject)j).GetComponent<PlayerShooting>();
            r_PlayerShooting.SetFire("P" + (i + 1) + "_Fire");

            uiPlayerConArray[i] = r_PlayerController;
            uiPlayerShootArray[i] = r_PlayerShooting;
            r_Players[i] = (GameObject)j;
        }
        for (int i = 0; i < MAX_PLAYERS; i++)
        {
            // assign the target transforms for the camera to track
            m_CameraControl.m_Targets[i] = r_Players[i].transform;
        }
    } 
}
