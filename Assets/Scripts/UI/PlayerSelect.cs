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
/// -  - David Azouz 28/08/2016
/// 
/// TODO:
/// -  - /8/2016
/// - 
/// </summary>
/// ----------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerSelect : MonoBehaviour
{
	// Per Player Navigation
	// [Players][Confirm, left, Right]
	public ArrayLayout playerButtons;
	public float fSensitivity = 2.0f;
	public Button c_LevelSelect;

	private const int MAX_CLASS_COUNT = 2; //PlayerController.E_CLASS_STATE.E_PLAYER_STATE_COUNT
	public MeshRenderer[,] c_Classes = new MeshRenderer[PlayerManager.MAX_PLAYERS, MAX_CLASS_COUNT];
	// For which player
	[SerializeField] private int[] iCurrentClassSelection = new int[PlayerManager.MAX_PLAYERS];
	// used for when players have confirmed their character selection
	[SerializeField] bool[] playersConfirmed = new bool[PlayerManager.MAX_PLAYERS];

	/*//OnLevelWasLoaded is called after a new scene has finished loading
	void OnLevelWasLoaded ()
	{
		//If there is no EventSystem (needed for UI interactivity) present
		if(!FindObjectOfType<EventSystem>())
		{
			GameObject obj = new GameObject("EventSystem");
			//And adds the required components
			obj.AddComponent<EventSystem>();
		}
	} */

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < PlayerManager.MAX_PLAYERS; i++) 
		{
			playersConfirmed [i] = false;
			// Start 1-4 Coroutines to check for player input - each on their own "thread*"
			StartCoroutine (PlayerInput (i));
		}		
	}

	//Coroutine, which gets Started in "Start()" and runs over the whole game to check for player input
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
	}

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
			if (!playersConfirmed [i] == true) 
			{
				break;
			} 
			// All players are ready
			else 
			{
				LevelSelect ();
			}
		}	
	}

	// '0' based
	public void PlayerReady(int i)
	{
		playersConfirmed [i] = true;
		//TODO: "Ready" animation?
	}

	// '0' based
	public void PlayerSelectLeft(int a_player)
	{
		// Move one left
		iCurrentClassSelection[a_player] -= 1;
		if (iCurrentClassSelection[a_player] < 0) 
		{
			iCurrentClassSelection[a_player] = MAX_CLASS_COUNT;
		}
		c_Classes [a_player, iCurrentClassSelection[a_player]].gameObject.SetActive(true);
	}

	// '0' based
	public void PlayerSelectRight(int a_player)
	{
		// Move one right
		iCurrentClassSelection[a_player] += 1;
		if (iCurrentClassSelection[a_player] > MAX_CLASS_COUNT) 
		{
			iCurrentClassSelection[a_player] = 0;
		}
		c_Classes [a_player, iCurrentClassSelection[a_player]].gameObject.SetActive(true);
	}

	void LevelSelect()
	{
		c_LevelSelect.Select ();
		// confirm (click)
	}
}
