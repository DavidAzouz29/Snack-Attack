/// ----------------------------------
/// <summary>
/// Name: MenuSorting.cs
/// Author: David Azouz
/// Date Created: 15/08/2016
/// Date Modified: 8/2016
/// ----------------------------------
/// Brief: Sorts the order Levels are presented on Level Select Screen
/// viewed: https://msdn.microsoft.com/en-us/library/bb341731(v=vs.110).aspx
/// https://docs.unity3d.com/Manual/AnimationScripting.html
/// https://docs.unity3d.com/Manual/animeditor-AnimationCurves.html
/// 
/// *Edit*
/// - Sorting the menu - David Azouz 15/08/2016
/// - Tint applied, offset - David Azouz 16/08/2016
/// TODO:
/// - Because it's Button - it's still using the Axis name
/// - make offset, tint, animation, sorting work
/// - remove alpha
/// - Animation? over Animator
/// - Change from keycode to "Button"
/// </summary>
/// ----------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class MenuSorting : MonoBehaviour
{
    [SerializeField]
    private Transform centre; // TODO: private
    public Vector3 offset; // TODO: private
    //public Animation c_fadeAndSwipe;
    [SerializeField]
    public Animator[] c_fadeAndSwipe = new Animator[LEVEL_COUNT];

    //private AnimationClip fadeLeft;
    //private AnimationClip fadeRight; //AnimationState

    const ushort LEVEL_COUNT = 5;
    const ushort SHOW_COUNT = 3; // how many levels to show (hide the rest)
    [SerializeField]
    int currentLevel = 0;
    //[SerializeField]
    public GameObject[] r_LevelsCount = new GameObject[LEVEL_COUNT]; //TOOD: private? Designers drag on or one prefab?
    [SerializeField]
    private GameObject[] r_LevelsShow = new GameObject[SHOW_COUNT]; // Show only (3)
    // Movement
    private float fSensitivity = 0.2f;
    private float moveHorizontal;
    private float moveVertical;

    // Our tints
    //const float panelAlpha = 0.7f;
    Color colorClear = Color.white;
    Color colorMid   = Color.HSVToRGB(0.0f, 16.0f, 92.0f);
    Color colorDark  = Color.gray;

    // Use this for initialization
    void Start ()
    {
        //colorClear.a = colorMid.a = colorDark.a = panelAlpha; // TODO: remove alpha
        //r_Levels = GetComponentsInChildren<GameObject>();
        //c_fadeAndSwipe.GetClip("UILevelSelectFadeLeft").legacy = true;
        //c_fadeAndSwipe.GetClip("UILevelSelectFadeRight").legacy = true;
        //fadeLeft = c_fadeAndSwipe.GetClip("UILevelSelectFadeLeft"); ////c_fadeAndSwipe["UILevelSelectFadeRight"];
        //fadeRight = c_fadeAndSwipe.GetClip("UILevelSelectFadeRight"); //c_fadeAndSwipe["UILevelSelectFadeRight"];
        currentLevel = 0;

        for (short i = 0; i < LEVEL_COUNT; ++i)
        {
            // Get each animation component
            c_fadeAndSwipe[i] = r_LevelsCount[i].GetComponent<Animator>();
            //r_LevelsCount[i].SetActive(false);
        }

        // turn everything we're not using, turn off (the screen)
        /*for (int i = SHOW_COUNT; i < LEVEL_COUNT; ++i)
        {
            r_LevelsCount[i].SetActive(false);
        } */

        // Proceed to "Show" the next (3/ SHOW_COUNT) levels from our current level
        for (int i = currentLevel; i < SHOW_COUNT; ++i)
        {
            r_LevelsShow[i] = r_LevelsCount[currentLevel + i];
            r_LevelsShow[i].GetComponent<Image>().gameObject.SetActive(true);
        }
        
        FixLayout();
    }

    // Update is called once per frame
    void Update()
    {
        // Left/ Down Button "Horizontal"?
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            c_fadeAndSwipe[currentLevel].Play("UIFadeLeft");
            c_fadeAndSwipe[SHOW_COUNT - 1].Play("UIFadeRight");
            UnityEditor.EditorApplication.isPaused = true;
            MenuLeft();
            Debug.Log("Menu_Left");
        }

        // Right/ Up Button Input.GetButtonDown("Vertical") 
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            c_fadeAndSwipe[currentLevel].Play("UIFadeRight");
            c_fadeAndSwipe[SHOW_COUNT - 1].Play("UIFadeLeft");
            MenuRight();
            Debug.Log("Menu_Right");
        }
        /*
        // Analog sticks
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        // Left/ Down Button
        if (moveHorizontal < -fSensitivity || moveVertical < -fSensitivity)
        {
            MenuLeft();
        }

        // Right/ Up Button
        if (moveHorizontal > fSensitivity || moveHorizontal > fSensitivity)
        {
            MenuRight();
        }
        */
    }

    #region Menu Cycle (Left/ Right)
    /// -----------------------------------
    /// <summary>
    /// These functions are cross-platform.
    /// To be called on event
    /// </summary>
    /// -----------------------------------

    /// <summary>
    /// Cycle - or V
    /// </summary>
    void MenuLeft()
    {
        currentLevel--;
        if (currentLevel < 0)
        {
            // loop back around to the 'end'
            currentLevel = LEVEL_COUNT - 1;
        }
        MenuCycle(false); //GameObjectExtensions.SortChildren(this.gameObject);
    }

    /// <summary>
    /// Cycle -> or ^
    /// </summary>
    void MenuRight()
    {
        currentLevel++;
        if (currentLevel >= LEVEL_COUNT)
        {
            // loop back around to the 'front'
            currentLevel = 0;
        }
        MenuCycle(true);
    }

    /// <summary>
    /// Players are assumed to cycle right more
    /// often as they are the levels that they will see.
    /// </summary>
    /// <param name="isRight">Are we moving right?</param>
    void MenuCycle(bool isRight)
    {
        // Proceed to "Show" the next (3/ SHOW_COUNT) levels from our current level
        for (ushort i = 0; i < SHOW_COUNT; ++i)
        {
            r_LevelsShow[i] = r_LevelsCount[(i + currentLevel) % LEVEL_COUNT];
            if (!r_LevelsShow[i].GetComponent<Image>().IsActive())
            {
                r_LevelsShow[i].GetComponent<Image>().gameObject.SetActive(true);
            }
        }

        if (isRight)
        {
            Debug.Log("Menu_C_Right");
        }
        else
        {
            Debug.Log("Menu_C_Left");
        }

        FixLayout();
    }

    void FixLayout()
    {
        // ---------------------------
        // Hacky way of applying properties
        // --------------------------
        // Change visible order
        // --------------------------
        r_LevelsShow[0].transform.SetAsLastSibling();
        r_LevelsShow[1].transform.SetSiblingIndex(r_LevelsShow[0].transform.GetSiblingIndex() - 1);
        r_LevelsShow[2].transform.SetSiblingIndex(r_LevelsShow[1].transform.GetSiblingIndex() - 1);
        r_LevelsCount[3].GetComponent<Image>().gameObject.SetActive(false);
        r_LevelsCount[4].GetComponent<Image>().gameObject.SetActive(false);

        // ---------------------------
        // Offset Position
        // ---------------------------
        r_LevelsShow[0].GetComponent<RectTransform>().position = centre.position;
        r_LevelsShow[1].GetComponent<RectTransform>().position = r_LevelsShow[0].GetComponent<RectTransform>().position + offset;
        r_LevelsShow[2].GetComponent<RectTransform>().position = r_LevelsShow[1].GetComponent<RectTransform>().position + offset;

        // ---------------------------
        // Tint
        // ---------------------------
        r_LevelsShow[0].GetComponent<Image>().color = colorClear;
        r_LevelsShow[1].GetComponent<Image>().color = colorMid;
        r_LevelsShow[2].GetComponent<Image>().color = colorDark;
    }
    #endregion
}
