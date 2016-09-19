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
/// Bouncey/ Tramp degree of difficulty 
/// http://www.motionscript.com/articles/bounce-and-overshoot.html#calc-overshoot
/// https://youtu.be/J6EEninU8g0
/// https://docs.google.com/document/d/12ymsMLNA0oiIZc8_iXS9zuEMUH1NGV1zbvwnHkm21DM/edit
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

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public const uint MAX_PLAYERS = PlayerManager.MAX_PLAYERS;

    // PRIVATE VARIABLES [MenuItem ("MyMenu/Do Something")]
    [Header("Movement")]
    public float playerSpeed = 10.0f;
    public float speedBoost = 6.0f;
    public bool m_Moving = false;

    [Tooltip("This will change at runtime.")]
    [Header("Health")]
    public int hitsBeforeDeath = 5;
    public int health = 100;
    
    // these will change for each player 
    [Header("KeyBinds")] 
    public string verticalAxis = "_Vertical";
    public string horizontalAxis = "_Horizontal";
	public string Attack1 = "_Attack1";
	public string Attack2 = "_Attack2";
    public string Jump = "_Jump";
    [HideInInspector]
    public string m_PlayerTag = "NoPlayerAttached";
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
        E_CLASS_STATE_ROCKYROAD,
        E_CLASS_STATE_BROCCOLION,
        E_CLASS_STATE_WATERMELOMON,
        E_CLASS_STATE_KARATEA,
        E_CLASS_STATE_PATTYCAKE,
        E_CLASS_STATE_CAUILILION,
        E_CLASS_STATE_ROCKMELOMON,

        E_PLAYER_STATE_COUNT,
    };
    public E_CLASS_STATE m_eCurrentClassState;
    public bool isOnGround; // set to true if we are on the ground
    public float fJumpForce = 12.0f;
    public float fGravity = 9.8f;

    // PRIVATE VARIABLES
    // A way to identidy players
    Animator animator;
    [SerializeField] private uint m_playerID = 0;
    private float fRot = 0.2f;
    bool isPaused = false;
    // Health
    private int healthDeduct = 0;
    // used for jumping

    Rigidbody rb;
    float fJumpForceMax = 24.0f;// *2;
    private Vector3 m_PreviousPos;

    // Use this for initialization
    void Start ()
    {
		//setting our current state to alive
        m_eCurrentPlayerState = E_PLAYER_STATE.E_PLAYER_STATE_ALIVE;
        verticalAxis = "_Vertical";
        horizontalAxis = "_Horizontal";
        Attack1 = "_Attack1";
        Attack2 = "_Attack2";
        Jump = "_Jump";

        // Loops through our players and assigns variables for input from different controllers
        for (uint i = 0; i < MAX_PLAYERS; ++i)
        {
            if (m_playerID == i)
            {
                verticalAxis = "P" + (i + 1) + verticalAxis; //"_Vertical";
                horizontalAxis = "P" + (i + 1) + horizontalAxis; // "_Horizontal";
                Attack1 = "P" + (i + 1) + Attack1;
                Attack2 = "P" + (i + 1) + Attack2;
                Jump = "P" + (i + 1) + Jump;
                m_PlayerTag = "Player " + (i + 1);
            }
        }
        //m_ShootingManager.SetFire(Fire);
        //TODO: healthBars = FindObjectOfType<healthBar> ();
        healthDeduct = health / hitsBeforeDeath;
        animator = GetComponent<Animator>(); //GetComponentInChildren<Animator> ();
        rb = GetComponent<Rigidbody>();
        //jump
        //fMovementSpeedSlowDown = fMovementSpeed - 2.0f;
        fJumpForce = 8.0f;
        //fJumpForceMax = fJumpForce;// *2;
        m_PreviousPos = transform.position;
    }

    // Should be used for Physics calculations
    void FixedUpdate()
    {
        // Jumping
        // if we're on or close to the ground
        if ((m_PreviousPos.y - transform.position.y) < 0.3f)
        {
            isOnGround = true;
        }
        // Falling
        else
        {
            isOnGround = false;
        }
        if (Input.GetButtonDown(Jump) && isOnGround)
        {
            rb.AddForce(0, fJumpForce, 0, ForceMode.Impulse);
            isOnGround = false;
        }
        m_PreviousPos = transform.position;
    }

    // Update is called once per frame
    void Update ()
    {

        //creating a variable that gets the input axis
        float moveHorizontal = Input.GetAxis(horizontalAxis);
        float moveVertical = Input.GetAxis(verticalAxis);

        // Movement
        // was (moveHorizontal < -fRot || moveHorizontal > fRot)
        if (Mathf.Abs(moveHorizontal) > fRot || Mathf.Abs(moveVertical) > fRot && isPaused == false)
        {
            m_Moving = true;
            // TOOD: rb.AddForce(Vector3.up);
            Vector3 movementDirection = new Vector3 (moveHorizontal, 0.0f, moveVertical);
//            movementDirection = Quaternion.Euler(0, 45, 0) * movementDirection;
            Vector3 pos = transform.position + movementDirection * playerSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp (transform.position, pos, 0.2f);
            transform.forward = new Vector3(-moveVertical, 0.0f, moveHorizontal);
            transform.forward = Quaternion.Euler(0, -270, 0) * transform.forward;
        }
        // we're not moving so play the idle animation
        else
        {
            m_Moving = false;
        }

        if (health <= 0)
        {
            m_eCurrentPlayerState = E_PLAYER_STATE.E_PLAYER_STATE_DEAD;
            //DO STUFF
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
                    //r_weapon.SetActive(true);
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
					//c_death.CrossFade("Death");

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
        //r_gameOverPanel.SetActive(true); 
        Time.timeScale = 0.00001f;
        // After three seconds, return to menu
        Invoke("ReturnToMenu", 1);
        Debug.Log("Bomb Effect");
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    // this is the function that should be used
    public void SetPlayerState(E_PLAYER_STATE a_ePlayerState)
    {
        m_eCurrentPlayerState = a_ePlayerState;
    }

    public uint GetPlayerID()
    {
        return m_playerID;
    }

    public void SetPlayerID(uint a_uiPlayerID)
    {
        m_playerID = a_uiPlayerID; 
    }

    void OnCollisionEnter(Collision a_collision)
    {
        // make jump work
        if (a_collision.transform.tag == "Bench")
        {
            isOnGround = true;
        }

        // Bouncey objects
        if (a_collision.transform.tag == "Bounce")
        {
            float amp = 80;
            float freq = 1;
            float decay = 1;

            float t = Time.time;// - inPoint; //TODO: 
            amp *= Mathf.Sin(t * freq * Mathf.PI * 2) / Mathf.Exp(t * decay);
        }
    }
}
