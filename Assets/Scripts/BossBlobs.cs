using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossBlobs : MonoBehaviour {

    /*
    Whoever is currently the boss, when going below a certain health threshold, should drop blobs around them.
    */

    [Tooltip("Use these to specify at what health the boss drops its blobs.")]
    public List<int> m_Thresholds = new List<int>(new int[] { 100, 75, 50, 25 });

    [Tooltip("Use these to specify how many blobs to drop")]
    public List<int> m_BlobsToDrop = new List<int>(new int[] { 4, 3, 2, 1 });

    [Tooltip("use these to specify how much power the blobs will give")]
    public List<int> m_PowerToGive = new List<int>(new int[] { 5, 10, 15, 20 });

    [Tooltip("The scale of each different power level")]
    public List<float> m_ScaleLevel = new List<float>(new float[] { 2.0f, 1.5f, 1.0f, 0.75f });
    
    private int m_CurrentThreshold;

    private int m_CurrentHealth;
    private int m_PreviousHealth;

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

    /*
        Could turn this into a list, for different characters and have different
        prefabs for different bosses. Eg; Watermelon slices for watermelon boss blobs.
    */
    public GameObject m_BlobObject; 

    [HideInInspector]
    public List<GameObject> m_CreatedBlobs; // Used to manage the instantiated blobs, and to only explode those.


    void Start()
    {
        InitializeStruct();

        m_Threshold = Thresholds.GIANT;
        m_CurrentHealth = 100;
        m_CurrentThreshold = m_Blobs.GiantThresh;
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
        if(Input.GetKeyDown(KeyCode.F))
        {
            m_CurrentHealth = m_CurrentHealth - 5;
            if (m_CurrentHealth < m_CurrentThreshold)
            {
                Drop(m_Threshold);


            }
        }
    }
    
    void OnCollisionEnter(Collision _col)
    {
        if(_col.gameObject.tag == "Projectile")
        {
            m_PreviousHealth = m_CurrentHealth;
            // Will need to get the damage of the projectile here
            Destroy(_col.gameObject); // Destroy the projectile

            m_CurrentHealth = m_CurrentHealth - 5; // Use projectile damage here
            if(m_CurrentHealth < m_CurrentThreshold) // Does the 'boss' drop a health threshold?
            {
                Drop(m_Threshold);
                m_CurrentThreshold = m_CurrentThreshold - 25; // Drop the threshold.
            }
        }
    }

    void Drop(Thresholds _t)
    {
        switch (_t)
        {
            #region GIANT
            case Thresholds.GIANT:
                for (int i = 0; i < m_Blobs.GiantDrop ; i++)
                {
                    int a = i * (360 / m_Blobs.GiantDrop);
                    GameObject _curBlob = (GameObject)Instantiate(m_BlobObject, BlobSpawn(a), Quaternion.identity);
                    _curBlob.GetComponent<BlobManager>().m_PowerToGive = m_Blobs.GiantPower;
                    m_CreatedBlobs.Add(_curBlob);
                }
                // Apply Explosion
                ExplodeBlobs();
                m_CreatedBlobs.Clear();

                // Everytime a player goes down a threshold, lower their scale by .25
                gameObject.transform.localScale = new Vector3(transform.localScale.x - m_Blobs.GiantScale,
                                                              transform.localScale.y - m_Blobs.GiantScale,
                                                              transform.localScale.z - m_Blobs.GiantScale);

                m_CurrentThreshold = m_Blobs.BigThresh;
                m_Threshold = Thresholds.BIG;
                break;
            #endregion

            #region BIG
            case Thresholds.BIG:
                for (int i = 0; i < m_Blobs.BigDrop; i++)
                {
                    int a = i * (360 / m_Blobs.BigDrop);
                    GameObject _curBlob = (GameObject)Instantiate(m_BlobObject, BlobSpawn(a), Quaternion.identity);
                    _curBlob.GetComponent<BlobManager>().m_PowerToGive = m_Blobs.BigPower;
                    m_CreatedBlobs.Add(_curBlob);
                }
                // Apply Explosion
                ExplodeBlobs();
                m_CreatedBlobs.Clear();

                // Everytime a player goes down a threshold, lower their scale by .25
                gameObject.transform.localScale = new Vector3(transform.localScale.x - m_Blobs.BigScale,
                                                              transform.localScale.y - m_Blobs.BigScale,
                                                              transform.localScale.z - m_Blobs.BigScale);

                m_Threshold = Thresholds.REGULAR;
                break;
            #endregion

            #region REGULAR
            case Thresholds.REGULAR:
                for (int i = 0; i < m_Blobs.RegularDrop; i++)
                {
                    int a = i * (360 / m_Blobs.RegularDrop);
                    GameObject _curBlob = (GameObject)Instantiate(m_BlobObject, BlobSpawn(a), Quaternion.identity);
                    _curBlob.GetComponent<BlobManager>().m_PowerToGive = m_Blobs.RegularPower;
                    m_CreatedBlobs.Add(_curBlob);
                }
                // Apply Explosion
                ExplodeBlobs();
                m_CreatedBlobs.Clear();

                // Everytime a player goes down a threshold, lower their scale by .25
                gameObject.transform.localScale = new Vector3(transform.localScale.x - m_Blobs.RegularScale,
                                                              transform.localScale.y - m_Blobs.RegularScale,
                                                              transform.localScale.z - m_Blobs.RegularScale);

                m_CurrentThreshold = m_Blobs.SmallThresh;
                m_Threshold = Thresholds.SMALL;
                break;
            #endregion

            case Thresholds.SMALL:
                // Kill
                break;
            default:
                break;
        }
    }

    void ExplodeBlobs()
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
        _position.y = _center.y;
        _position.z = _center.z + _radius * Mathf.Cos(_angle * Mathf.Deg2Rad);

        return _position;
    }

}
