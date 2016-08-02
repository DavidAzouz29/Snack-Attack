﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WindowManager : MonoBehaviour {

	public GameObject scoreBoard;

	// Use this for initialization
	void Start () {
        // turns the scoreboard off during playtime.
        scoreBoard.SetActive(true);
        scoreBoard.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.timeScale == 0)
        {
            scoreBoard.SetActive(true);
        }
		if(Input.GetKeyDown(KeyCode.Tab)) {
			scoreBoard.SetActive( !scoreBoard.activeSelf );
		}
	}

    public void Replay()
    {
        scoreBoard.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void Quit()
    {
        //SceneManager.
        Application.Quit();
    }
}
