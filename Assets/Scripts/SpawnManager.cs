using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

    public GameObject m_PlayerSpawnPrefab, m_BlobSpawnPrefab;

    public List<GameObject> m_PlayerSpawns, m_BlobSpawns;

    public float m_PlayerRespawnTime = 2.0f;
    private float m_ResetTime;

    void Awake()
    {
        m_ResetTime = m_PlayerRespawnTime;
    }

    public void CreatePlayerSpawn()
    {
        GameObject _pSpawn = Instantiate(m_PlayerSpawnPrefab);
        _pSpawn.transform.SetParent(transform);
        m_PlayerSpawns.Add(_pSpawn);
    }

    public void CreateBlobSpawn()
    {
        GameObject _bSpawn = Instantiate(m_BlobSpawnPrefab);
        _bSpawn.transform.SetParent(transform);
        m_BlobSpawns.Add(_bSpawn);
    }

    public void UpdateList() // This is called when either a player spawn or blob spawn is deleted, it then cleans up the list.
    {
        for (int i = 0; i < m_PlayerSpawns.Count; i++)
        {
            if (m_PlayerSpawns[i] == null)
            {
                m_PlayerSpawns.RemoveAt(i);
                m_PlayerSpawns.TrimExcess();
            }
        }

        for (int i = 0; i < m_BlobSpawns.Count; i++)
        {
            if (m_BlobSpawns[i] == null)
            {
                m_BlobSpawns.RemoveAt(i);
                m_BlobSpawns.TrimExcess();
            }
        }
    }

    public bool RespawnWait()
    {
        m_PlayerRespawnTime -= Time.deltaTime;
        if (m_PlayerRespawnTime <= 0)
        {
            m_PlayerRespawnTime = m_ResetTime;
            return true;
        }
        else return false;
    }
}
