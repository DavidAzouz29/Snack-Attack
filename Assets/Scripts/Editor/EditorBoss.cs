using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BossBlobs))]
public class EditorBoss : Editor {

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorList.Show(serializedObject.FindProperty("m_Thresholds"));
        EditorList.Show(serializedObject.FindProperty("m_BlobsToDrop"));
        EditorList.Show(serializedObject.FindProperty("m_PowerToGive"));
        EditorList.Show(serializedObject.FindProperty("m_ScaleLevel"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_BlobObject"));
        serializedObject.ApplyModifiedProperties();
    }

}
