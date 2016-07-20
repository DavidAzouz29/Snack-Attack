using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    const int MAX_PLAYERS = 2;
    public GameObject m_PlayerManager;

    private PlayerController[] m_ListOfPlayers;
    private int m_RandomPlayer;
    private float m_BossScale;
    // Use this for initialization
    void Start () {
        //Generate Random player
        m_RandomPlayer = Random.Range(0, (MAX_PLAYERS - 1));
        //Grab required data and assign the random player as the boss
        m_BossScale = m_PlayerManager.GetComponent<PlayerManager>().uiPlayerConArray[m_RandomPlayer].gameObject.GetComponent<BossBlobs>().m_Blobs.GiantScale;
        //
        Debug.Log(m_BossScale);
        m_PlayerManager.GetComponent<PlayerManager>().r_Players[m_RandomPlayer].gameObject.GetComponent<BossBlobs>().enabled = true;
        m_PlayerManager.GetComponent<PlayerManager>().r_Players[m_RandomPlayer].gameObject.transform.localScale = new Vector3(m_BossScale, m_BossScale, m_BossScale);


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
