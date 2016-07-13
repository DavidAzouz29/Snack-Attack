using UnityEngine;
using System.Collections;

public class CameraAdjustment : MonoBehaviour
{
    [Header("Players")]
    public GameObject[] m_Players = new GameObject[4];

    int curPlayer = 0;
    int amountOfPlayers;
	void Start () {
	   
	}

    public void addPlayer(GameObject player)
    {
        if (curPlayer >= 4) return;

        m_Players[curPlayer] = player;
        curPlayer++;
        amountOfPlayers = curPlayer;
    }

    void Update()
    {
        // Distance
        Vector2 smallest = m_Players[0].transform.position;
        Vector2 largest = m_Players[0].transform.position;
        foreach (GameObject g in m_Players)
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

        Vector2 distance = largest - smallest;
        if (distance.x <= 5) distance.x = 5;
        if (distance.y <= 5) distance.y = 5;

        // Average
        Vector3 average = Vector3.zero;
        foreach (GameObject g in m_Players)
        {
            if (g == null) break;

            average += g.transform.position;
        }
        average /= amountOfPlayers;
        float heightOfCamera = distance.magnitude;
        if (heightOfCamera > 20) heightOfCamera = 20;

        //Camera.main.transform.position = new Vector3(average.x, heightOfCamera, average.z - (3 * amountOfPlayers));
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(average.x, heightOfCamera, average.z - (3 * amountOfPlayers)), 0.1f);

    }
}
