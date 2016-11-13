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
/// http://answers.unity3d.com/questions/820599/simulate-button-presses-through-code-unity-46-gui.html
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
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
//using System.Linq;

public class PlayerSelect : MonoBehaviour
{
	// Per Player Navigation
	// [Players][Confirm, left, Right]
	//public ArrayLayout playerButtons;
	public float fSensitivity = 2.0f;
	public float fWaitForNextInteractable = 0.5f;
	public Button c_BackButton;
	public Button c_LevelSelect;

    private bool hasAllPlayersSelected = false;
    bool hasBeenSelected = false;

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

    // Update is called once per frame
    void Update () 
	{
        // if all characters haven't confimed their selection
        if (!hasAllPlayersSelected)
        {
            //RECODE: allow for AI players
            // if player 1 is ready 
            if (!hasBeenSelected)
            {
                if (GameSettings.Instance.players[0].isReady)
                {
                    StartCoroutine(PlayerReadyWait());
                }
            }

            // Checks if all players are ready...
            if (GameSettings.Instance.players.TrueForAll(player => player.isReady == true))
            {
                // proceed to LS screen
                hasAllPlayersSelected = true;
                LevelSelect(true);
            }
            // ...else/ if a player decides they're not ready
            else
            {
                hasAllPlayersSelected = false;
                //LevelSelect(false);
            }
        }
	}

    // Wait for a few seconds before allowing to continue to the next section.
    IEnumerator PlayerReadyWait()
    {
        yield return new WaitForSeconds(fWaitForNextInteractable);
        c_LevelSelect.interactable = true;
        if (!hasBeenSelected)
        {
            c_BackButton.Select(); // used to restart Level Select Anim
            c_LevelSelect.Select();
            hasBeenSelected = true;
        }
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
        c_LevelSelect.interactable = a_isActive;
        if (a_isActive)
        {
            c_LevelSelect.Select();
            // confirm (click)
            ExecuteEvents.submitHandler(c_LevelSelect, new BaseEventData(FindObjectOfType<EventSystem>())); //TODO: EventSystem.current
        }
	}

    public void PlayerSelectPanel(bool isActive)
    {
        // toggle the PS panel on/ off
        gameObject.SetActive(isActive);
        // Choose/ Pick your Character/ Player
        if(isActive)
        {
            GameManager.Instance.PlayAudioClip(2, 1);
        }
        // Reset characters
        OnLevelWasLoaded();

    }

}
