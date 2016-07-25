/// ----------------------------------
/// <summary>
/// Name: PlayerController.cs
/// Author: Dylan Harvey and David Azouz
/// Date Created: 20/07/2016
/// Date Modified: 7/2016
/// ----------------------------------
/// Brief: Player Controller class that controls the player.
/// viewed: https://unity3d.com/learn/tutorials/projects/roll-a-ball/moving-the-player
/// http://wiki.unity3d.com/index.php?title=Xbox360Controller
/// http://answers.unity3d.com/questions/788043/is-it-possible-to-translate-an-object-diagonally-a.html
/// *Edit*
/// - Selects a random player to be the boss - Dylan Harvey 20/07/2016
/// - team ups - David Azouz 20/07/2016
/// TODO:
/// - 
/// </summary>
/// ----------------------------------
/// 
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    const int MAX_PLAYERS = 2;
    public GameObject m_PlayerManager; //TODO: why not PlayerManager?

    private int m_RandomPlayer;
    private float m_BossScale;
    // Use this for initialization
    void Start () {
        //Generate Random player
        m_RandomPlayer = Random.Range(0, (MAX_PLAYERS - 1));
        //Grab required data and assign the random player as the boss
        m_BossScale = m_PlayerManager.GetComponent<PlayerManager>().r_Players[m_RandomPlayer].gameObject.GetComponent<BossBlobs>().m_Blobs.GiantScale;
        m_PlayerManager.GetComponent<PlayerManager>().r_Players[m_RandomPlayer].gameObject.GetComponent<BossBlobs>().enabled = true;
        m_PlayerManager.GetComponent<PlayerManager>().r_Players[m_RandomPlayer].gameObject.transform.localScale = new Vector3(m_BossScale, m_BossScale, m_BossScale);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
