using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BossBlobs))]
public class EditorBoss : Editor {

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorList.Show(serializedObject.FindProperty("m_Thresholds"), true, true);
        EditorList.Show(serializedObject.FindProperty("m_BlobsToDrop"), true, true);
        EditorList.Show(serializedObject.FindProperty("m_PowerToGive"), true, true);
        EditorList.Show(serializedObject.FindProperty("m_ScaleLevel"), true, true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_BlobObject"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Power"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_PowerMax"));
        serializedObject.ApplyModifiedProperties();
    }

}
