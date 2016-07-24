///<summary>
/// Name: GameObjectItem.cs
/// Author: David Azouz
/// Date Created: 18/07/16
/// Date Modified: 18/07/16
/// --------------------------------------------------
/// Brief:
/// viewed: https://docs.unity3d.com/ScriptReference/EditorUtility.html
/// http://forum.unity3d.com/threads/editor-script-create-game-object-from-a-prefab.47845/
/// http://code.tutsplus.com/tutorials/how-to-add-your-own-tools-to-unitys-editor--active-10047
/// --------------------------------------------------
/// Edits:
/// -   - David Azouz 14/07/2016
/// - 
/// 
/// TODO:
/// 
/// </summary>

using UnityEngine;
using UnityEditor;

public class GameObjectItem : Editor
{
    [MenuItem("Player Game Object/Add Player")]
    public static void AddPlayerToScene()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Player.prefab", typeof(GameObject));
        GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        // you may now modify the game object
        obj.transform.position = Vector3.one;
		Selection.activeGameObject = obj;
    }
    [MenuItem("Player Game Object/Add Shot")]
    public static void AddShotToScene()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Shot.prefab", typeof(GameObject));
        GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        // you may now modify the game object
        obj.transform.position = Vector3.one;
		Selection.activeGameObject = obj;
    }
}
