/// <summary>
/// 
/// *Edit*
/// - Cleaned up and unified Player Selection - David Azouz 15/10/2016
/// 
/// TODO:
/// - clean up (int)
/// </summary>

using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerInfoController : MonoBehaviour
{
	public int PlayerIndex;
	public UnityEngine.UI.Button PlayerColor;
	public UnityEngine.UI.Text c_PlayerBrain;
    [SerializeField] private GameSettings m_ActiveGameSettings;

    //private MainMenuController _mainMenu;
	private GameSettings.PlayerInfo _player;
    public void Awake()
    {
        //_mainMenu = GetComponentInParent<MainMenuController>();
        m_ActiveGameSettings = GetComponentInParent<MainMenuController>().GameSettingsTemplate;
        CharacterSelection(GameSettings.Instance.players[PlayerIndex]);// m_ActiveGameSettings.players[i]);
    }

    public void Refresh()
	{
		_player = GameSettings.Instance.players[PlayerIndex];

		var colorBlock = PlayerColor.colors;
		colorBlock.normalColor = _player.Color;
		colorBlock.highlightedColor = _player.Color;
		PlayerColor.colors = colorBlock;

		c_PlayerBrain.text = (_player.Brain != null)
			? _player.Brain.name.TrimEnd(" Brain".ToCharArray())
			: "None";

    }

	/*public void OnCycleColor()
	{
		_player.Color = _mainMenu.GetNextColor(_player.Color);
		Refresh();
	} */

    // Left
    public void OnPreviousBrain()
    {
        //_player.Brain = _mainMenu.CycleNextSelection(_player.Brain, false);
        _player.Brain = m_ActiveGameSettings.CycleNextSelection(_player.Brain, false);
        CharacterSelection(_player);
        Refresh();
    }

    // Right
    public void OnNextBrain()
	{
        //_player.Brain = _mainMenu.CycleNextSelection(_player.Brain, true);
        _player.Brain = m_ActiveGameSettings.CycleNextSelection(_player.Brain, true);
        CharacterSelection(_player);
        Refresh();
    }

    void CharacterSelection(GameSettings.PlayerInfo a_player)
    {
        //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(playerButtons.playerColsButton[a_player].coloumn[0].gameObject);
        // //c_GameSettings.availableBrains[iCurrentClassSelection[a_player]];//c_Characters[iCurrentClassSelection[a_player]];
        SubClassBrain c_currChar = m_ActiveGameSettings.availableBrains.FirstOrDefault(b => b._iBrainID == (int)a_player.eClassState);
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
