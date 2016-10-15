using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class AnyKeyLevelLoader : MonoBehaviour {

	public int levelToLoad;
	public float timeToLoadLevel = 5;

	void Start()
	{
		Invoke("LoadLevel", timeToLoadLevel);
	}

	void Update () {
	
		if( Input.anyKey )
		{
			LoadLevel();
		}
	}

	void LoadLevel()
	{
		CancelInvoke("LoadLevel");
        GC.Collect();
        Resources.UnloadUnusedAssets();
		SceneManager.LoadScene(levelToLoad);
	}
}
