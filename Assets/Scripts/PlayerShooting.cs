///////////////////////////////////////////////////////////////////////////////////////////
//	File Name:		PlayerShooting
//	Author:		David Azouz
//	Date Created:	08/10/2015
//	Brief:			A Class that controls bullets used within Unity
///////////////////////////////////////////////////////////////////////////////////////////
// viewed http://unity3d.com/learn/tutorials/projects/space-shooter-tutorial
// http://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/object-pooling
///////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShooting : MonoBehaviour 
{
    //-------------------------------------
    // PUBLIC INSTANCE VARIABLES
    //-------------------------------------
    public float speed = 20.0f;
    public float fireTime = 0.05f;
	public float tilt;

	public Transform shotSpawn;
	public float fireRate; // how long we wait before firing another bullet

    public AudioClip shootSound;
    public string sFire;

    //-------------------------------------
    // PRIVATE INSTANCE VARIABLES
    //-------------------------------------
    // Shots
    private float nextFire;
    private Vector3 SpawnPosition;
    private Quaternion SpawnRotation;
    [SerializeField]
    private GameObject _shot;
    // A way to store the different shots based on class
    [SerializeField]
    private GameObject[] shotArray = new GameObject[PlayerManager.MAX_PLAYERS];
    private PlayerManager r_PlayerMan;
    private PlayerController r_PlayerCon;
    //-------------------------------------
    // Use this for initialization
    //-------------------------------------
    void Start()
    {
        SpawnPosition = shotSpawn.position;
        SpawnRotation = Quaternion.Euler(0, 180, 0) * transform.rotation;
        r_PlayerMan = FindObjectOfType<PlayerManager>();
        r_PlayerCon = GetComponent<PlayerController>();
        shotArray = r_PlayerMan.GetShotArray();
    }

    //-------------------------------------
    // Update is called once per frame
    //-------------------------------------

    void Update () 
    {
        SpawnPosition = shotSpawn.position;
        SpawnRotation = Quaternion.Euler(0, 180, 0) * transform.rotation;
        // Hacky way of getting players firing
        if (Input.GetButton(sFire) && Time.time > nextFire)
        {
            GameObject shot = null;
            // Spawn *type* of projectile based of player class
            switch (r_PlayerCon.m_eCurrentClassState)
            {
                case PlayerController.E_CLASS_STATE.E_CLASS_STATE_ROCKYROAD:
                    {
                        shot = shotArray[0];
                        break;
                    }
                case PlayerController.E_CLASS_STATE.E_CLASS_STATE_BROCCOLION:
                    {
                        shot = shotArray[1];
                        break;
                    }
                case PlayerController.E_CLASS_STATE.E_CLASS_STATE_WATERMELOMON:
                    {
                        shot = shotArray[2];
                        break;
                    }
                case PlayerController.E_CLASS_STATE.E_CLASS_STATE_KARATEA:
                    {
                        shot = shotArray[3];
                        break;
                    }
                default:
                    {
                        Debug.LogError("Character animation not set up");
                        break;
                    }
            }
            nextFire = Time.time + fireRate;
            GameObject _shot = (GameObject)Instantiate(shot, SpawnPosition, SpawnRotation);
            _shot.GetComponent<BulletScript>().m_Parent = gameObject;
        }
    }

    // pass through the number/ id
    public void SetFire(string a_fire) { sFire = a_fire; }
}