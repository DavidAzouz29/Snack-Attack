/// <summary>
/// 
/// *Edit*
/// - Cleaned up and unified Player Selection - David Azouz 15/10/2016
/// 
/// TODO:
/// - clean up (int)
/// </summary>

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;

public class PlayerInfoController : MonoBehaviour
{
	public int PlayerIndex;
	public bool isReady = false;
	public UnityEngine.UI.Button PlayerColor;
	public UnityEngine.UI.Text c_PlayerBrain;
    public float fSensitivity = 2.0f;

    //private MainMenuController _mainMenu;
    private GameSettings.PlayerInfo _player;
	private ArrayLayout playerButtonsArray;
    private int m_Controller = 0; // tracks time
    private float m_Timer = 0; // tracks time
    private float m_TimerPause = 0; // Time before next input
    public void Awake()
    {
        //_mainMenu = GetComponentInParent<MainMenuController>();
        CharacterSelection(GameSettings.Instance.players[PlayerIndex]);
        playerButtonsArray = GetComponentInParent<PlayerSelect>().playerButtons;
        m_Controller = PlayerIndex + 1;
}

    void Update()
    {
        m_Timer += Time.deltaTime;

        // Left
        if (Input.GetKeyDown("joystick " + (m_Controller) + " button 4") && m_Timer <= m_TimerPause || 
            Input.GetAxis("P" + (m_Controller) + "_Horizontal") < fSensitivity && m_Timer <= m_TimerPause)
        {
            // Cycle left in character list
            OnPreviousBrain();
            //playerButtonsArray.playerColsButton[PlayerIndex].coloumn[1].Select();
            //playerButtonsArray.playerRows[PlayerIndex].row[PlayerIndex].Select();
            m_Timer = 0;
        }

        // Right
        if (Input.GetKeyDown("joystick " + (m_Controller) + " button 5") && m_Timer <= m_TimerPause ||
            Input.GetAxis("P" + (m_Controller) + "_Horizontal") > fSensitivity && m_Timer <= m_TimerPause)
        {
            // Cycle right in character list
            OnNextBrain();
            //playerButtonsArray.playerColsButton[PlayerIndex].coloumn[2].Select();
            //playerButtonsArray.playerRows[PlayerIndex].row[PlayerIndex].Select();
            m_Timer = 0;
        }

        if (Input.GetAxis("P" + (m_Controller) + "_Vertical") < fSensitivity && m_Timer <= m_TimerPause)
        {

        }

        // Select (A on Xbox)
        if (Input.GetKeyDown("joystick " + (m_Controller) + " button 0") && m_Timer <= m_TimerPause)
        {
            // Toggle "Ready" on/ off
            isReady = !isReady;
            //playerButtonsArray.playerColsButton[PlayerIndex].coloumn[1].Select();
            //playerButtonsArray.playerRows[PlayerIndex].row[PlayerIndex].Select();
            m_Timer = 0;
        }
    }

    public void Refresh()
	{
		_player = GameSettings.Instance.players[PlayerIndex];

		var colorBlock = PlayerColor.colors;
		colorBlock.normalColor = _player.Color;
		colorBlock.highlightedColor = _player.Color;
		PlayerColor.colors = colorBlock;

		c_PlayerBrain.text = (_player.Brain != null)
			? _player.Brain.name.Substring(0, _player.Brain.name.Length - " Brain".Length)
			: "None";

    }

    /*public void OnCycleColor()
	{
		_player.Color = _mainMenu.GetNextColor(_player.Color);
		Refresh();
	} */

    public void ToggleIsReady()
    {
        isReady = !isReady;
    }

    // Left
    public void OnPreviousBrain()
    {
        //_player.Brain = _mainMenu.CycleNextSelection(_player.Brain, false);
        _player.Brain = GameSettings.Instance.CycleNextSelection(_player.Brain, false);
        CharacterSelection(_player);
        Refresh();
    }

    // Right
    public void OnNextBrain()
	{
        //_player.Brain = _mainMenu.CycleNextSelection(_player.Brain, true);
        _player.Brain = GameSettings.Instance.CycleNextSelection(_player.Brain, true);
        CharacterSelection(_player);
        Refresh();
    }

    void CharacterSelection(GameSettings.PlayerInfo a_player)
    {
        //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(playerButtons.playerColsButton[a_player].coloumn[0].gameObject);
        // //c_GameSettings.availableBrains[iCurrentClassSelection[a_player]];//c_Characters[iCurrentClassSelection[a_player]];
        SubClassBrain c_currChar = GameSettings.Instance.availableBrains.FirstOrDefault(b => b._iBrainID == (int)a_player.eClassState);
        GameObject characterSlot = null;
        // Rocky Road
        if ((int)a_player.eClassState < (int)PlayerBuild.E_ROCKYROAD_STATE.E_ROCKYROAD_BASE_ROCKYROAD_COUNT)
        {
            if (!transform.GetChild(1).gameObject.activeInHierarchy)
            {
                transform.GetChild(1).gameObject.SetActive(true);
            }
            characterSlot = transform.GetChild(1).gameObject;
            transform.GetChild(2).gameObject.SetActive(false);
        }
        // Princess Cake // We already know it's greater than Rocky because it's reached this statement
        else if ((int)a_player.eClassState < (int)PlayerBuild.E_BASE_CLASS_STATE.E_BASE_CLASS_BASE_PRINCESSCAKE_COUNT)
        {
            if (!transform.GetChild(2).gameObject.activeInHierarchy)
            {
                transform.GetChild(2).gameObject.SetActive(true);
            }
            characterSlot = transform.GetChild(2).gameObject;
            transform.GetChild(1).gameObject.SetActive(false);
        }
        // Pizza Punk
        else if ((int)a_player.eClassState < (int)PlayerBuild.E_BASE_CLASS_STATE.E_BASE_CLASS_BASE_PIZZAPUNK_COUNT)
        {
            // TODO: get child 3 // Set Child 2 to Inactive ^ same for others
            if (!transform.GetChild(2).gameObject.activeInHierarchy)
            {
                transform.GetChild(2).gameObject.SetActive(true);
            }
            characterSlot = transform.GetChild(2).gameObject;
            transform.GetChild(1).gameObject.SetActive(false);
        }

        SkinnedMeshRenderer characterSkinnedRenderer = characterSlot.GetComponentInChildren<SkinnedMeshRenderer>();
        Animator characterAnimator = characterSlot.GetComponent<Animator>();
        characterSkinnedRenderer.sharedMaterial = c_currChar.GetStateMaterial(1);//GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial;
        characterSkinnedRenderer.sharedMesh = c_currChar.GetStateMesh(1);// GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
        characterAnimator.runtimeAnimatorController = c_currChar.GetAnimatorController(); //GetComponent<Animator>().runtimeAnimatorController;
        characterAnimator.avatar = c_currChar.GetAnimatorAvatar();// GetComponent<Animator>().avatar;

        characterSlot.transform.rotation = c_currChar._rotation; //transform.rotation;
        characterSlot.transform.localScale = new Vector3(c_currChar._localScale, c_currChar._localScale, c_currChar._localScale); // transform.localScale;
    }
}
