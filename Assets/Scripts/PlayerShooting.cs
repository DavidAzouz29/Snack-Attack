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

[System.Serializable]
public class Done_Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

//-------------------------------------
// *{} Class Declaration - PlayerShooting
//-------------------------------------
public class PlayerShooting : MonoBehaviour 
{
    //-------------------------------------
    // PUBLIC INSTANCE VARIABLES
    //-------------------------------------
    public float speed = 20.0f;
    public float fireTime = 0.05f;
	public float tilt;
	public Done_Boundary boundary;

    public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;

    public AudioClip shootSound;

    //-------------------------------------
    // PRIVATE INSTANCE VARIABLES
    //-------------------------------------
	private float nextFire;

    //-------------------------------------
	// Use this for initialization
    //-------------------------------------
	void Start () 
    {
        //if (Input.GetButton("Fire1") && Time.time > nextFire)
        //{
        //    nextFire = Time.time + fireRate;
        //    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        //    //InvokeRepeating("Fire", fireTime, fireTime); Used for Object Pooling
        //}
	}

    //-------------------------------------
	// Update is called once per frame
    //-------------------------------------
	void Update () 
    {
        // Hacky way of getting players firing
        if (Input.GetButton("Fire1") && Time.time > nextFire ||
            Input.GetButton("Fire2") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        }
    }

    void Fire()
    {
        // Object Pooling Stuff
        //GameObject obj = ObjectPoolScript.current.GetPooledObject();
        //
        //if (obj == null) return;
        //
        //SoundManager.instance.RandomizeSfx(shootSound);
        //obj.transform.position = transform.position;
        //obj.transform.rotation = transform.rotation;
        //obj.SetActive(true);
    }
}

//--------------------------------------------------------------------------------------
// A method of 
//--------------------------------------------------------------------------------------
//
// Param:
//			path: The path of the Folder of the images desired to present.
// searchPattern: is the string to match against the names of files in the target directory.
//
// Return:
//		Returns a string array to get the files in the target directory where path is the target directory.
//		
// Example:
//		Directory.GetFiles(string path, string searchPattern)
//--------------------------------------------------------------------------------------