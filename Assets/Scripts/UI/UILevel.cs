using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UILevel : MonoBehaviour {

	public RoundTimer r_RoundTimer;
	public Text[] r_PlayerScoresText = new Text[PlayerManager.MAX_PLAYERS];

    [SerializeField]
    private Text c_timer;
    [SerializeField]
    private Text c_countDown;

    [SerializeField]
    private Image[] c_playerIcons = new Image[PlayerManager.MAX_PLAYERS];

    float m_Mins;
    float m_Secs;

    private bool m_roundStarted = false;
    private int m_countDown = 3;
    private float timer = 0.0f;
    [SerializeField] private float m_countDownDelay = 2.0f;

    private GameManager r_GameManager;
    private Transform r_AudioCountdown;

    private const int iAudioSlotCountDown = 8;
    private const int iAudioSlotFight = 9;

    void Awake()
    {
        r_GameManager = FindObjectOfType<GameManager>(); // TODO: use GameManager.Instance
        r_RoundTimer = r_GameManager.GetComponent<RoundTimer>();
        r_AudioCountdown = r_GameManager.transform.GetChild(GameManager.iChInGame);
        // Countdown Audio
        r_AudioCountdown.GetChild(iAudioSlotCountDown).GetComponent<AudioSource>().Play();
    }

    // Use this for initialization
    void Start ()
    {
       //r_text.GetComponentInChildren<Text>();
        for (int i = 0; i < PlayerManager.MAX_PLAYERS; i++)
        {
            // Get the neut icon as the default
            c_playerIcons[i].sprite = GameSettings.Instance.players[i].Brain.GetIcon(0);
        }

        // Countdown related stuffs.
        //Time.timeScale = 0.0f;
        timer = m_countDownDelay;
        c_countDown.enabled = true;
        c_timer.enabled = false;
    }

    // Update is called once per frame
    void Update ()
    {
        if (!m_roundStarted) CountDownUpdate();
        else SetClockText();
    }

    void CountDownUpdate()
    {
        c_countDown.text = ( m_countDown > 0 ) ? m_countDown.ToString() : "FIGHT!";

        float scaleValue = (m_countDown > 0) ? timer / m_countDownDelay : 1 - (timer / m_countDownDelay);
        c_countDown.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

        timer -= Time.unscaledDeltaTime;
        if (timer <= 0)
        {
            m_countDown--;
            if (m_countDown < 0)
            {
                m_roundStarted = true;
                c_countDown.enabled = false;
                c_timer.enabled = true;
                Time.timeScale = 1.0f;
                // Enable movement. Read the comment for disabling movement.
                for (int i = 0; i < PlayerManager.MAX_PLAYERS; i++)
                {
                    GameManager.Instance.r_PlayerManager.GetPlayer(i).GetComponent<PlayerController>().enabled = true;
                }
            }
            timer = m_countDownDelay;
            if (m_countDown == 0)
            {
                timer = 0.5f;
                // Fight!
                r_AudioCountdown.GetChild(iAudioSlotFight).GetComponent<AudioSource>().Play();
                // Play correlated level theme: Kitchen/ Banquet
                AudioSource audioSlot = null;
                int iScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
                switch (iScene)
                {
                    case 0:
                    case 1: //Scene.Menu
                    default:
                        {
                            // For Splash only
                            audioSlot = GameManager.Instance.transform.GetChild(GameManager.iChInGame).GetComponentInChildren<AudioSource>();
                            audioSlot.loop = true;
                            audioSlot.Play();
                            break;
                        }
                    case 2: // Scene.Level1Kitchen:
                    case 3: // Scene.Level2Banquet:
                        {
                            GameManager.Instance.transform.GetChild(GameManager.iChInGame).GetComponentInChildren<AudioSource>().enabled = false; //TODO: solve this
                            //audioSlot = transform.GetChild(iScene + 1).GetChild(1).GetComponent<AudioSource>(); //Debug.Log("GM: Theme " + iScene);
                            //audioSlot.loop = true; //TODO: restore once footage is captured
                            //audioSlot.Play();
                            break;
                        }
                }
            }
        }
    }

    void SetClockText()
    {
        m_Mins = Mathf.Floor(r_RoundTimer.GetTimeRemaining() / 60); // Get Minutes Remaining
        m_Secs = Mathf.Floor(r_RoundTimer.GetTimeRemaining() % 60);  // Get Seconds Remaining

        c_timer.text = (m_Mins.ToString() + ":" + m_Secs.ToString()); // Display the time remaining
        if (m_Secs < 10.0f) // If seconds aren't in the 10s, display a 0 before the second.
        {
            c_timer.text = (m_Mins.ToString() + ":" + "0" + m_Secs.ToString());
        }
        // if "0:03" or lower
        if (m_Secs <= 4.9f && m_Secs >= 4.0f && m_Mins < 1)
        {
            // Countdown
            r_AudioCountdown.GetChild(iAudioSlotCountDown).GetComponent<AudioSource>().Play();
        }
    }

    /// <summary>
    /// Switches Icons when their state changes: i.e. evolution or damaged.
    /// </summary>
    /// <param name="a_chosenPlayer">Player's icon to change.</param>
    /// <param name="a_eTransitionState">State to return to.</param>
    /// <param name="a_isDead">Are they dead?</param>
    /// <returns></returns>
    public IEnumerator UpdateIcon(int a_chosenPlayer, BossBlobs.TransitionState a_eTransitionState, bool a_isDead)
    {
        c_playerIcons[a_chosenPlayer].sprite = GameSettings.Instance.players[a_chosenPlayer].Brain.GetIcon(1);
        // If they're dead
        if(a_isDead)
        {
            //"deactivate" their icon: i.e. apply a red tint.
            c_playerIcons[a_chosenPlayer].color = GameSettings.Instance.players[0].Color;
        }
        yield return new WaitForSeconds(0.7f);
        c_playerIcons[a_chosenPlayer].color += Color.white;
        c_playerIcons[a_chosenPlayer].sprite = GameSettings.Instance.players[a_chosenPlayer].Brain.GetIcon(0); //(int)a_eTransitionState
        yield return null;
    }
}
