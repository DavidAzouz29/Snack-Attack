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
/// - Clean up - David Azouz 1/10/16
/// - 
/// TODO:
/// - 
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
    private int iLevelSelection = 0;

    // Update is called once per frame
    /*void Update ()
    {
        // If 'C' is pressed...
        if(Input.GetKeyUp(KeyCode.C))
        {
            //... show/ hide the controls
            ToggleControlsView();
            //Debug.Log("isCon: " + isControls);
        }
	} */

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

    // 
    public int GetLevelSelection()
    {
        return iLevelSelection;
    }

    // Selects time duration for a round
    public void SetTimerSelection(int a_time)
    {
        switch (a_time)
        {
            // One minute
            case 0:
                {
                    GameSettings.Instance.SetRoundTimer(60);
                    break;
                }
            // Three Minutes
            case 1:
                {
                    GameSettings.Instance.SetRoundTimer(180); Debug.Log("MS.cs Time Sel: " + GameSettings.Instance.iRoundTimerChoice);
                    break;
                }
            // Five Minutes
            case 2:
                {
                    GameSettings.Instance.SetRoundTimer(350);
                    break;
                }
            default:
                {
                    GameSettings.Instance.SetRoundTimer(60);
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
