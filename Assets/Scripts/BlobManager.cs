using UnityEngine;
using System.Collections;

public class BlobManager : MonoBehaviour {

    public int m_PowerToGive = 0; // Based off scale percentage. Giant Thresh = 2.0f scale, which is 200 total power.

    void OnCollisionEnter(Collision _col)
    {
        if (_col.gameObject.layer == 9) // Check if colliding with the player layer
        {
            if (_col.gameObject.tag != "Boss")
            {
                _col.gameObject.GetComponent<BossBlobs>().m_Power += m_PowerToGive;
                _col.gameObject.GetComponent<BossBlobs>().m_Updated = true;

                Destroy(gameObject); // Maybe play a cool animation here
            }
        }
    }
}
