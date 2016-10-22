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
/// - Added Game Loop code - David Azouz 2/10/2016
/// TODO:
/// - 
/// //SceneManager.MoveGameObjectToScene(this.gameObject,
///    SceneManager.GetSceneAt(FindObjectOfType<MenuScript>().GetLevelSelection()));
/// </summary>
/// ----------------------------------
/// 
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public PlayerManager r_PlayerManager; //TODO: why not PlayerManager?
    //const uint MAX_PLAYERS = PlayerManager.MAX_PLAYERS; //TODO: needed?

    private int m_RandomPlayer;
    private float m_BossScale;

    public float m_StartDelay = 3f;             // The delay between the start of RoundStarting and RoundPlaying phases.
    public float m_EndDelay = 3f;               // The delay between the end of RoundPlaying and RoundEnding phases.
    public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.
    public Text m_MessageText;                  // Reference to the overlay Text to display winning text, etc.
    public SnackThinker m_SnackPrefab;             // Reference to the prefab the players will control.
    public Transform[] SpawnPoints;

    private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
    private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.
    private List<SnackThinker> m_Snacks;

    public GameSettings m_ActiveGameSettings;

    [SerializeField] float m_GravityForce = 9.8f;      // Used once at start to set gravity of all rigidbodies.

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            // If we have an instance
            if (_instance != null)
            {
                return _instance;
            }
            // If we're null
            // Still need to call this for the build/ first initialisation in Splash.
            GameManager gameManager = FindObjectOfType<GameManager>(); //.GetComponent<GameManager>();
            // If we've found an object
            if (gameManager != null)
            {
                _instance = gameManager; //_instance = Instantiate(gameManager);
                return _instance;
            }
#if UNITY_EDITOR
            // Still null
            // In a build we will always have a Game Manager as we start from the splash screen.
            // This is made to make testing much simpler.
            Object prefab = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/ScenePrefabs/GameManager.prefab", typeof(GameObject));
            _instance = (UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject).GetComponent<GameManager>();
            // you may now modify the game object
            //_instance.transform.position = Vector3.one;
            //UnityEditor.Selection.activeGameObject = obj;
#endif
            return _instance;
        }
    }

    void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == Scene.Menu)
        {
#if UNITY_EDITOR
            UnityEditor.PrefabUtility.ResetToPrefabState(this.gameObject);
#endif
        }
        // Splash
        else if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        r_PlayerManager = GetComponent<PlayerManager>();

        // Create the delays so they only have to be made once.
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        AudioThemeSelection();

        Physics.gravity = new Vector3(0, -m_GravityForce, 0);

        //SpawnAllSnacks(); //TODO: restore
        //SetCameraTargets();

        // Once the Snacks have been created and the camera is using them as targets, start the game.
        //StartCoroutine(GameLoop());
    }

    // TODO: move to RoundStarting()
    public void Init()
    {
        AudioThemeSelection();
        SetupBoss();
    }

    public void AudioThemeSelection()
    {
        AudioSource audioSlot = null;
        int iScene = SceneManager.GetActiveScene().buildIndex;
        switch (iScene)
        {
            case 0:
            case 1: //Scene.Menu
            default:
                {
                    // For Splash only
                    audioSlot = transform.GetChild(2).GetComponentInChildren<AudioSource>();
                    audioSlot.pitch = Random.Range(0.78f, 1.5f); //TODO: remove
                    audioSlot.loop = true;
                    audioSlot.Play();
                    break;
                }
            case 2: // Scene.Level1Kitchen:
            case 3: // Scene.Level2Banquet:
                {
                    transform.GetChild(2).GetComponentInChildren<AudioSource>().enabled = false; //TODO: solve this
                    audioSlot = transform.GetChild(2).GetChild(iScene - 1).GetComponent<AudioSource>(); Debug.Log("GM: Theme " + iScene);
                    audioSlot.loop = true;
                    audioSlot.Play();
                    break;
                }
        }
    }

    public void SetupBoss()
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

    private void SpawnAllSnacks()
    {
        var points = new List<Transform>(SpawnPoints);

        m_Snacks = new List<SnackThinker>();

        foreach (GameState.PlayerState state in GameState.Instance.players)
        {
            var spawnPointIndex = Random.Range(0, points.Count);

            // ... create them, set their player number and references needed for control.
            var Snack = Instantiate(m_SnackPrefab);
            Snack.Setup(state, points[spawnPointIndex]);

            points.RemoveAt(spawnPointIndex);

            m_Snacks.Add(Snack);
        }
    }

    //
    private void SetCameraTargets()
    {
        // Create a collection of transforms the same size as the number of Snacks.
        m_CameraControl.m_Targets = new Transform[m_Snacks.Count];

        // For each of these transforms...
        for (int i = 0; i < m_Snacks.Count; i++)
        {
            // ... set it to the appropriate Snack transform.
            m_CameraControl.m_Targets[i] = m_Snacks[i].transform;
        }
    }


    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoop()
    {
        GameSettings.Instance.OnBeginRound();

        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundStarting());

        // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundPlaying());

        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
        yield return StartCoroutine(RoundEnding());

        // This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.
        if (GameSettings.Instance.ShouldFinishGame())
        {
            // TODO: set scoreboard on
            Debug.Log("GM Game Loop, Should Finish Game");//SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(Scene.Level1Kitchen /*1*/, LoadSceneMode.Single);
        }
    }

    //
    private IEnumerator RoundStarting()
    {
        // As soon as the round starts reset the Snacks and make sure they can't move.
        DisableSnackControl();

        // Snap the camera's zoom and position to something appropriate for the reset Snacks.
        m_CameraControl.SetStartPositionAndSize();

        // Increment the round number and display text showing the players what round it is.
        m_MessageText.text = "ROUND " + GameState.Instance.RoundNumber;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_StartWait;
    }

    //
    private IEnumerator RoundPlaying()
    {
        // As soon as the round begins playing let the players control the Snacks.
        EnableSnackControl();

        // Clear the text from the screen.
        m_MessageText.text = string.Empty;

        // While there is not one Snack left...
        while (!GameSettings.Instance.ShouldFinishRound())
        {
            // ... return on the next frame.
            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {
        // Stop Snacks from moving.
        DisableSnackControl();

        var winner = GameSettings.Instance.OnEndRound();

        // Get a message based on the scores and whether or not there is a game winner and display it.
        string message = EndMessage(winner);
        m_MessageText.text = message;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_EndWait;
    }

    // Returns a string message to display at the end of each round.
    private string EndMessage(SnackThinker winner)
    {
        return winner != null ? winner.player.PlayerInfo.GetColoredName() + " WINS THE ROUND!" : "DRAW!";
    }

    private void EnableSnackControl()
    {
        for (int i = 0; i < m_Snacks.Count; i++)
        {
            if (m_Snacks[i])
                m_Snacks[i].enabled = true;
        }
    }


    private void DisableSnackControl()
    {
        for (int i = 0; i < m_Snacks.Count; i++)
        {
            if (m_Snacks[i])
                m_Snacks[i].enabled = false;
        }
    }

    // Update is called once per frame
    //void Update () {	}
}
