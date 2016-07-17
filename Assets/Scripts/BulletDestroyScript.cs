using UnityEngine;
using System.Collections;

public class BulletDestroyScript : MonoBehaviour
{
    public float destroyTime = 2.0f;

    void OnEnable()
    {
        Invoke("Destroy", destroyTime);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }
}
