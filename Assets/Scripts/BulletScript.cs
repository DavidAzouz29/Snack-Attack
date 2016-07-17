using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public float speed;
    private float time = 2;


    void Update()
    {
		transform.position += transform.forward * speed * Time.deltaTime;
        Destroy(gameObject, time);
    }
}
