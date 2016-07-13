using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

    public bool grabbable = true;
    public bool thrown = false;
    public float maxAirTime = 30;
    public float throwSpeed = 10;
    private float currentAirTime = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (thrown == true)
        {
            currentAirTime += Time.deltaTime;

            if (currentAirTime >= maxAirTime)
            {
                GetComponent<Rigidbody>().useGravity = true;
                currentAirTime = 0;
                thrown = false;
            }
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Untagged")
        {
            GetComponent<Rigidbody>().useGravity = true;
            grabbable = true;
            thrown = false;
            currentAirTime = 0;
            tag = "Pickup";
        }
    }
}
