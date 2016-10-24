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
using UnityEngine.UI;
using System.Collections;
//using System.Linq;

public class PlayerSelect : MonoBehaviour
{
	// Per Player Navigation
	// [Players][Confirm, left, Right]
	public ArrayLayout playerButtons;
	public float fSensitivity = 2.0f;
	public Button c_BackButton;
	public Button c_LevelSelect;

    public GameObject c_eventSystems;

    // For which player
	// used for when players have confirmed their character selection
	[SerializeField] bool[] playersConfirmed = new bool[PlayerManager.MAX_PLAYERS];

	// Use this for initialization
	void Start () //TODO: Awake?
	{
        for (int i = 0; i <= PlayerManager.MAX_PLAYERS - 1; ++i)
        {
            playersConfirmed[i] = false;
            // Start 1-4 Coroutines to check for player input - each on their own "thread*"
            //StartCoroutine(PlayerInput(i));
        }		
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

		// Check if all players are ready to go
		for (int i = 0; i < PlayerManager.MAX_PLAYERS; i++) 
		{
			// if all players are not ready
			if (!GameSettings.Instance.players[i].isReady) 
			{
                //c_BackButton.Select();
                break;
			} 
			// All players are ready
			else 
			{
                //StopAllCoroutines(); //TODO: does this break things?
                LevelSelect(true);
			}
		}	
	}

	// '0' based
	/*public void PlayerReady(int i)
	{
		playersConfirmed [i] = !playersConfirmed[i];
		//TODO: "Ready" animation?
	} */

	void LevelSelect(bool a_isActive)
	{
        c_LevelSelect.interactable = a_isActive;// !c_LevelSelect.interactable;
		c_LevelSelect.Select();
		// confirm (click)
	}

    public void PlayerSelectPanel(bool isActive)
    {
        // toggle the PS panel on/ off
        if (!gameObject.activeSelf && isActive)
        {
            this.gameObject.SetActive(true);
        }

        // Player Select Panel is turned off
        /*if (!isActive)
        {
            c_eventSystems.transform.GetChild(c_eventSystems.transform.childCount - 1).gameObject.SetActive(true);
            // Set the Selected GameObject in the Event System.
            // Due to "back to main menu" and "forward to level select"
        }
        else
        {
            // turn the main event system off if we're in the PS
            c_eventSystems.transform.GetChild(c_eventSystems.transform.childCount - 1).gameObject.SetActive(false);
        }*/

        /*// Player Select Panel is turned on/ off
        for (int i = 0; i < PlayerManager.MAX_PLAYERS; i++)
        {
            GameObject go = c_eventSystems.transform.GetChild(i).gameObject;
            go.SetActive(isActive);
            // if PS is turned on
            if (go.activeSelf)
            {
                go.GetComponent<MyEventSystem>().SetSelectedGameObject(
                transform.GetChild(0).GetChild(i).GetChild(1).GetChild(1).gameObject);
            }
        } */

        if (!isActive)
        {
            gameObject.SetActive(false);
        }

    }
}
