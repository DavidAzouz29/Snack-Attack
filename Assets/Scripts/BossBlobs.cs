using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossBlobs : MonoBehaviour {

    /*
    Whoever is currently the boss, when going below a certain ppwer threshold, should drop blobs around them.
    */
    public Collider AttackIgnore;

    [Tooltip("Use these to specify at what Power the boss drops its blobs.")]
    public List<int> m_Thresholds = new List<int>(new int[] { 200, 150, 100, 75 });

    [Tooltip("Use these to specify how many blobs to drop")]
    public List<int> m_BlobsToDrop = new List<int>(new int[] { 4, 3, 2, 1 });

    [Tooltip("use these to specify how much power the blobs will give")]
    public List<int> m_PowerToGive = new List<int>(new int[] { 5, 10, 15, 20 });

    [Tooltip("The scale of each different power level")]
    public List<float> m_ScaleLevel = new List<float>(new float[] { 2.0f, 1.5f, 1.0f, 0.75f });
    

    public int m_CurrentThreshold;
    // Power (Boss)
    public int m_Power;
    public int m_PowerMax = 200;
    public bool m_Updated = false;
    public ParticleSystem r_ParticleSystem;

    private Killbox m_Killbox;
    private bool activeWeaponCheck;

    public enum Thresholds
    {
        GIANT,
        BIG,
        REGULAR,
        SMALL
    }

    public Thresholds m_Threshold;

    public struct Blobs
    {
        public int GiantThresh, BigThresh, RegularThresh, SmallThresh;
        public int GiantDrop, BigDrop, RegularDrop, SmallDrop;
        public int GiantPower, BigPower, RegularPower, SmallPower;
        public float GiantScale, BigScale, RegularScale, SmallScale;
    };

    public Blobs m_Blobs;
    //public UIBossLevel r_UIBoss;

    /*
        Could turn this into a list, for different characters and have different
        prefabs for different bosses. Eg; Watermelon slices for watermelon boss blobs.
    */
    public GameObject m_BlobObject; // this is used to spawn a blob based on class 		    public GameObject m_BlobObject; 
    private GameObject _curBlob; // buffer storage

    [HideInInspector]
    public List<GameObject> m_CreatedBlobs; // Used to manage the instantiated blobs, and to only explode those.

    private PlayerManager r_PlayerMan;
    private PlayerController r_PlayerCon;
    [SerializeField]
    private GameObject[] blobsArray = new GameObject[PlayerManager.MAX_PLAYERS];
    private bool m_Respawned = false;

    void Start()
    {
        AttackIgnore = GetComponentInChildren<SphereCollider>();
        Physics.IgnoreCollision(gameObject.GetComponent<BoxCollider>(), AttackIgnore);
        //if (gameObject.GetComponent<CapsuleCollider>() != null)
        Physics.IgnoreCollision(gameObject.GetComponent<CapsuleCollider>(), AttackIgnore);
        m_Killbox = FindObjectOfType<Killbox>();
        InitializeStruct();

        m_Threshold = Thresholds.REGULAR;
        m_Power = 100;
        m_CurrentThreshold = m_Blobs.RegularThresh;
        transform.localScale = new Vector3(m_ScaleLevel[2], m_ScaleLevel[2], m_ScaleLevel[2]);
        r_PlayerMan = FindObjectOfType<PlayerManager>();
        r_PlayerCon = GetComponent<PlayerController>();
        r_ParticleSystem = GetComponent<ParticleSystem>();
        blobsArray = r_PlayerMan.GetBlobArray();
        //r_UIBoss = GetComponentInChildren<UIBossLevel>();
        //m_Blobs initialise
    }

    void InitializeStruct()
    {
        m_Blobs.GiantDrop   = m_BlobsToDrop[0];
        m_Blobs.BigDrop     = m_BlobsToDrop[1];
        m_Blobs.RegularDrop = m_BlobsToDrop[2];
        m_Blobs.SmallDrop   = m_BlobsToDrop[3];

        m_Blobs.GiantPower   = m_PowerToGive[0];
        m_Blobs.BigPower     = m_PowerToGive[1];
        m_Blobs.RegularPower = m_PowerToGive[2];
        m_Blobs.SmallPower   = m_PowerToGive[3];

        m_Blobs.GiantScale   = m_ScaleLevel[0];
        m_Blobs.BigScale     = m_ScaleLevel[1];
        m_Blobs.RegularScale = m_ScaleLevel[2];
        m_Blobs.SmallScale   = m_ScaleLevel[3];

        m_Blobs.GiantThresh   = m_Thresholds[0];
        m_Blobs.BigThresh     = m_Thresholds[1];
        m_Blobs.RegularThresh = m_Thresholds[2];
        m_Blobs.SmallThresh   = m_Thresholds[3];
    }

    void Update()
    {
        if(m_Power <= 0 && !m_Respawned)
        {
            m_Killbox.PlayerRespawn(gameObject);
            m_Respawned = true;
        }
        UpdateScale();
    }
    
    void UpdateScale()
    {
        if (m_Updated)
        {
            m_Updated = false;
            if (m_Power >= m_Thresholds[0]) // if power >= 200
            {
                transform.localScale = new Vector3(m_ScaleLevel[0], m_ScaleLevel[0], m_ScaleLevel[0]);
                gameObject.GetComponent<Animator>().SetBool("Boss", true);
            }
            else if (m_Power >= m_Thresholds[1])
            {
                transform.localScale = new Vector3(m_ScaleLevel[1], m_ScaleLevel[1], m_ScaleLevel[1]);
                gameObject.GetComponent<Animator>().SetBool("Boss", false);
            }
            else if (m_Power >= m_Thresholds[2])
            {
                transform.localScale = new Vector3(m_ScaleLevel[2], m_ScaleLevel[2], m_ScaleLevel[2]);
                gameObject.GetComponent<Animator>().SetBool("Boss", false);
            }
            else if (m_Power >= m_Thresholds[3])
            {
                transform.localScale = new Vector3(m_ScaleLevel[3], m_ScaleLevel[3], m_ScaleLevel[3]);
                gameObject.GetComponent<Animator>().SetBool("Boss", false);
            }
        }
    }

    void OnTriggerEnter(Collider _col)
    {
        if (_col.gameObject.tag == "Weapon")
        {
            Debug.Log("Trigger");
            if (_col.gameObject.GetComponent<PlayerCollision>().weaponIsActive)
            {
                Debug.Log("Active");
                m_Power = m_Power - _col.gameObject.GetComponent<PlayerCollision>().damage; // Power - Damage recieved
                if (m_Power < m_CurrentThreshold)
                {
                    Debug.Log("Dropping blobs");
                    Drop(m_Threshold);
                    //r_ParticleSystem.Play();
                }
                if (m_Power <= 0)
                {
                    m_Killbox.StartCoroutine(m_Killbox.IRespawn(gameObject));
                    GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreManager>().ChangeScore(_col.gameObject.GetComponentInParent<PlayerController>().m_PlayerTag, "kills", 1);
                    GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreManager>().ChangeScore(gameObject.GetComponent<PlayerController>().m_PlayerTag, "deaths", 1);
                }
            }
        }
    }

    public void Drop(Thresholds _t)
    {
        //GameObject _curBlob = null;
        // Spawn *type* of projectile based of player class
        switch (r_PlayerCon.m_eCurrentClassState)
        {
            case PlayerController.E_CLASS_STATE.E_CLASS_STATE_ROCKYROAD:
                {
                    m_BlobObject = blobsArray[0];
                    break;
                }
            case PlayerController.E_CLASS_STATE.E_CLASS_STATE_BROCCOLION:
                {
                    m_BlobObject = blobsArray[1];
                    break;
                }
            case PlayerController.E_CLASS_STATE.E_CLASS_STATE_WATERMELOMON:
                {
                    m_BlobObject = blobsArray[2];
                    break;
                }
            case PlayerController.E_CLASS_STATE.E_CLASS_STATE_KARATEA:
                {
                    m_BlobObject = blobsArray[0];
                    break;
                }
            case PlayerController.E_CLASS_STATE.E_CLASS_STATE_CAUILILION:
                {
                    m_BlobObject = blobsArray[3];
                    //shot.GetComponent<MeshRenderer>().material.mainTexture = r_Coli;
                    //shot.GetComponent<MeshRenderer>().material.SetColor("_SpecColor", Color.white);
                    break;
                }
            default:
                {
                    Debug.LogError("Character animation not set up");
                    break;
                }
        }

        switch (_t)
        {
            #region GIANT
            case Thresholds.GIANT:
                r_PlayerCon.SetPlayerState(PlayerController.E_PLAYER_STATE.E_PLAYER_STATE_BOSS);
                for (int i = 0; i < m_Blobs.GiantDrop; i++)
                {
                    int a = i * (360 / m_Blobs.GiantDrop);
                    _curBlob = (GameObject)Instantiate(m_BlobObject, BlobSpawn(a), Quaternion.identity);
                    _curBlob.GetComponent<BlobCollision>().m_PowerToGive = m_Blobs.GiantPower;
                    m_CreatedBlobs.Add(_curBlob);
                }
                // Apply Explosion
                ExplodeBlobs();
                m_CreatedBlobs.Clear();

                // Everytime a player goes down a threshold, lower their scale by .25
                gameObject.transform.localScale = new Vector3(m_ScaleLevel[1], m_ScaleLevel[1], m_ScaleLevel[1]);

                m_CurrentThreshold = m_Blobs.BigThresh;
                m_Threshold = Thresholds.BIG;

                //r_UIBoss.SkullOn();
                break;
            #endregion

            #region BIG
            case Thresholds.BIG:
                for (int i = 0; i < m_Blobs.BigDrop; i++)
                {
                    int a = i * (360 / m_Blobs.BigDrop);
                    _curBlob = (GameObject)Instantiate(m_BlobObject, BlobSpawn(a), Quaternion.identity);
                    _curBlob.GetComponent<BlobCollision>().m_PowerToGive = m_Blobs.BigPower;
                    m_CreatedBlobs.Add(_curBlob);
                }
                // Apply Explosion
                ExplodeBlobs();
                m_CreatedBlobs.Clear();

                // Everytime a player goes down a threshold, lower their scale by .25
                gameObject.transform.localScale = new Vector3(m_ScaleLevel[2], m_ScaleLevel[2], m_ScaleLevel[2]);
                m_CurrentThreshold = m_Blobs.RegularThresh;
                m_Threshold = Thresholds.REGULAR;
                break;
            #endregion

            #region REGULAR
            case Thresholds.REGULAR:
                for (int i = 0; i < m_Blobs.RegularDrop; i++)
                {
                    int a = i * (360 / m_Blobs.RegularDrop);
                    _curBlob = (GameObject)Instantiate(m_BlobObject, BlobSpawn(a), Quaternion.identity);
                    _curBlob.GetComponent<BlobCollision>().m_PowerToGive = m_Blobs.RegularPower;
                    m_CreatedBlobs.Add(_curBlob);
                }
                // Apply Explosion
                ExplodeBlobs();
                m_CreatedBlobs.Clear();

                // Everytime a player goes down a threshold, lower their scale by .25
                gameObject.transform.localScale = new Vector3(m_ScaleLevel[3], m_ScaleLevel[3], m_ScaleLevel[3]);
                
                m_CurrentThreshold = m_Blobs.SmallThresh;
                m_Threshold = Thresholds.SMALL;

                //r_UIBoss.SkullOff();
                break;
            #endregion

            case Thresholds.SMALL:
                // Kill
                // Add a point to the boss if we were killed by a boss
                /*if (_col.gameObject.GetComponent<PlayerController>().m_eCurrentPlayerState
                    == PlayerController.E_PLAYER_STATE.E_PLAYER_STATE_BOSS)
                {

                } */

                
                //r_UIBoss.SkullOff();
                break;
            default:
                //r_UIBoss.SkullOff();
                break;
        }
    }

    public void ExplodeBlobs()
    {
        float _radius = 3.0f;
        Vector3 _explosionPos = transform.position;
        Collider[] _colliders = Physics.OverlapSphere(_explosionPos, _radius);
        foreach (Collider hit in _colliders)
        {
            if (hit.tag == "SpawningBlob")
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                rb.AddExplosionForce(Random.Range(5.0f, 15.0f), _explosionPos, _radius, 5.0f, ForceMode.Impulse);
                rb.tag = "Blob"; // Reset the tag so forces aren't applied to it again.
                //r_ParticleSystem.Play();
            }
        }
    }

    Vector3 BlobSpawn(int _a)
    {
        Vector3 _center = transform.position;
        float _radius = 2.0f;

        float _angle = _a + Random.Range(5.0f, 40.0f);
        Vector3 _position;

        _position.x = _center.x + _radius * Mathf.Sin(_angle * Mathf.Deg2Rad);
        _position.y = _center.y + 2.0f;
        _position.z = _center.z + _radius * Mathf.Cos(_angle * Mathf.Deg2Rad);

        return _position;
    }

    public void Respawn()
    {
        m_Threshold = Thresholds.REGULAR;
        m_Power = 100;
        m_CurrentThreshold = m_Blobs.RegularThresh;
        transform.localScale = new Vector3(m_ScaleLevel[2], m_ScaleLevel[2], m_ScaleLevel[2]);
    }
}
