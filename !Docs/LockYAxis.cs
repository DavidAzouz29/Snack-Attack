// https://docs.unity3d.com/ScriptReference/ExecuteInEditMode.html

using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class LockYAxis : Editor
{
    public Transform target;

    // Update is only called when something in the scene changed.
    void Update ()
    {
        if (Selection.activeTransform.tag == "Player")
        {
            Selection.activeTransform.position = new Vector3(target.position.x, 0.0f, target.position.z);
        }
    }
}
