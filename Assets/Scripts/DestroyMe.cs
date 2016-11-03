using UnityEngine;
using System.Collections;

public class DestroyMe : MonoBehaviour
{
    public float fDestroyAfterTime = 0.4f;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, fDestroyAfterTime);
    }
}
