using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public float speed;
    private float time = 5;
    [HideInInspector]
    public string m_playerTag = "NoPlayerAttached";

    public GameObject m_Parent;

    void Update()
    {
		transform.position += transform.forward * speed * Time.deltaTime;
        Destroy(gameObject, time);
    }

}
