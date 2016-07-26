using UnityEngine;
using System.Collections;

public class BlobManager : MonoBehaviour {

    public int m_PowerToGive = 0; // Based off scale percentage. Giant Thresh = 2.0f scale, which is 200 total power.

    private BossBlobs m_BossBlobs;

    void OnCollisionEnter(Collision _col)
    {
        if (_col.gameObject.tag == "Player")
        {
            m_BossBlobs = _col.gameObject.GetComponent<BossBlobs>();

            if(m_BossBlobs.m_Power <= 199)
            {
                m_BossBlobs.m_Power += m_PowerToGive;
                if (_col.gameObject.GetComponent<BossBlobs>().m_Power > 200)
                    _col.gameObject.GetComponent<BossBlobs>().m_Power = 200;
                _col.gameObject.GetComponent<BossBlobs>().m_Updated = true;

                Destroy(gameObject); // Maybe play a cool animation here
            }

        }
    }
}
