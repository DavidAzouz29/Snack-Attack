using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

[CustomEditor(typeof(SpawnManager))]
public class EditorSpawns : Editor
{
    private static SpawnManager m_SpawnManager;

    private static GUIContent
        m_PlayerSpawnCreate = new GUIContent("Create Player Spawn", "Use this to create a new spawn for players."),
        m_BlobSpawnCreate = new GUIContent("Create Blob Spawn", "Use this to create a new spawn for blobs."); // Creating buttons to use for designers.

    void OnEnable()
    {
        m_SpawnManager = ((MonoBehaviour)target).gameObject.GetComponent<SpawnManager>(); // Get the spawnmanager script to access functions.
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //m_SpawnManager.m_PlayerSpawns.Clear();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_PlayerRespawnTime"));

        GUILayout.Space(20);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_PlayerSpawnPrefab"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_BlobSpawnPrefab"));

        GUILayout.Space(20);

        ShowElements(EditorListOption.BUTTONS);

        GUILayout.Space(20);

        EditorList.Show(serializedObject.FindProperty("m_PlayerSpawns"), true, false);
        EditorList.Show(serializedObject.FindProperty("m_BlobSpawns"), true, false);
        
        serializedObject.ApplyModifiedProperties();
    }

    private static void ShowElements(EditorListOption _options)
    {
        bool showButtons = (_options & EditorListOption.BUTTONS) != 0;
        if (showButtons)
            ShowButtons();
    }

    private static void ShowButtons()
    {
        if(GUILayout.Button(m_PlayerSpawnCreate))
        {
            // Create new player prefab, add it to the list.
            m_SpawnManager.CreatePlayerSpawn();
        }
        GUILayout.Space(10);

        if (GUILayout.Button(m_BlobSpawnCreate))
        {
            m_SpawnManager.CreateBlobSpawn();
        }
    }
}

[Flags]
public enum EditorListOption
{
    NONE = 0,
    BUTTONS = 1,
}