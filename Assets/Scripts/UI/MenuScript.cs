/// <summary>
/// Author: 		David Azouz
/// Date Created: 	11/04/16
/// Date Modified: 	27/08/16
/// --------------------------------------------------
/// Brief: A Menu Script class that interprets data to present on the U.I.
/// viewed https://www.youtube.com/watch?v=y3OZXMxsrUI
/// http://answers.unity3d.com/questions/1109497/unity-53-how-to-load-current-level.html
/// 
/// ***EDIT***
/// - Menu and related systems working 	- David Azouz 11/04/16
/// - Remaining comments added 		- David Azouz 11/04/16
/// - Credits added					- David Azouz 12/04/16
/// - Player and Level Select added	- David Azouz 9/08/16
/// - Per Player U.I. Navigation (Player Select) - David Azouz 27/08/16
/// - 
/// TODO:
/// - get rid of isPlayerSelect etc
/// </summary>

using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuScript : MonoBehaviour
{
    //----------------------------------
    // PUBLIC VARIABLES //TODO: make Panel?
    //----------------------------------
    public GameObject c_titleScreenPanel;//Store a reference to the Game Object Title Screen
    public GameObject playerSelectPanel;//Store a reference to the Game Object Player Select
    public GameObject levelSelectPanel; //Store a reference to the Game Object Level Select
    public GameObject controlsPanel;    //Store a reference to the Game Object ControlsPanel
    public GameObject creditsPanel;     //Store a reference to the Game Object ControlsPanel

	//----------------------------------
    // PRIVATE VARIABLES //TODO: delete?
    //----------------------------------
    private bool isPlayerSelect = false;
    private bool isLevelSelect = false;
    private bool isControls = false;
    private bool isCredits = false;

    private int iLevelSelection = 0;
    private int iTimeSelection = 0;

    // Use this for initialization
    /*void Start ()
    {
		
	} */

    // Update is called once per frame
    void Update ()
    {
        // If 'C' is pressed...
        if(Input.GetKeyUp(KeyCode.C))
        {
            //... show/ hide the controls
            ToggleControlsView();
            //Debug.Log("isCon: " + isControls);
        }
	}

    #region Panels
    /// <summary>
    /// Shows/ hides the 'Title Screen' panel 
    /// Includes title and menu buttons
    /// </summary>
    public void ToggleTitleScreen()
    {
        // set the Title Screen panel on/ off
        c_titleScreenPanel.SetActive(!c_titleScreenPanel.activeSelf);
    }

    /// <summary>
    /// Shows/ hides the 'Player Select' panel 
    /// </summary>
    public void TogglePlayerSelect()
    {
        // toggle Player Select true/ false
        //isPlayerSelect = !isPlayerSelect;
        // set the Player Select panel on/ off
		playerSelectPanel.SetActive(!playerSelectPanel.activeSelf); //isPlayerSelect
    }

    /// <summary>
    /// Shows/ hides the 'Level Select' panel
    /// Should be enabled after the Player Select
    /// stage is complete.
    /// </summary>
    public void ToggleLevelSelect()
    {
        // toggle Level Select true/ false
        //isLevelSelect = !isLevelSelect;
        // set the Level Select panel on/ off
		levelSelectPanel.SetActive(!levelSelectPanel.activeSelf);
    }

    ///-----------------------------------
    /// <summary> 
    /// Shows/ hides the 'controls' panel
    /// </summary>
    ///-----------------------------------
    public void ToggleControlsView()
    {
        // toggle controls true/ false
        //isControls = !isControls;
        // set the controls panel on/ off
		controlsPanel.SetActive(!controlsPanel.activeSelf);
    }

    ///-----------------------------------
    /// <summary> 
    /// Shows/ hides the 'credits' panel
    /// </summary>
    ///-----------------------------------
    public void ToggleCreditsView()
    {
        // toggle credits true/ false
        isCredits = !isCredits;
        // set the credits panel on/ off
		creditsPanel.SetActive(!creditsPanel.activeSelf);
    }
    #endregion

    // Sets what level to play
    public void SetLevelSelection(int a_selection)
    {
        iLevelSelection = a_selection;
    }
    // Load the level to play the game
    public void LoadLevel()
    {
        SceneManager.LoadScene(iLevelSelection);
    }

    // Selects time duration for a round
    public void TimerSelection(int a_time)
    {
        switch (a_time)
        {
            // One minute
            case 0:
                {
                    iTimeSelection = 60;
                    break;
                }
            // Three Minutes
            case 1:
                {
                    iTimeSelection = 180; Debug.Log("MS.cs Time Sel: " + iTimeSelection);
                    break;
                }
            // Five Minutes
            case 2:
                {
                    iTimeSelection = 350;
                    break;
                }
            default:
                {
                    iTimeSelection = 60;
                    break;
                }
        }
    }

    // If the player wants to play again, reload the currently loaded level.
    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Quits the game
    public void Quit()
    {
        //If we are running in a standalone build of the game
#if UNITY_STANDALONE
        //Quit the application
        Application.Quit();
#endif
        //If we are running in the editor
#if UNITY_EDITOR
        //Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
