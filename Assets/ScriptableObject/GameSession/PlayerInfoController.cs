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
    //public bool isReady = false;
    public UnityEngine.UI.Button PlayerColor;
    public UnityEngine.UI.Text c_PlayerBrain;
    public float fSensitivity = 0.2f;

    //private MainMenuController _mainMenu;
    private GameSettings.PlayerInfo _player;
    private Animator[] anims = new Animator[(int)PlayerBuild.E_BASE_CLASS_STATE.E_BASE_CLASS_STATE_BASE_COUNT - 1]; //RECODE: remove -1 for add. char
    private AudioSource c_AudioSource;
    private UnityEngine.UI.Image c_ReadyImage;
    private ArrayLayout playerButtonsArray;
    private int m_iController = 0; // tracks time
    private float m_Timer = 0; // tracks time
    private float m_TimerPause = 0.3f; // Time before next input
    bool isAudioToPlay = false;

    public void Awake()
    {
        //_mainMenu = GetComponentInParent<MainMenuController>();

        Refresh();
        // if a Princess Cake is selected - set the ID //RECODE: remove the "- 1" for additional characters
        for (int i = 0; i < (int)PlayerBuild.E_BASE_CLASS_STATE.E_BASE_CLASS_STATE_BASE_COUNT - 1; i++)
        {
            anims[i] = transform.GetChild(i + 1).GetComponent<Animator>();
            anims[i].SetInteger("AnimationClassID", _player.Brain.GetAnimID());
        }

        //CharacterSelection(GameSettings.Instance.players[PlayerIndex]);
        playerButtonsArray = GetComponentInParent<PlayerSelect>().playerButtons;
        c_ReadyImage = transform.GetChild(0).GetChild(3).GetComponent<UnityEngine.UI.Image>();
        c_ReadyImage.enabled = false;
        m_iController = PlayerIndex + 1;
        fSensitivity = 0.2f;

    }

    public void Start()
    {
        CharacterSelection(_player);// GameSettings.Instance.players[PlayerIndex]);
        Refresh();
    }

    void Update()
    {
        m_Timer += Time.deltaTime;

        // If a player isn't ready, receive input.
        if (!_player.isReady)
        {
            // Left
            if (Input.GetKeyDown("joystick " + (m_iController) + " button 4") && m_Timer >= m_TimerPause ||
                Input.GetAxis("P" + (m_iController) + "_Horizontal") < -fSensitivity && m_Timer >= m_TimerPause)
            {
                OnLeft();
            }

            // Right
            if (Input.GetKeyDown("joystick " + (m_iController) + " button 5") && m_Timer >= m_TimerPause ||
                Input.GetAxis("P" + (m_iController) + "_Horizontal") > fSensitivity && m_Timer >= m_TimerPause)
            {
                OnRight();
            }

            if (Input.GetAxis("P" + (m_iController) + "_Vertical") < -fSensitivity && m_Timer >= m_TimerPause)
            {

            }

            // Select (A on Xbox)
            if (Input.GetKeyDown("joystick " + (m_iController) + " button 7") && m_Timer >= m_TimerPause)
            {
                ToggleIsReady();
            }
        }

        // Submit/ ready
        if (_player.isReady && isAudioToPlay)
        {
            // override the clip anyway
            c_AudioSource = GameManager.Instance.transform.GetChild(2).GetChild(4).GetComponent<AudioSource>();
            c_AudioSource.clip = _player.Brain.GetAudioTaunt(Random.Range(0, 4)); //GameManager.Instance.transform.GetChild(2).GetChild(4).GetComponent<AudioSource>();
        }
        else if (m_iController % 1 == 0 && isAudioToPlay)
        {
            c_AudioSource = GameManager.Instance.transform.GetChild(2).GetChild(5).GetComponent<AudioSource>();
        }
        else if (m_iController % 2 == 0 && isAudioToPlay)
        {
            c_AudioSource = GameManager.Instance.transform.GetChild(2).GetChild(6).GetComponent<AudioSource>();
        }

        // if audio hasn't played
        if (isAudioToPlay)
        {
            c_AudioSource.pitch = 1.0f + (m_iController * 0.1f); //TODO: this will play with the character's taunts?
            c_AudioSource.Play();
            isAudioToPlay = false;
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

    public void OnLeft()
    {
        // Cycle left in character list
        OnPreviousBrain();
        //playerButtonsArray.playerColsButton[PlayerIndex].coloumn[1].Select();
        //playerButtonsArray.playerRows[PlayerIndex].row[PlayerIndex].Select();
        m_Timer = 0;
        isAudioToPlay = true;
    }

    public void OnRight()
    {
        // Cycle right in character list
        OnNextBrain();
        //playerButtonsArray.playerColsButton[PlayerIndex].coloumn[2].Select();
        //playerButtonsArray.playerRows[PlayerIndex].row[PlayerIndex].Select();
        m_Timer = 0;
        isAudioToPlay = true;
    }

    public void ToggleIsReady()
    {
        _player.isReady = !_player.isReady;


        m_Timer = 0;
        //c_AudioSource.clip = _player.Brain.GetAudioTaunt(Random.Range(0, 4)); //TODO: 5?
        c_ReadyImage.enabled = _player.isReady;

        // Attack and walk if we're ready
        anims[(int)_player.eBaseClassState].SetBool("Attack1Left", _player.isReady);
        anims[(int)_player.eBaseClassState].SetBool("Walking", _player.isReady);
        anims[(int)_player.eBaseClassState].SetBool("Idling", !_player.isReady);
        if (_player.isReady)
        {
            anims[(int)_player.eBaseClassState].SetTrigger("AttackToWalk");
            //anims[(int)_player.eBaseClassState].CrossFade("Walkcycle", 0);
            // Ready! anim
            transform.GetChild(0).GetChild(3).GetComponent<Animator>().SetTrigger("Pressed");
        }
        //else
            //anims[(int)_player.eBaseClassState].CrossFade("Idle", 0);
        //c_AudioSource.clip = _player.Brain.GetAudioTaunt(Random.Range(0, 4));
        isAudioToPlay = true;
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

    GameObject CharacterSelection(GameSettings.PlayerInfo a_player)
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
        /*/ Pizza Punk
        else if ((int)a_player.eClassState < (int)PlayerBuild.E_BASE_CLASS_STATE.E_BASE_CLASS_BASE_PIZZAPUNK_COUNT)
        {
            // TODO: get child 3 // Set Child 2 to Inactive ^ same for others
            if (!transform.GetChild(2).gameObject.activeInHierarchy)
            {
                transform.GetChild(2).gameObject.SetActive(true);
            }
            characterSlot = transform.GetChild(2).gameObject;
            transform.GetChild(1).gameObject.SetActive(false);
        }*/

        SkinnedMeshRenderer characterSkinnedRenderer = characterSlot.GetComponentInChildren<SkinnedMeshRenderer>();
        Animator characterAnimator = characterSlot.GetComponent<Animator>();
        characterSkinnedRenderer.sharedMaterial = c_currChar.GetStateMaterial(1);//GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial;
        characterSkinnedRenderer.sharedMesh = c_currChar.GetStateMesh(1);// GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
        characterAnimator.runtimeAnimatorController = c_currChar.GetAnimatorController(); //GetComponent<Animator>().runtimeAnimatorController;
        characterAnimator.avatar = c_currChar.GetAnimatorAvatar();// GetComponent<Animator>().avatar;
        int animClassID = 0;
        switch (_player.eBaseClassState)
        {
            case PlayerBuild.E_BASE_CLASS_STATE.E_BASE_CLASS_STATE_ROCKYROAD:
                {
                    animClassID = 0;
                    break;
                }
            case PlayerBuild.E_BASE_CLASS_STATE.E_BASE_CLASS_STATE_PRINCESSCAKE:
                {
                    animClassID = 2;
                    break;
                }
            default:
                {
                    animClassID = 2;
                    break;
                }
        }
        characterAnimator.SetInteger("AnimationClassID", animClassID);

        characterSlot.transform.rotation = c_currChar._rotation; //transform.rotation;
        characterSlot.transform.localScale = new Vector3(c_currChar._localScale, c_currChar._localScale, c_currChar._localScale); // transform.localScale;
        return characterSlot;
    }
}
