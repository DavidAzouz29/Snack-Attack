using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BossBlobs : MonoBehaviour
{

    /*
    Whoever is currently the boss, when going below a certain ppwer threshold, should drop blobs around them.
    */
    public Collider[] IgnoreAttacksFrom;

    [Tooltip("Use these to specify at what Power the boss drops its blobs.")]
    public List<int> BossDropThreshold = new List<int>(new int[] { 200, 132, 66 });

    [Tooltip("Use these to specify how many blobs to drop")]
    public List<int> m_BlobsToDrop = new List<int>(new int[] { 3, 2, 1 });

    [Tooltip("use these to specify how much power the blobs will give")]
    public List<int> m_PowerToGive = new List<int>(new int[] { 5, 10, 15, 20 });

    [Tooltip("The scale of each different power level")]
    public List<float> m_PowerLevelScale = new List<float>(new float[] { 1.5f, 1.0f, 0.75f });


    public int m_CurrentThreshold;
    // Power (Boss)
    public int m_Power;
    public int m_PowerMax = 150;
    public bool m_Updated = false;
    public int m_Knockback = 150;

    private Killbox m_Killbox;
    private bool activeWeaponCheck;
    private int m_KillCounter = 1;

    //Emission Color Varriables
    private Color m_EmissionColor;
    private float m_EmissionTimer;
    private bool m_EmissionTimerEnabled;
    public float m_EmissionThreshold;
    public List<SkinnedMeshRenderer> m_ModelMeshRenderers;

    public enum Thresholds
    {
        BIG,
        REGULAR,
        SMALL
    }
    public enum TransitionState
    {
        BOSS,
        NEUT,
        WEAK
    }

    public Thresholds m_Threshold;
    public TransitionState m_TransitionState;

    public struct Blobs
    {
        public int BigThresh, RegularThresh, SmallThresh;
        public int BigDrop, RegularDrop, SmallDrop;
        public int BigPower, RegularPower, SmallPower;
        public float BigScale, RegularScale, SmallScale;
    };

    public Blobs m_Blobs;
    //public UIBossLevel r_UIBoss;

    /*
        Could turn this into a list, for different characters and have different
        prefabs for different bosses. Eg; Watermelon slices for watermelon boss blobs.
    */
    public GameObject m_SpawnableBlob; // this is used to spawn a blob based on class 		    public GameObject m_BlobObject; 
    private GameObject _curBlob; // buffer storage

    [HideInInspector]
    public List<GameObject> m_BlobsCreated; // Used to manage the instantiated blobs, and to only explode those.

    private PlayerManager r_PlayerMan;
    private PlayerController r_PlayerCon;
    [SerializeField]
    private GameObject[] blobsArray = new GameObject[PlayerManager.MAX_PLAYERS];
    private bool m_Respawned = false;

    void Start()
    {
        IgnoreAttacksFrom = GetComponentsInChildren<SphereCollider>();
        //for(int i = 0; i < AttackIgnore.Length; i++)
        //{
        //   Physics.IgnoreCollision(gameObject.GetComponent<BoxCollider>(), AttackIgnore[i]);
        //   Physics.IgnoreCollision(gameObject.GetComponent<CapsuleCollider>(), AttackIgnore[i]);
        //}
        m_Killbox = FindObjectOfType<Killbox>();
        InitializeStruct();

        m_Threshold = Thresholds.REGULAR;
        m_TransitionState = TransitionState.NEUT;
        m_Power = 132;
        m_CurrentThreshold = m_Blobs.RegularThresh;
        transform.localScale = new Vector3(m_PowerLevelScale[1], m_PowerLevelScale[1], m_PowerLevelScale[1]);
        r_PlayerMan = FindObjectOfType<PlayerManager>();
        r_PlayerCon = GetComponent<PlayerController>();
        blobsArray = r_PlayerMan.GetBlobArray();
        m_EmissionColor = new Color(0.3f, 0.6f, 0.6f);
        m_EmissionTimer = 0f;
        m_EmissionTimerEnabled = false;
        m_EmissionThreshold = 2f;
    }

    void InitializeStruct()
    {
        m_Blobs.BigDrop = m_BlobsToDrop[0];
        m_Blobs.RegularDrop = m_BlobsToDrop[1];
        m_Blobs.SmallDrop = m_BlobsToDrop[2];

        m_Blobs.BigPower = m_PowerToGive[0];
        m_Blobs.RegularPower = m_PowerToGive[1];
        m_Blobs.SmallPower = m_PowerToGive[2];

        m_Blobs.BigScale = m_PowerLevelScale[0];
        m_Blobs.RegularScale = m_PowerLevelScale[1];
        m_Blobs.SmallScale = m_PowerLevelScale[2];

        m_Blobs.BigThresh = BossDropThreshold[0];
        m_Blobs.RegularThresh = BossDropThreshold[1];
        m_Blobs.SmallThresh = BossDropThreshold[2];
    }

    void Update()
    {
        if (m_Power <= 0 && !m_Respawned)
        {
            m_Killbox.PlayerRespawn(gameObject);
            m_Respawned = true;
        }
        UpdateScale();
        if (m_EmissionTimerEnabled)
            m_EmissionTimer += Time.deltaTime;
        if (m_EmissionTimer > m_EmissionThreshold)
        {
            for (int i = 0; i < m_ModelMeshRenderers.Count; i++)
            {
                m_ModelMeshRenderers[i].material.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    void UpdateScale()
    {
        if (m_Updated)
        {
            m_Updated = false;
            if (m_Power >= BossDropThreshold[0]) // if power >= 200
            {
                transform.localScale = new Vector3(m_PowerLevelScale[0], m_PowerLevelScale[0], m_PowerLevelScale[0]);
                gameObject.transform.FindChild("Boss").gameObject.SetActive(true);
                gameObject.transform.FindChild("Boss").gameObject.GetComponent<Animator>().SetBool("Boss", true);
                gameObject.transform.FindChild("Neut").gameObject.SetActive(false);
                gameObject.transform.FindChild("Weak").gameObject.SetActive(false);
                gameObject.GetComponent<PlayerAnims>().m_Anim = gameObject.transform.FindChild("Boss").GetComponent<Animator>();
                gameObject.GetComponent<PlayerAnims>().m_Anim.SetBool("Boss", true);
                m_TransitionState = TransitionState.BOSS;
                m_Threshold = Thresholds.BIG;
            }
            else if (m_Power >= BossDropThreshold[1])
            {
                transform.localScale = new Vector3(m_PowerLevelScale[1], m_PowerLevelScale[1], m_PowerLevelScale[1]);
                gameObject.transform.FindChild("Boss").gameObject.SetActive(false);
                gameObject.transform.FindChild("Neut").gameObject.SetActive(true);
                gameObject.transform.FindChild("Neut").gameObject.GetComponent<Animator>().SetBool("Boss", false);
                gameObject.transform.FindChild("Weak").gameObject.SetActive(false);
                gameObject.GetComponent<PlayerAnims>().m_Anim = gameObject.transform.FindChild("Neut").GetComponent<Animator>();
                m_TransitionState = TransitionState.NEUT;
                m_Threshold = Thresholds.REGULAR;
            }
            else if (m_Power >= BossDropThreshold[2])
            {
                transform.localScale = new Vector3(m_PowerLevelScale[2], m_PowerLevelScale[2], m_PowerLevelScale[2]);
                gameObject.transform.FindChild("Boss").gameObject.SetActive(false);
                gameObject.transform.FindChild("Neut").gameObject.SetActive(false);
                gameObject.transform.FindChild("Weak").gameObject.SetActive(true);
                gameObject.transform.FindChild("Weak").gameObject.GetComponent<Animator>().SetBool("Boss", false);
                gameObject.GetComponent<PlayerAnims>().m_Anim = gameObject.transform.FindChild("Weak").GetComponent<Animator>();
                m_TransitionState = TransitionState.WEAK;
                m_Threshold = Thresholds.SMALL;
            }
        }
    }

    void OnTriggerEnter(Collider _col)
    {
        PlayerAnims m_LocalAnim = gameObject.GetComponent<PlayerAnims>();
        PlayerAnims m_ColliderAnim = _col.gameObject.GetComponentInParent<PlayerAnims>();
        //Check if a player is hiting us
        if (_col.gameObject.tag == "Weapon1" || _col.gameObject.tag == "Weapon2")
        {
            //Check if its an active hit
            if (_col.gameObject.GetComponent<PlayerCollision>().weaponIsActive)
            {
                //checked if we are blocking Blocking
                if (m_LocalAnim.m_Anim.GetBool("Blocking"))
                {
                    //If Blocking

                    //Check Heavy Attack
                    if (_col.gameObject.GetComponent<PlayerCollision>().isHeavyAttack)
                    {
                        //If Heacy Attack

                        //check if its a boss heavy attack
                        if (m_ColliderAnim.m_Anim.GetBool("Boss"))
                        {
                            //Stun player and apply damage
                            m_LocalAnim.m_Anim.SetTrigger("Stunned");
                            ApplyDamage(_col);
                        }
                        //Else End Block
                        else
                        {

                            m_LocalAnim.m_Anim.SetTrigger("BlockEnd");
                            m_LocalAnim.m_Anim.SetBool("Blocking", false);
                        }
                    }

                }
                else
                {
                    //If were Not blocking apply damage
                    ApplyDamage(_col);
                }
            }
        }
    }

    public void Drop(Thresholds _t)
    {
        //GameObject _curBlob = null;
        // Spawn *type* of projectile based of player class
        #region Blobs
        switch (r_PlayerCon.m_eCurrentClassState)
        {
            case PlayerController.E_CLASS_STATE.E_CLASS_STATE_ROCKYROAD:
                {
                    m_SpawnableBlob = blobsArray[0];
                    break;
                }
            case PlayerController.E_CLASS_STATE.E_CLASS_STATE_BROCCOLION:
                {
                    m_SpawnableBlob = blobsArray[1];
                    break;
                }
            case PlayerController.E_CLASS_STATE.E_CLASS_STATE_WATERMELOMON:
                {
                    m_SpawnableBlob = blobsArray[2];
                    break;
                }
            case PlayerController.E_CLASS_STATE.E_CLASS_STATE_KARATEA:
                {
                    m_SpawnableBlob = blobsArray[0];
                    break;
                }
            case PlayerController.E_CLASS_STATE.E_CLASS_STATE_CAUILILION:
                {
                    m_SpawnableBlob = blobsArray[3];
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
        #endregion

        switch (_t)
        {
            #region BIG
            case Thresholds.BIG:
                for (int i = 0; i < m_Blobs.BigDrop; i++)
                {
                    int a = i * (360 / m_Blobs.BigDrop);
                    _curBlob = (GameObject)Instantiate(m_SpawnableBlob, BlobSpawn(a), Quaternion.identity);
                    _curBlob.GetComponent<BlobCollision>().m_PowerToGive = m_Blobs.BigPower;
                    m_BlobsCreated.Add(_curBlob);
                }
                // Apply Explosion
                ExplodeBlobs();
                m_BlobsCreated.Clear();

                // Everytime a player goes down a threshold, lower their scale by .25
                gameObject.transform.localScale = new Vector3(m_PowerLevelScale[1], m_PowerLevelScale[1], m_PowerLevelScale[1]);
                gameObject.transform.FindChild("Boss").gameObject.SetActive(false);
                gameObject.transform.FindChild("Neut").gameObject.SetActive(true);
                gameObject.transform.FindChild("Neut").gameObject.GetComponent<Animator>().SetBool("Boss", false);
                gameObject.transform.FindChild("Weak").gameObject.SetActive(false);
                gameObject.GetComponent<PlayerAnims>().m_Anim.SetBool("Boss", false);
                gameObject.GetComponent<PlayerAnims>().m_Anim = gameObject.transform.FindChild("Neut").GetComponent<Animator>();

                m_TransitionState = TransitionState.NEUT;
                m_CurrentThreshold = m_Blobs.RegularThresh;
                m_Threshold = Thresholds.REGULAR;
                break;
            #endregion

            #region REGULAR
            case Thresholds.REGULAR:
                for (int i = 0; i < m_Blobs.RegularDrop; i++)
                {
                    int a = i * (360 / m_Blobs.RegularDrop);
                    _curBlob = (GameObject)Instantiate(m_SpawnableBlob, BlobSpawn(a), Quaternion.identity);
                    _curBlob.GetComponent<BlobCollision>().m_PowerToGive = m_Blobs.RegularPower;
                    m_BlobsCreated.Add(_curBlob);
                }
                // Apply Explosion
                ExplodeBlobs();
                m_BlobsCreated.Clear();

                // Everytime a player goes down a threshold, lower their scale by .25
                gameObject.transform.localScale = new Vector3(m_PowerLevelScale[2], m_PowerLevelScale[2], m_PowerLevelScale[2]);
                gameObject.transform.FindChild("Boss").gameObject.SetActive(false);
                gameObject.transform.FindChild("Neut").gameObject.SetActive(false);
                gameObject.transform.FindChild("Weak").gameObject.SetActive(true);
                gameObject.transform.FindChild("Weak").gameObject.GetComponent<Animator>().SetBool("Boss", false);
                gameObject.GetComponent<PlayerAnims>().m_Anim = gameObject.transform.FindChild("Weak").GetComponent<Animator>();

                m_TransitionState = TransitionState.WEAK;
                m_CurrentThreshold = m_Blobs.SmallThresh;
                m_Threshold = Thresholds.SMALL;

                //r_UIBoss.SkullOff();
                break;
            #endregion

            #region SMALL
            case Thresholds.SMALL:
                for (int i = 0; i < m_Blobs.RegularDrop; i++)
                {
                    int a = i * (360 / m_Blobs.RegularDrop);
                    _curBlob = (GameObject)Instantiate(m_SpawnableBlob, BlobSpawn(a), Quaternion.identity);
                    _curBlob.GetComponent<BlobCollision>().m_PowerToGive = m_Blobs.RegularPower;
                    m_BlobsCreated.Add(_curBlob);
                }
                // Apply Explosion
                ExplodeBlobs();
                m_BlobsCreated.Clear();
                break;
            #endregion

            default:
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
        m_Threshold = Thresholds.SMALL;
        m_Power = 66;
        m_CurrentThreshold = m_Blobs.SmallThresh;
        transform.localScale = new Vector3(m_PowerLevelScale[2], m_PowerLevelScale[2], m_PowerLevelScale[2]);
        gameObject.transform.FindChild("Boss").gameObject.SetActive(false);
        gameObject.transform.FindChild("Neut").gameObject.SetActive(false);
        gameObject.transform.FindChild("Weak").gameObject.SetActive(true);
        gameObject.transform.FindChild("Weak").gameObject.GetComponent<Animator>().SetBool("Boss", false);
        gameObject.GetComponent<PlayerAnims>().m_Anim.SetBool("Boss", false);
        gameObject.GetComponent<PlayerAnims>().m_Anim = gameObject.transform.FindChild("Weak").GetComponent<Animator>();

        m_TransitionState = TransitionState.WEAK;
    }

    public void UpdateScore(Collider _col)
    {
        ScoreManager myScore = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreManager>();
        myScore.ChangeScore(_col.gameObject.GetComponentInParent<PlayerController>().m_PlayerTag, "kills", 1);
        GameObject.FindGameObjectWithTag(_col.gameObject.GetComponentInParent<PlayerController>().m_PlayerTag + " Score").GetComponent<Text>().text = m_KillCounter.ToString();
        m_KillCounter++;
        Debug.Log(m_KillCounter);
        GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreManager>().ChangeScore(gameObject.GetComponent<PlayerController>().m_PlayerTag, "deaths", 1);
    }

    public void ApplyDamage(Collider _col)
    {
        //Apply Emmision Map
        Debug.Log("Model renderers " + m_ModelMeshRenderers.Count);
        for (int i = 0; i < m_ModelMeshRenderers.Count; i++)
        {
            m_ModelMeshRenderers[i].material.SetColor("_EmissionColor", m_EmissionColor);
        }

        //Apply Knockback
        Vector3 Dir = _col.GetComponent<PlayerCollision>().m_ParentTransform.position - gameObject.transform.position;
        gameObject.GetComponent<Rigidbody>().AddForce(Dir.normalized * -m_Knockback);

        //Apply Damage
        m_Power = m_Power - _col.gameObject.GetComponent<PlayerCollision>().damage; // Power - Damage recieved
        if (m_Power < m_CurrentThreshold)
        {
            Drop(m_Threshold);
        }
        if (m_Power <= 0)
        {
            m_Killbox.StartCoroutine(m_Killbox.IRespawn(gameObject));
            UpdateScore(_col);
        }
    }
}

