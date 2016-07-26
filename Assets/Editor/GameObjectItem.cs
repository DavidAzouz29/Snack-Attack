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
/// - edits made to add various players - David Azouz 26/07/2016
/// 
/// TODO:
/// 
/// </summary>

using UnityEngine;
using UnityEditor;

public class GameObjectItem : Editor
{
    [MenuItem("Player Game Object/Add Player RockyRoad")]
    public static void AddPlayerRockyRoadToScene()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/RockyRoad.prefab", typeof(GameObject));
        GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        // you may now modify the game object
        obj.transform.position = Vector3.one;
		Selection.activeGameObject = obj;
    }
    [MenuItem("Player Game Object/Add Player BroccoLION")]
    public static void AddPlayerBroccoLIONToScene()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/BroccoLION.prefab", typeof(GameObject));
        GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        // you may now modify the game object
        obj.transform.position = Vector3.one;
        Selection.activeGameObject = obj;
    }
    [MenuItem("Player Game Object/Add Player Watermelomon")]
    public static void AddPlayerWatermelomonToScene()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Watermelomon.prefab", typeof(GameObject));
        GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        // you may now modify the game object
        obj.transform.position = Vector3.one;
        Selection.activeGameObject = obj;
    }
    [MenuItem("Player Game Object/Add Player KataTea")]
    public static void AddPlayerKataTeaToScene()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/KataTea.prefab", typeof(GameObject));
        GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        // you may now modify the game object
        obj.transform.position = Vector3.one;
        Selection.activeGameObject = obj;
    }
    [MenuItem("Player Game Object/Add Bolt")]
    public static void AddBoltToScene()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bolt.prefab", typeof(GameObject));
        GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        // you may now modify the game object
        obj.transform.position = Vector3.one;
		Selection.activeGameObject = obj;
    }
}
