using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ObjectChecker))]
public class EditorObjectChecker : Editor {

    SpawnManager m_SpawnManager;

    void OnEnable()
    {
        m_SpawnManager = ((MonoBehaviour)target).GetComponentInParent<SpawnManager>();
    }

    public void OnDestroy()
    {
        if(Application.isEditor)
        {
            if(((ObjectChecker)target) == null)
            {
                m_SpawnManager.UpdateList();
            }
        }
    }
}
