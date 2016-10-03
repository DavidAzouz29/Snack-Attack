using System;
using UnityEngine;


[Serializable]
public class SnackManager
{
    // This class is to manage various settings on a Snack.
    // It works with the GameManager class to control how the Snacks behave
    // and whether or not players have control of their Snack in the 
    // different phases of the game.

    public Color m_PlayerColor;                             // This is the color this Snack will be tinted.
	public SnackBrain m_Brain;								// The brain the Snack will use
    [HideInInspector] public int m_PlayerNumber;            // This specifies which player this the manager for.
    [HideInInspector] public string m_ColoredPlayerText;    // A string that represents the player with their number colored to match their Snack.
    [HideInInspector] public GameObject m_Instance;         // A reference to the instance of the Snack when it is created.
    [HideInInspector] public int m_Wins;                    // The number of wins this player has so far.


	private SnackThinker m_Thinker;
    private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.


    public void Setup ()
    {
        // Get references to the components.
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas> ().gameObject;

		m_Thinker = m_Instance.GetComponent<SnackThinker>();
		m_Thinker.brain = m_Brain;

        // Create a string using the correct color that says 'PLAYER 1' etc based on the Snack's color and the player's number.
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        // Get all of the renderers of the Snack.
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer> ();

        // Go through all the renderers...
        for (int i = 0; i < renderers.Length; i++)
        {
            // ... set their material color to the color specific to this Snack.
            renderers[i].material.color = m_PlayerColor;
        }
    }


    // Used during the phases of the game where the player shouldn't be able to control their Snack.
    public void DisableControl ()
    {
		m_Thinker.enabled = false;

        m_CanvasGameObject.SetActive (false);
    }


    // Used during the phases of the game where the player should be able to control their Snack.
    public void EnableControl ()
    {
		m_Thinker.enabled = true;

        m_CanvasGameObject.SetActive (true);
    }


    // Used at the start of each round to put the Snack into it's default state.
    public void Reset ()
    {
        m_Instance.SetActive (false);
        m_Instance.SetActive (true);
    }
}
