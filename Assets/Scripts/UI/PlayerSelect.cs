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
/// - cache a few of the "GetComponent"'s - 
/// -  - /8/2016
/// - 
/// </summary>
/// ----------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class PlayerSelect : MonoBehaviour
{
	// Per Player Navigation
	// [Players][Confirm, left, Right]
	public ArrayLayout playerButtons;
	public float fSensitivity = 2.0f;
	public Button c_LevelSelect;

	private const int MAX_CLASS_COUNT = (int)PlayerBuild.E_BASE_CLASS_STATE.E_BASE_CLASS_STATE_TOTAL_COUNT;
    // ------------------------------------
    // Used to hold the different characters we can play as.
    // Mesh, MeshRenderer, and Animation
    // ------------------
    public GameObject[] c_Characters = new GameObject[MAX_CLASS_COUNT];
    public GameObject c_eventSystems;

    // For which player
    [SerializeField] private int[] iCurrentClassSelection = new int[PlayerManager.MAX_PLAYERS];
	// used for when players have confirmed their character selection
	[SerializeField] bool[] playersConfirmed = new bool[PlayerManager.MAX_PLAYERS];

	// Use this for initialization
	void Start () 
	{
        for (int i = 0; i <= PlayerManager.MAX_PLAYERS - 1; ++i)
        {
            playersConfirmed[i] = false;
            CharacterSelection(i);
            // Start 1-4 Coroutines to check for player input - each on their own "thread*"
            StartCoroutine(PlayerInput(i));
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
                StopAllCoroutines(); //TODO: does this break things?
                LevelSelect();
			}
		}	
	}

	// '0' based
	public void PlayerReady(int i)
	{
		playersConfirmed [i] = !playersConfirmed[i];
		//TODO: "Ready" animation?
	}

	// '0' based
	public void PlayerSelectLeft(int a_player)
	{
        // Move one left
        iCurrentClassSelection[a_player] -= 1;
		if (iCurrentClassSelection[a_player] < 0) 
		{
            iCurrentClassSelection[a_player] = MAX_CLASS_COUNT - 1; Debug.Log("Left");
        }
        //
        CharacterSelection(a_player);
    }

	// '0' based
	public void PlayerSelectRight(int a_player)
	{
        // Move one right
        iCurrentClassSelection[a_player] += 1;
        if (iCurrentClassSelection[a_player] >= MAX_CLASS_COUNT)
        {
            iCurrentClassSelection[a_player] = 0; Debug.Log("Right");
        }
        //
        CharacterSelection(a_player);
    }

	void LevelSelect()
	{
		c_LevelSelect.Select ();
		// confirm (click)
	}

    void CharacterSelection(int a_player)
    {
        //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(playerButtons.playerColsButton[a_player].coloumn[0].gameObject);
        // 
        GameObject c_currChar = c_Characters[iCurrentClassSelection[a_player]];
        GameObject characterSlot = null;
        // Rocky Road
        if (iCurrentClassSelection[a_player] < (int)PlayerBuild.E_ROCKYROAD_STATE.E_ROCKYROAD_BASE_ROCKYROAD_COUNT)
        {
            if(!transform.GetChild(0).GetChild(a_player).GetChild(1).gameObject.activeInHierarchy)
            {
                transform.GetChild(0).GetChild(a_player).GetChild(1).gameObject.SetActive(true);
            }
            characterSlot = transform.GetChild(0).GetChild(a_player).GetChild(1).gameObject;
            transform.GetChild(0).GetChild(a_player).GetChild(2).gameObject.SetActive(false);
        }
        // Princess Cake
        else if (//iCurrentClassSelection[a_player] >= (int)PlayerBuild.E_CLASS_STATE.E_CLASS_BASE_ROCKYROAD_COUNT &&
            iCurrentClassSelection[a_player] < (int)PlayerBuild.E_BASE_CLASS_STATE.E_BASE_CLASS_BASE_PRINCESSCAKE_COUNT)
        {
            if (!transform.GetChild(0).GetChild(a_player).GetChild(2).gameObject.activeInHierarchy)
            {
                transform.GetChild(0).GetChild(a_player).GetChild(2).gameObject.SetActive(true);
            }
            characterSlot = transform.GetChild(0).GetChild(a_player).GetChild(2).gameObject;
            transform.GetChild(0).GetChild(a_player).GetChild(1).gameObject.SetActive(false);
        }
        // Pizza Punk
        else if (//iCurrentClassSelection[a_player] >= (int)PlayerBuild.E_CLASS_STATE.E_CLASS_BASE_PRINCESSCAKE_COUNT &&
            iCurrentClassSelection[a_player] < (int)PlayerBuild.E_BASE_CLASS_STATE.E_BASE_CLASS_BASE_PIZZAPUNK_COUNT)
        {
            // TODO: get child 3 // Set Child 2 to Inactive ^ same for others
            if (!transform.GetChild(0).GetChild(a_player).GetChild(2).gameObject.activeInHierarchy)
            {
                transform.GetChild(0).GetChild(a_player).GetChild(2).gameObject.SetActive(true);
            }
            characterSlot = transform.GetChild(0).GetChild(a_player).GetChild(2).gameObject;
            transform.GetChild(0).GetChild(a_player).GetChild(1).gameObject.SetActive(false);
        }

        SkinnedMeshRenderer characterSkinnedRenderer = characterSlot.GetComponentInChildren<SkinnedMeshRenderer>();
        Animator characterAnimator = characterSlot.GetComponent<Animator>();
        characterSkinnedRenderer.sharedMaterial = c_currChar.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial;
        characterSkinnedRenderer.sharedMesh = c_currChar.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
        characterAnimator.runtimeAnimatorController = c_currChar.GetComponent<Animator>().runtimeAnimatorController;
        characterAnimator.avatar = c_currChar.GetComponent<Animator>().avatar;

        characterSlot.transform.rotation = c_currChar.transform.rotation;
        characterSlot.transform.localScale = c_currChar.transform.localScale;
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

        // Player Select Panel is turned on/ off
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
        }

        if (!isActive)
        {
            gameObject.SetActive(false);
        }

    }
}
