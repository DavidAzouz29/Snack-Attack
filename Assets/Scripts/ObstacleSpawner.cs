using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    
    public Transform[] spawnPoints;
    public GameObject pipePrefab;
    public int numPipes;

	// Use this for initialization
	void Start ()
    {
        SpawnPipes();
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    //function that spawns the pipes
    void SpawnPipes()
    {
        for (int i = 0; i < numPipes; ++i)
        {
            int randSpawnPoint = (int)(Random.value * spawnPoints.Length);

            GameObject go = Instantiate(pipePrefab, spawnPoints[randSpawnPoint].position, spawnPoints[randSpawnPoint].rotation) as GameObject;
            go.transform.Rotate(Vector3.up, Random.Range(0, 360));
        }
    }
}
