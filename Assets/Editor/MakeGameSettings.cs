using UnityEngine;
using UnityEditor;
using System.Collections;

public class MakeGameSettings : MonoBehaviour {

    [MenuItem("Assets/Create/New Game Settings")]
    public static void CreateMyAsset()
    {
        GameSettings asset = ScriptableObject.CreateInstance<GameSettings>();

        AssetDatabase.CreateAsset(asset, "Assets/New Game Settings.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
