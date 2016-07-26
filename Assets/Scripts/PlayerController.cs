/// ----------------------------------
/// <summary>
/// Name: PlayerController.cs
/// Author: David Azouz
/// Date Created: 20/06/2016
/// Date Modified: 6/2016
/// ----------------------------------
/// Brief: Player Controller class that controls the player.
/// viewed: https://unity3d.com/learn/tutorials/projects/roll-a-ball/moving-the-player
/// http://wiki.unity3d.com/index.php?title=Xbox360Controller
/// http://answers.unity3d.com/questions/788043/is-it-possible-to-translate-an-object-diagonally-a.html
/// *Edit*
/// - Player state machine - David Azouz 20/06/2016
/// - Player moving at a 45 degree angle - David Azouz 20/06/2016
/// TODO:
/// - More than one player added - /6/2016
/// - 
/// </summary>
/// ----------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public const uint MAX_PLAYERS = 2; //TODO: more than two players?

    // PRIVATE VARIABLES [MenuItem ("MyMenu/Do Something")]
    [Header("Movement")]
    public float playerSpeed = 10.0f;
    public float speedBoost = 6.0f;
    public bool m_Moving = false;

    [Tooltip("This will change at runtime.")]
    [Header("Health")]
    public int hitsBeforeDeath = 5;
    public int health = 100;
    public float m_JumpHeight = 5; 
    // these will change for each player  
    public string verticalAxis = "_Vertical";
    public string horizontalAxis = "_Horizontal";
    public string rotationAxisX = "_Rotation_X";
	public string rotationAxisY = "_Rotation_Y";
	public string Fire = "_Fire";
	public string Melee = "_Melee";
    public string Jump = "_Jump";
    [HideInInspector]
    public bool m_IsBoss = false;

    Animator animator;
    public PlayerShooting m_ShootingManager;

    public AudioSource dizzyBirds;
    //public GameManager 
    [Header("Weapon")]
    public GameObject r_weapon;
    public GameObject r_gameOverPanel;
    //public GameObject r_bombExplosionParticleEffect;
    //choosing player states
    [HideInInspector]
    public enum E_PLAYER_STATE
    {
        E_PLAYER_STATE_ALIVE,
        E_PLAYER_STATE_BOSS, //TODO: add more?
        E_PLAYER_STATE_TEAMUP,
        E_PLAYER_STATE_DEAD,

        E_PLAYER_STATE_COUNT,
    };
    public E_PLAYER_STATE m_eCurrentPlayerState;

    // what class the player is
    [HideInInspector]
    public enum E_CLASS_STATE
    {
        E_CLASS_STATE_ROCKYROAD, //ice-cream
        E_CLASS_STATE_BROCCOLION,
        E_CLASS_STATE_WATERMELOMON,
        E_CLASS_STATE_KARATEA,

        E_PLAYER_STATE_COUNT,
    };
    public E_PLAYER_STATE m_eCurrentClassState;

    // PRIVATE VARIABLES
    // A way to identidy players
    [SerializeField] private uint m_playerID = 0;
    private float fRot = 0.2f;
    // Health
    private int healthDeduct = 0;
    //private healthBar healthBars;

    // Use this for initialization
    void Start ()
    {
		//setting our current state to alive
        m_eCurrentPlayerState = E_PLAYER_STATE.E_PLAYER_STATE_ALIVE;

        verticalAxis = "_Vertical";
        horizontalAxis = "_Horizontal";
        rotationAxisX = "_Rotation_X";
        rotationAxisY = "_Rotation_Y";
        Fire = "_Fire";
        Melee = "_Melee";
        Jump = "_Jump";

        // Loops through our players and assigns variables for input from different controllers
        for (uint i = 0; i < MAX_PLAYERS; ++i)
        {
            if (m_playerID == i)
            {
                verticalAxis = "P" + (i + 1) + verticalAxis; //"_Vertical";
                horizontalAxis = "P" + (i + 1) + horizontalAxis; // "_Horizontal";
                rotationAxisX = "P" + (i + 1) + rotationAxisX; // "_Rotation_X";
                rotationAxisY = "P" + (i + 1) + rotationAxisY; // "_Rotation_Y";
				Fire = "P" + (i + 1) + Fire;
                Melee = "P" + (i + 1) + Melee;
                Jump = "P" + (i + 1) + Jump;
            }
        }
        m_ShootingManager.SetFire(Fire);
        //TODO: healthBars = FindObjectOfType<healthBar> ();
        healthDeduct = health / hitsBeforeDeath;
        animator = GetComponentInChildren<Animator> ();
    }

    // Update is called once per frame
    void Update ()
    {
        //creating a variable that gets the input axis
        float moveHorizontal = Input.GetAxis(horizontalAxis);
        float moveVertical = Input.GetAxis(verticalAxis);
        float moveRotationX = Input.GetAxis(rotationAxisX);
        float moveRotationY = Input.GetAxis(rotationAxisY);

        // Movement
        if (moveHorizontal < -fRot || moveHorizontal > fRot ||
                  moveVertical < -fRot || moveVertical > fRot)
        {
            m_Moving = true;
            Vector3 movementDirection = new Vector3 (moveHorizontal, 0.0f, moveVertical);
            movementDirection = Quaternion.Euler(0, 45, 0) * movementDirection;
            Vector3 pos = transform.position + movementDirection * playerSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp (transform.position, pos, 0.2f);

            //Debug.Log("HELP");
            //animator.SetBool("Walking", true);
            //c_walk.CrossFade("Walk");
        }
        // we're not moving so play the idle animation
        else
        {
            m_Moving = false;
            //animator.SetBool ("Walking", false);
            //c_idle.Play ("idle");
        }

        // If we are rotating
        // Rotation/ Direction with the (right) analog stick
        if (moveRotationX < -fRot || moveRotationX > fRot ||
            moveRotationY < -fRot || moveRotationY > fRot)
        {
            transform.forward = new Vector3(moveRotationX, 0.0f, moveRotationY);
            transform.forward = Quaternion.Euler(0, -45, 0) * transform.forward;
            //Debug.LogFormat("{0}", m_playerID);
        }

        // if we topple over
        if (Input.GetButton("Reset"))
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, 0.2f);
			m_eCurrentPlayerState = E_PLAYER_STATE.E_PLAYER_STATE_ALIVE;
        }
		if (Input.GetButton ("Jump"))
        {
            //TODO: Jump
		}

        if (health <= 0)
        {
            m_eCurrentPlayerState = E_PLAYER_STATE.E_PLAYER_STATE_DEAD;
            //DO STUFF
            //animator.SetBool("Dead", true);
        }

        //Switches between player states
        #region Player States
        switch (m_eCurrentPlayerState)
        {
            //checks if the player is alive
            case E_PLAYER_STATE.E_PLAYER_STATE_ALIVE:
                {
                    //gameObject.GetComponent<BossBlobs>().enabled = false;
                    //r_weapon.SetActive(false);
                    //Debug.Log("Alive!");
                    break;
                }
            case E_PLAYER_STATE.E_PLAYER_STATE_BOSS:
                {
                    //gameObject.GetComponent<BossBlobs>().enabled = true;
                    r_weapon.SetActive(true);
                    //Debug.Log("Boss");
                    break;
                }
            /*case E_PLAYER_STATE.E_PLAYER_STATE_TEAMUP:
                {
                    // if player 'A' and 'B's' states
                    switch(m_eCurrentClassState)
                    {
                        case E_CLASS_STATE.E_CLASS_STATE_ROCKYROAD:
                            {

                            }
                    }
                    m_playerID
                } */
            //if player is dead
            case E_PLAYER_STATE.E_PLAYER_STATE_DEAD:
                {
                    //gameObject.GetComponent<BossBlobs>().enabled = false;
                    // Particle effect bomb (explosion)
                    //r_bombExplosionParticleEffect.SetActive(true);
                    // actions to perform after a certain time
                    uint uiBombEffectTimer = 2;
                    Invoke("BombEffectDead", uiBombEffectTimer);
//					c_death.CrossFade("Death");

                    Debug.Log("Dead :(");
                    break;
                }
            default:
                {
                    Debug.LogError("No State Chosen!");
                    break;
                }
        }
        #endregion
	}

    void BombEffectDead()
    {
        //r_weapon.SetActive(false);
        Destroy(this.gameObject);
        r_gameOverPanel.SetActive(true); 
        Time.timeScale = 0;
        // After three seconds, return to menu
        Invoke("ReturnToMenu", 1);
        Debug.Log("Bomb Effect");
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public E_PLAYER_STATE ChangeStateBoss()
    {
        return E_PLAYER_STATE.E_PLAYER_STATE_BOSS;
    }

    public E_PLAYER_STATE ChangeStateTeamup()
    {
        return E_PLAYER_STATE.E_PLAYER_STATE_TEAMUP;
    }

    public E_PLAYER_STATE ChangeStateDead()
    {
        return E_PLAYER_STATE.E_PLAYER_STATE_DEAD;
    }

    public void SetPlayerStateDead(uint a_uiPlayerStateDead)
    {
        m_eCurrentPlayerState = (E_PLAYER_STATE)a_uiPlayerStateDead;
    }

    public uint GetPlayerID()
    {
        return m_playerID;
    }

    public void SetPlayerID(uint a_uiPlayerID)
    {
        m_playerID = a_uiPlayerID; 
    }

    // Upon Collision TODO: is this still needed?
    /*void OnCollisionEnter()
    {
        Vector3 v3PreviousPos = transform.localPosition;
        transform.parent.position = transform.localPosition;
        transform.position = v3PreviousPos;
    } */


    void OnCollisionEnter(Collision a_collision)
    {
        if (a_collision.gameObject.tag == "Weapon")
        {
            Debug.Log("PC: HIT");
            health -= healthDeduct; //20?
            dizzyBirds.Play();

			float healthFraction = 1.0f - (float)health / 100;
			healthFraction = Mathf.Lerp (0, 5, healthFraction);
			int healthImageID = Mathf.FloorToInt (healthFraction);

			//healthBars.healthHit (m_playerID, healthImageID);
        }
    }
}
