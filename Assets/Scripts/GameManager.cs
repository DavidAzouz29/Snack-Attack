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
/// - Cleaned up code - David Azouz 26/07/2016
/// TODO:
/// - 
/// </summary>
/// ----------------------------------
/// 
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public PlayerManager r_PlayerManager; //TODO: why not PlayerManager?
    //const uint MAX_PLAYERS = PlayerManager.MAX_PLAYERS; //TODO: needed?

    private int m_RandomPlayer;
    private float m_BossScale;

    void Start()
    {
        r_PlayerManager = FindObjectOfType<PlayerManager>();
    }

    public void SetupBoss ()
    {
        //Generate Random player
        m_RandomPlayer = Random.Range(0, (int)PlayerManager.MAX_PLAYERS);
        //Grab required data and assign the random player as the boss
        // TODO: Player Array is 0 (in PlayerManager)- this is being called in (RoundTimer) Update not Start like it once was,
        // as there are 0 players in the array GameManager script is playing up
		r_PlayerManager.GetPlayer(m_RandomPlayer).GetComponent<BossBlobs>().enabled = true;
		//m_BossScale = r_PlayerManager.GetPlayer(m_RandomPlayer).GetComponent<BossBlobs>().m_Blobs.BigScale;
		r_PlayerManager.GetPlayer(m_RandomPlayer).transform.localScale = new Vector3(m_BossScale, m_BossScale, m_BossScale);
    }
	
	// Update is called once per frame
	//void Update () {	}
}
