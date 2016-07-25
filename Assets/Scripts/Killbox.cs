using UnityEngine;
using System.Collections;

public class Killbox : MonoBehaviour {

    [Tooltip("Where to respawn the player if they fall into this killbox")]
    public Vector3 m_PlayerRespawn;

    [Tooltip("Where to respawn the boss blobs if they fall into this killbox")]
    public Vector3 m_BlobRespawn;

    public int m_BlobPower = 10;

    

    void OnTriggerEnter(Collider _col)
    {
        if(_col.gameObject.tag == "Player")
        {
            // Kill and respawn the player
            _col.transform.position = m_PlayerRespawn;
            int _pow = _col.GetComponent<BossBlobs>().m_Power;
            // Get player power here, spawn blobs they would have lost.
            if (_pow >= 150)
            {
                float _toDrop = _pow / 20;
                int _drop = Mathf.RoundToInt(_toDrop);

                Debug.Log(_drop);
                for (int i = 0; i < _drop; i++)
                {
                    int a = i * (360 / _drop);
                    GameObject _blob = (GameObject)Instantiate(_col.GetComponent<BossBlobs>().m_BlobObject, BlobSpawn(a), Quaternion.identity);
                    _blob.GetComponent<BlobManager>().m_PowerToGive = m_BlobPower;
                }
                ExplodeBlobs();
            }
            _col.GetComponent<BossBlobs>().Respawn();

        }
        else if(_col.gameObject.tag == "Blob")
        {
            // Respawn the blob
            _col.transform.position = m_BlobRespawn;
        }
    }

    Vector3 BlobSpawn(int _a)
    {
        Vector3 _center = m_BlobRespawn;
        float _radius = 3.0f;

        float _angle = _a + Random.Range(5.0f, 40.0f);
        Vector3 _position;

        _position.x = _center.x + _radius * Mathf.Sin(_angle * Mathf.Deg2Rad);
        _position.y = _center.y;
        _position.z = _center.z + _radius * Mathf.Cos(_angle * Mathf.Deg2Rad);

        return _position;
    }

    void ExplodeBlobs()
    {
        float _radius = 3.0f;
        Vector3 _explosionPos = m_BlobRespawn;
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
}
