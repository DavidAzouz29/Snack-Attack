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
    static void AddPrebab(string a_prefab)
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + a_prefab + ".prefab", typeof(GameObject));
        GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        // you may now modify the game object
        obj.transform.position = Vector3.one;
        Selection.activeGameObject = obj;
    }

    [MenuItem("Player Game Object/Add Player RockyRoad")]
    public static void AddPlayerRockyRoadToScene()
    {
        string player = "RockyRoad";
        AddPrebab(player);
    }
    [MenuItem("Player Game Object/Add Player PrincessCake")]
    public static void AddPlayerPrincessCakeToScene()
    {
        string player = "PrincessCake";
        AddPrebab(player);
    }

    // Weapons/ PlayerCollision
    [MenuItem("Player Game Object/Select Weapons on Players")]
    public static void SelectWeaponsOnPlayers()
    {
        PlayerCollision[] weapons = FindObjectsOfType<PlayerCollision>();
        // If our players are turned off.
        if (weapons.Length == 0)
        {
            Debug.Log("You must turn on each <b>Player</b> first.");
        }
        else
        {
            Debug.Log("Don't forget to <b>Apply</b> each Player prefab when complete.");
        }
        Selection.objects = weapons;
    }
    [MenuItem("Player Game Object/Open Weapons on Players")]
    public static void OpenWeaponsOnPlayers()
    {
        PlayerCollision[] weapons = FindObjectsOfType<PlayerCollision>();
        foreach (PlayerCollision weapon in weapons)
        {
            Selection.activeGameObject = weapon.gameObject;
        }
    }
}
