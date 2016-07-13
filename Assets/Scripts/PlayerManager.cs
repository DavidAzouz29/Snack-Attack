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

//#define MAX_PLAYERS 4

public class PlayerManager : MonoBehaviour
{
    //----------------------------------
    // PUBLIC VARIABLES
    //----------------------------------
	[Header("Hold Players")]
    public const uint MAX_PLAYERS = 2; // TODO: change in Player Controller
    public GameObject r_Player; // Referance to a player.
    public GameObject[] r_Players = new GameObject[MAX_PLAYERS]; // Used for camera FOV
    public Vector3 v3PlayerPosition = Vector3.zero;
	[Header("Values for camera pos and Lerping")]
	public Vector3 distance = Vector3.zero;
	public float heightOfCamera = 2.3f;
    public Camera c_Camera;
    public Material r_Player1;
    public Material r_Player2;
    //public Texture r_Player1T; //TODO: make textures/ materials work
    //public Texture r_Player2T;

    //public Color[] colorsArray = new Color[MAX_PLAYERS];
    public PlayerController[] uiPlayerArray = new PlayerController[MAX_PLAYERS]; //TODO: private
    public PlayerController r_PlayerController; // Referance to a player.

    //----------------------------------
    // PRIVATE VARIABLES
    //----------------------------------
    private float fTerrRadius = 3.5f;
    private const int m_iCameraFOV = 60;
    private const int m_iCameraFOVBT = 40;
    private float m_fCameraFOVCurrent = 0.0f;

    // Selects the main camera once level is loaded
    void OnLevelWasLoaded()
    {
        c_Camera = Camera.main;
        c_Camera.fieldOfView = m_iCameraFOV;
    }

    // Use this for initialization
    void Start ()
    {
        v3PlayerPosition.y = 2.0f;

        //Loop through and create our players.
        for (uint i = 0; i < MAX_PLAYERS; ++i)
        {
            // Position characters randomly on the floor
            v3PlayerPosition.x = Random.Range(-fTerrRadius, fTerrRadius); //
            v3PlayerPosition.z = Random.Range(-fTerrRadius, fTerrRadius); //
            Object j = Instantiate(r_Player, v3PlayerPosition, r_Player.transform.rotation);
			j.name = "Character " + ( i + 1);
            SkinnedMeshRenderer mesh = ((GameObject)j).GetComponentInChildren<SkinnedMeshRenderer>();
            // if the first player
            if(i == 0)
            {
                mesh.material = r_Player1;
                //mesh.material.mainTexture = r_Player1T;
            }
            else if (i == 1)
            {
                mesh.material = r_Player2;
                //mesh.material.mainTexture = r_Player2T;
            }
            // -------------------------------------------------------------
            // This allows each instance the ability to move independently
            // -------------------------------------------------------------
            r_PlayerController = ((GameObject)j).GetComponent<PlayerController>();
            r_PlayerController.SetPlayerID(i);

            uiPlayerArray[i] = r_PlayerController;
            r_Players[i] = (GameObject)j;
            // -------------------------------------------------------------
            Debug.Log(v3PlayerPosition);
        }
    }

	//TODO: get camera lerping working
    // Used for zooming the camera in and out based on the distance of the playerss
  /*void Update()
    {
        // Distance
        Vector3 smallest = r_Players[0].transform.position;
		Vector3 largest = r_Players[MAX_PLAYERS - 1].transform.position; //changed from 0
        foreach (GameObject g in r_Players)
        {
            if (g == null) break;

            if (g.transform.position.x < smallest.x)
            {
                smallest.x = g.transform.position.x;
            }
            else if (g.transform.position.x > largest.x)
            {
                largest.x = g.transform.position.x;
            }

            if (g.transform.position.z < smallest.y)
            {
                smallest.y = g.transform.position.z;
            }
            else if (g.transform.position.z > largest.y)
            {
                largest.y = g.transform.position.z;
            }
        } 

        distance = largest - smallest;
		if (distance.x <= 5) distance.x = 5; Debug.Log ("x");
		if (distance.y <= 1) distance.y = 1; Debug.Log ("y");
		if (distance.z <= 5) distance.z = 5; Debug.Log ("z");

		if (distance.x >= 5) distance.x = 5; Debug.Log ("x");
		if (distance.y >= 1) distance.y = 1; Debug.Log ("y");
		if (distance.z >= 5) distance.z = 5; Debug.Log ("z");

        // Average
        Vector3 average = Vector3.zero;
        foreach (GameObject g in r_Players)
        {
            if (g == null) break;

            average += g.transform.position;
        }
        average /= MAX_PLAYERS;
        heightOfCamera = distance.magnitude;
		if (heightOfCamera > 4) {
			heightOfCamera = ;
			//c_Camera.fieldOfView = Mathf.Lerp (m_iCameraFOV, m_iCameraFOVBT, Time.deltaTime);//m_fCameraFOVCurrent); //0.2f quicker || //Time.timeScale//

		} else if (heightOfCamera < 3) {
			heightOfCamera = 0;
		} else if (heightOfCamera < -1) {
			heightOfCamera = -1;
		}

        //////Camera.main.transform.position = new Vector3(average.x, heightOfCamera, average.z - (3 * amountOfPlayers));
        //Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(average.x, heightOfCamera, average.z - (3 * MAX_PLAYERS)), 0.1f);
		float fExtra = 0;//5.0f;
		c_Camera.transform.position = Vector3.Lerp(c_Camera.transform.position, new Vector3(average.x - fExtra, heightOfCamera, average.z - (3 * MAX_PLAYERS) - fExtra), 0.1f);
		Vector3 v3LookAt = new Vector3 (distance.x / 2, distance.y / 2, distance.z / 2);
		c_Camera.transform.LookAt(v3LookAt); //Vector3.Lerp(Camera.main.transform.position, new Vector3(average.x, heightOfCamera, average.z - (3 * MAX_PLAYERS)), 0.1f);
		Debug.DrawLine(c_Camera.transform.position, v3LookAt);
		//c_Camera.fieldOfView = Mathf.Lerp (m_iCameraFOV, m_iCameraFOVBT, Time.deltaTime);//m_fCameraFOVCurrent); //0.2f quicker || //Time.timeScale//
	} */
}
