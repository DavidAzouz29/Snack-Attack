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

    public GameObject shot;
	public Transform shotSpawn;
	public float fireRate; // how long we wait before firing another bullet

    public AudioClip shootSound;
    public string sFire;

    //-------------------------------------
    // PRIVATE INSTANCE VARIABLES
    //-------------------------------------
    private float nextFire;
    private Vector3 SpawnPosition;
    private Quaternion SpawnRotation;
    //-------------------------------------
    // Use this for initialization
    //-------------------------------------
    void Start()
    {
        SpawnPosition = shotSpawn.position;
        SpawnRotation = Quaternion.Euler(0, 180, 0) * transform.rotation;
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
            nextFire = Time.time + fireRate;
            Instantiate(shot, SpawnPosition, SpawnRotation);
        }
    }

    // pass through the number/ id
    public void SetFire(string a_fire) { sFire = a_fire; }
}