using UnityEngine;
using System.Collections;

public class EnemyBulletScipt : MonoBehaviour {

    public float speed;
    private float time = 2;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    void Update()
    {
        Destroy(gameObject, time);
    }
}
