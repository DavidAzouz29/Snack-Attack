using UnityEngine;
using UnityEditor;

public class EditorList {

    public static void Show(SerializedProperty list, bool _showListLabel = true, bool _blobs = false)
    {
        if(_showListLabel)
        {
            EditorGUILayout.PropertyField(list);
            EditorGUI.indentLevel += 1;
        }

        if (!_showListLabel || list.isExpanded && _blobs)
        {
            for (int i = 0; i < list.arraySize; i++)
            {
                switch (i)
                {
                    case 0:
                        EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i),
                            new GUIContent("Giant"));
                        break;
                    case 1:
                        EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i),
                            new GUIContent("Big"));
                        break;
                    case 2:
                        EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i),
                            new GUIContent("Regular"));
                        break;
                    case 3:
                        EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i),
                            new GUIContent("Small"));
                        break;

                    default:
                        break;
                }
            }
        }

        else if (!_blobs)
        {
            for (int i = 0; i < list.arraySize; i++)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), new GUIContent("Giant"));

            }
        }
        
        if(_showListLabel)
            EditorGUI.indentLevel -= 1;
    }

}
