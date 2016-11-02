/// ----------------------------------
/// <summary>
/// Name: PlayerSelect.cs
/// Author: David Azouz
/// Date Created: 28/08/2016
/// Date Modified: 8/2016
/// ----------------------------------
/// Brief: Player Select class that selects players with individual controllers
/// viewed: http://wiki.unity3d.com/index.php?title=Xbox360Controller
/// http://docs.unity3d.com/ScriptReference/MonoBehaviour.StartCoroutine.html
/// http://itween.pixelplacement.com/documentation.php
/// http://blog.pastelstudios.com/2015/09/07/unity-tips-tricks-multiple-event-systems-single-scene-unity-5-1/
/// 2D Arrays on Inspector https://youtu.be/uoHc-Lz9Lsc 
/// *Edit*
/// - Player Select almost happening with individual controllers - David Azouz 28/08/2016
/// - Cleaned up and unified Player Selection - David Azouz 15/10/2016
/// 
/// TODO:
/// - cache a few of the "GetComponent"'s - 
/// -  - /8/2016
/// - 
/// </summary>
/// ----------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
//using System.Linq;

public class PlayerSelect : MonoBehaviour
{
	// Per Player Navigation
	// [Players][Confirm, left, Right]
	public ArrayLayout playerButtons;
	public float fSensitivity = 2.0f;
	public float fWaitForNextInteractable = 0.5f;
	public Button c_BackButton;
	public Button c_LevelSelect;

    private bool hasAllPlayersSelected = false;

    delegate void OnLevelFinishedLoading();
    OnLevelFinishedLoading m_OnLevelFinishedLoading;

    // Use this for initialization
    void Start () //TODO: Awake?
	{
        OnLevelWasLoaded();
    }

    void OnLevelWasLoaded()
    {
        if (gameObject.activeSelf)
        {
            for (int i = 0; i <= PlayerManager.MAX_PLAYERS - 1; ++i)
            {
                GameSettings.Instance.players[i].isReady = false;
                // turn images off //TODO: on back button, *breaks*

                Text text = GameObject.Find(("P" + (i + 1) + "ReadyImage")).GetComponent<Text>();
                text.enabled = true;
                text.enabled = false;
            }
            c_LevelSelect.interactable = false;
        }
        hasAllPlayersSelected = false;
    }

    /*//Coroutine, which gets Started in "Start()" and runs over the whole game to check for player input
	IEnumerator PlayerInput(int i)
	{
		//Loop. Otherwise we wouldnt check continoulsy
		while (true) 
		{
			// Left Bumper //Input.GetAxis ("P1_Horizontal") < fSensitivity
			if (Input.GetKeyDown ((string)"joystick " + (i + 1) + " button 4")) 
			{
				// Cycle left in character list
				playerButtons.playerColsButton [i].coloumn [1].Select ();
				playerButtons.playerRows [i].row [i].Select ();
			}

			// Right Bumper
			if (Input.GetKeyDown ((string)"joystick " + (i + 1) + " button 5")) 
			{
				// Cycle right in character list
				playerButtons.playerColsButton [i].coloumn [2].Select ();
				playerButtons.playerRows [i].row [i].Select ();
			}

			// Select (A on Xbox)
			if (Input.GetKeyDown ((string)"joystick " + (i + 1) + " button 0")) 
			{
				
			}

			yield return null;
		}
	} */

    // Update is called once per frame
    void Update () 
	{
        /*// Player 1
		// Left Bumper
		if (Input.GetButtonDown((string)"joystick 1 button 4")) //Input.GetAxis ("P1_Horizontal") < fSensitivity
		{
			playerButtons.playerRows [0].row[0].Select();
			playerButtons.playerColsButton [0].coloumn[0].Select();
			//Navigation.defaultNavigation.selectOnLeft = c_MenuButtons [0] [0]; //.navigation.selectOnLeft;
		}*/

        // if all characters haven't confimed their selection
        if (!hasAllPlayersSelected)
        {
            // Check if all players are ready to go
            //for (int i = 0; i < PlayerManager.MAX_PLAYERS; i++)
            //{
            // if all players are not ready
            //if (!GameSettings.Instance.players[i].isReady)
            // if player 1 is ready 
            if (GameSettings.Instance.players[0].isReady)
            {
                StartCoroutine(PlayerReadyWait());
            }
            //}
            //hasAllPlayersSelected = true;

        }
        // All players are ready
        else
        {
            //StopAllCoroutines(); //TODO: does this break things?
            LevelSelect(true);

        }
	}

    // Wait for a few seconds before allowing to continue to the next section.
    IEnumerator PlayerReadyWait()
    {
        yield return new WaitForSeconds(fWaitForNextInteractable);
        c_LevelSelect.interactable = true;

        yield return null;
    }

    /*void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        //SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        //SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    UnityEngine.Event OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level Loaded");
        Debug.Log(scene.name);
        Debug.Log(mode);
    }*/

    void LevelSelect(bool a_isActive)
	{
        c_LevelSelect.interactable = a_isActive;// !c_LevelSelect.interactable;
		c_LevelSelect.Select();
		// confirm (click)
	}

    public void PlayerSelectPanel(bool isActive)
    {
        // toggle the PS panel on/ off
        gameObject.SetActive(!gameObject.activeSelf);
        // Reset characters
        OnLevelWasLoaded();

    }

}
