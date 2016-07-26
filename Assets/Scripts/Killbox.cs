using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Killbox : MonoBehaviour {

    public int m_BlobPower = 10;

    public GameObject m_SpawnManagerObject;
    private SpawnManager m_SpawnManager;
    public List<GameObject> m_PlayerSpawnList, m_BlobSpawnList;

    private Vector3 m_BlobPos;
    
    private bool m_Respawning;
    private GameObject m_Player;

    void Start()
    {
        m_SpawnManager = m_SpawnManagerObject.GetComponent<SpawnManager>();
        m_PlayerSpawnList = m_SpawnManager.m_PlayerSpawns;
        m_BlobSpawnList = m_SpawnManager.m_BlobSpawns;
    }

    void OnTriggerEnter(Collider _col)
    {
        if(_col.gameObject.tag == "Player")
        {
            m_Player = _col.gameObject;
            // Kill and respawn the player
            m_Player.transform.position = m_PlayerSpawnList[Random.Range(0, m_PlayerSpawnList.Count)].transform.position;

            int _pow = m_Player.GetComponent<BossBlobs>().m_Power;
            // Get player power here, spawn blobs they would have lost.
            if (_pow >= 150)
            {
                float _toDrop = _pow / 20;
                int _drop = Mathf.RoundToInt(_toDrop);

                Debug.Log(_drop);

                for (int i = 0; i < _drop; i++)
                {
                    int a = i * (360 / _drop);
                    GameObject _blob = (GameObject)Instantiate(m_Player.GetComponent<BossBlobs>().m_BlobObject, BlobSpawn(a), Quaternion.identity);
                    _blob.GetComponent<BlobManager>().m_PowerToGive = m_BlobPower;
                }
                ExplodeBlobs();
            }

            m_Respawning = true;

        }
        else if(_col.gameObject.tag == "Blob")
        {
            // Respawn the blob
            _col.transform.position = m_BlobSpawnList[Random.Range(0, m_BlobSpawnList.Count)].transform.position;
        }
    }

    void Update()
    {
        if (m_Respawning)
        {
            m_Respawning = false;
            StartCoroutine(IRespawn(m_Player));
            m_Player = null;
        }
    }

    Vector3 BlobSpawn(int _a)
    {
        if(m_BlobPos == null || m_BlobPos == Vector3.zero)
            m_BlobPos = m_BlobSpawnList[Random.Range(0, m_BlobSpawnList.Count)].transform.position;

        float _radius = 3.0f;

        float _angle = _a + Random.Range(5.0f, 40.0f);
        Vector3 _position;

        _position.x = m_BlobPos.x + _radius * Mathf.Sin(_angle * Mathf.Deg2Rad);
        _position.y = m_BlobPos.y;
        _position.z = m_BlobPos.z + _radius * Mathf.Cos(_angle * Mathf.Deg2Rad);

        return _position;
    }

    void ExplodeBlobs()
    {
        float _radius = 3.0f;
        Vector3 _explosionPos = m_BlobPos;
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
        m_BlobPos = Vector3.zero;
    }

    public void PlayerRespawn(GameObject _player)
    {
        StartCoroutine(IRespawn(_player));
    }

    public IEnumerator IRespawn(GameObject _player)
    {
        _player.SetActive(false);

        yield return new WaitForSeconds(m_SpawnManager.m_PlayerRespawnTime);
        _player.SetActive(true);
        _player.GetComponent<BossBlobs>().Respawn();
        m_Respawning = false;
    }
}
