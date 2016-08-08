using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {

    private BossBlobs m_BossBlobs;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<BossBlobs>();
        }
    }
}
