using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossBlobs : MonoBehaviour {

    /*
    Whoever is currently the boss, when going below a certain health threshold, should drop blobs around them.
    */

    static uint m_Fourth = 100, m_Third = 75, m_Second = 50, m_First = 25;

    public uint m_CurrentThreshold;

    public uint m_CurrentHealth;
    public uint m_PreviousHealth;

    public GameObject m_Blob;

    public List<GameObject> m_CreatedBlobs;


    void Start()
    {
        m_CurrentThreshold = m_Fourth;
        m_CurrentHealth = m_CurrentThreshold;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            m_CurrentHealth = m_CurrentHealth - 5;
            if (m_CurrentHealth < m_CurrentThreshold)
            {
                DropBlobs(m_CurrentThreshold);
                m_CurrentThreshold = m_CurrentThreshold - 25;

                // Everytime a player goes down a threshold, lower their scale by .25
                gameObject.transform.localScale = new Vector3(transform.localScale.x - 0.25f,  
                                                              transform.localScale.y - 0.25f,
                                                              transform.localScale.z - 0.25f); 
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
            if(m_CurrentHealth < m_CurrentThreshold)
            {
                DropBlobs(m_CurrentThreshold);
                m_CurrentThreshold = m_CurrentThreshold - 25;
            }
        }
    }

    void DropBlobs(uint _threshold)
    {
        int _blobs;
        // Should only be called if this 'boss' has gone below certain threshold.
        switch (_threshold)
        {
            case 25:
                _blobs = 4;
                for (int i = 0; i < _blobs; i++)
                {
                    int a = 1;
                    GameObject _curBlob = (GameObject)Instantiate(m_Blob, BlobSpawn(a), Quaternion.identity);
                    _curBlob.GetComponent<BlobManager>().m_PowerToGive = 20;
                    m_CreatedBlobs.Add(_curBlob);
                }
                // Apply Explosion
                ExplodeBlobs();
                m_CreatedBlobs.Clear();
                // Drop Some Blobs
                break;
            case 50:
                _blobs = 2;
                for (int i = 0; i < _blobs; i++)
                {
                    int a = i * (360 / _blobs);
                    GameObject _curBlob = (GameObject)Instantiate(m_Blob, BlobSpawn(a), Quaternion.identity);
                    _curBlob.GetComponent<BlobManager>().m_PowerToGive = 15;
                    m_CreatedBlobs.Add(_curBlob);
                }
                // Apply Explosion
                ExplodeBlobs();
                m_CreatedBlobs.Clear();
                break;
            case 75:
                _blobs = 3;
                for (int i = 0; i < _blobs; i++)
                {
                    int a = i * (360 / _blobs);
                    GameObject _curBlob = (GameObject)Instantiate(m_Blob, BlobSpawn(a), Quaternion.identity);
                    _curBlob.GetComponent<BlobManager>().m_PowerToGive = 10;
                    m_CreatedBlobs.Add(_curBlob);
                }
                // Apply Explosion
                ExplodeBlobs();
                m_CreatedBlobs.Clear();
                break;
            case 100:
                // Drop 4 blobs
                _blobs = 4;
                for (int i = 0; i < _blobs; i++)
                {
                    int a = i * (360 / _blobs);
                    GameObject _curBlob = (GameObject)Instantiate(m_Blob, BlobSpawn(a), Quaternion.identity);
                    _curBlob.GetComponent<BlobManager>().m_PowerToGive = 5;
                    m_CreatedBlobs.Add(_curBlob);
                }
                // Apply Explosion
                ExplodeBlobs();
                m_CreatedBlobs.Clear();
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
