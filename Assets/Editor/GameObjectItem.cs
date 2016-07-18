///<summary>
///
/// viewed: http://forum.unity3d.com/threads/editor-script-create-game-object-from-a-prefab.47845/
/// </summary>

using UnityEngine;
using UnityEditor;
using System.Collections;

public class GameObjectItem : Editor
{
    //public GameObject prefab;
    //private string menuP = "Player";
    [MenuItem("Player Game Object/AddPlayer")]
    public static void AddPlayerToScene()
    {
        //AssetDatabase.CreateAsset(prefab, "");
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Player.prefab", typeof(GameObject));
        GameObject clone = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        // Modify the clone to your heart's content
        clone.transform.position = Vector3.one;
        //AssetDatabase.Refresh();
    }


	/*// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	} */
}
