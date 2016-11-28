using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Killbox : MonoBehaviour {

    public int m_BlobPower = 10;

    public GameObject m_SpawnManagerObject;
    private SpawnManager m_SpawnManager;
    public List<GameObject> m_PlayerSpawnList, m_BlobSpawnList, m_TakenSpawnList;

    private Vector3 m_BlobPos;

    //private float m_ResetTime = 2.0f;
    private bool m_Respawning;
    private GameObject m_Player;

    void Start()
    {
        m_SpawnManager = m_SpawnManagerObject.GetComponent<SpawnManager>();
        m_PlayerSpawnList = m_SpawnManager.m_PlayerSpawns;
        m_BlobSpawnList = m_SpawnManager.m_BlobSpawns;
    }

    void OnTriggerEnter(Collider _col) // Entering the killbox will kill the player, move them to a spawn, and respawn their blobs if need be.
    {
        // Blender wiz
        if(tag == "Bench")
        {
            GameManager.Instance.transform.GetChild(GameManager.iChLKitchen).GetChild(1).GetComponent<AudioSource>().Play();
        }

        if (_col.gameObject.tag == "Player")
        {
            m_Player = _col.gameObject;
            GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreManager>().ChangeScore(m_Player.GetComponent<PlayerController>().m_PlayerTag, "deaths", 1);

            // Kill and respawn the player
            //GameObject.Find("Scoreboard").GetComponent<ScoreManager>().ChangeScore(_col.gameObject.GetComponent<PlayerController>().m_PlayerTag, "deaths", 1);

            int _pow = m_Player.GetComponent<BossBlobs>().m_Power;
            // Get player power here, spawn blobs they would have lost.
            if (_pow >= 110) //TODO: 110?
            {
                float _toDrop = _pow / 20;
                int _drop = Mathf.RoundToInt(_toDrop);

                for (int i = 0; i < _drop; i++)
                {
                    int a = i * (360 / _drop);
                    // TODO: check if this can be cleaned up (Boss Blobs)
                    BossBlobs bossBlobs = m_Player.GetComponent<BossBlobs>();
                    GameObject _blob = (GameObject)Instantiate(bossBlobs.m_SpawnableBlob, BlobSpawn(a), Quaternion.identity);
                    _blob.GetComponent<BlobCollision>().m_PowerToGive = m_BlobPower;
                    switch(m_Player.GetComponent<PlayerController>().m_eCurrentClassState)
                    {
                        case PlayerController.E_CLASS_STATE.E_CLASS_STATE_RR_ROCKYROAD:
                        case PlayerController.E_CLASS_STATE.E_CLASS_STATE_RR_MINTCHOPCHIP:
                        case PlayerController.E_CLASS_STATE.E_CLASS_STATE_RR_COOKIECRUNCH:
                        case PlayerController.E_CLASS_STATE.E_CLASS_STATE_RR_RAINBOWWARRIOR:
                            {
                                _blob = bossBlobs.GetBlobArray()[0];
                                // Blob Materials brain
                                _blob.GetComponent<MeshRenderer>().sharedMaterial = bossBlobs.c_blobMaterial;
                                _blob.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = bossBlobs.c_blobMaterial;
                                _blob.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = bossBlobs.c_blobMesh;
                                break;
                            }
                        case PlayerController.E_CLASS_STATE.E_CLASS_STATE_PC_PRINCESSCAKE:
                        case PlayerController.E_CLASS_STATE.E_CLASS_STATE_PC_DUCHESSCAKE:
                        case PlayerController.E_CLASS_STATE.E_CLASS_STATE_PC_POUNDCAKE:
                        case PlayerController.E_CLASS_STATE.E_CLASS_STATE_PC_ANGELCAKE:
                            {
                                _blob = bossBlobs.GetBlobArray()[1];
                                // Blob Materials
                                _blob.GetComponent<MeshRenderer>().sharedMaterial = bossBlobs.c_blobMaterial;
                                _blob.GetComponent<MeshFilter>().sharedMesh = bossBlobs.c_blobMesh;
                                //m_SpawnableBlob.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = c_blobMaterial;
                                break;
                            }
                        default:
                            {
                                Debug.LogError("BB: Character Blob not set up.");
                                break;
                            }
                    }
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
        if(m_BlobPos == Vector3.zero)
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
        m_SpawnManager.RespawnPlayerPosition(_player);
        _player.SetActive(false);

        yield return new WaitForSeconds(m_SpawnManager.m_PlayerRespawnTime);
        _player.SetActive(true);
        _player.GetComponent<BossBlobs>().Respawn();
        m_Respawning = false;
    }
}
