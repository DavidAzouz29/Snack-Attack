using UnityEngine;
using System.Collections;

public class BlobManager : MonoBehaviour {

    public int m_PowerToGive = 0;

    void OnCollisionEnter(Collision _col)
    {
        if (_col.gameObject.layer == 9) // Check if colliding with the player layer
        {
            if (_col.gameObject.tag != "Boss")
            {
                _col.gameObject.GetComponent<PlayerController>().health += m_PowerToGive; // Give the player power for picking up the blob

                _col.transform.localScale = new Vector3(_col.transform.localScale.x + (m_PowerToGive * .01f),
                                                        _col.transform.localScale.x + (m_PowerToGive * .01f),
                                                        _col.transform.localScale.x + (m_PowerToGive * .01f));

                Destroy(gameObject); // Maybe play a cool animation here
            }
        }
    }
}
