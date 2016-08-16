/// ----------------------------------
/// <summary>
/// Name: MenuSorting.cs
/// Author: David Azouz
/// Date Created: 15/08/2016
/// Date Modified: 8/2016
/// ----------------------------------
/// Brief: Sorts the Levels and 
/// viewed: https://msdn.microsoft.com/en-us/library/bb341731(v=vs.110).aspx
/// https://docs.unity3d.com/Manual/AnimationScripting.html
/// 
/// *Edit*
/// - Sorting the menu - David Azouz 15/08/2016
/// TODO:
/// - Because it's Button - it's still using the Axis name
/// - make offset, tint, animation, sorting work
/// </summary>
/// ----------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class MenuSorting : MonoBehaviour
{
    public Vector2 centre; // TOOD: private
    public Vector3 offset; // TOOD: private
    public Animation c_fadeAndSwipe;

    private AnimationState fadeLeft;
    private AnimationState fadeRight;

    const ushort LEVEL_COUNT = 5;
    const ushort SHOW_COUNT = 3; // how many levels to show (hide the rest)
    [SerializeField]
    ushort currentLevel = 0;
    //[SerializeField]
    public GameObject[] r_LevelsCount = new GameObject[LEVEL_COUNT]; //TOOD: private? Designers drag on or one prefab?
    [SerializeField]
    private GameObject[] r_LevelsShow = new GameObject[SHOW_COUNT]; // Show only (3)
    // Movement
    private float fSensitivity = 0.2f;
    private float moveHorizontal;
    private float moveVertical;

    // Our tints
    Color colorClear = Color.white;
    Color colorMid   = Color.HSVToRGB(0.0f, 16.0f, 92.0f);
    Color colorDark  = Color.gray;

    // Use this for initialization
    void Start ()
    {
        //r_Levels = GetComponentsInChildren<GameObject>();
        centre = new Vector2(20.0f, -75.0f);
        offset = new Vector2(45.0f, 30.0f);
        c_fadeAndSwipe = GetComponentInChildren<Animation>();
        fadeLeft = c_fadeAndSwipe["UILevelSelectFadeLeft"]; //fadeLeft = c_fadeAndSwipe.GetClip("UILevelSelectFadeLeft");
        fadeRight = c_fadeAndSwipe["UILevelSelectFadeRight"];
        currentLevel = 0;

        // Proceed to "Show" the next (3/ SHOW_COUNT) levels from our current level
        for (ushort i = currentLevel; i < SHOW_COUNT; ++i)
        {
            r_LevelsShow[i] = r_LevelsCount[currentLevel + i];
            r_LevelsShow[i].SetActive(true);
        }

        FixLayout();
    }

    // Update is called once per frame
    void Update()
    {
        // Left/ Down Button
        if(Input.GetButtonDown("Horizontal"))
        {
            MenuLeft();
            Debug.Log("Menu_Left");
            //fadeRight.enabled = false;
        }

        // Right/ Up Button
        if (Input.GetButtonDown("Vertical"))
        {
            MenuRight();
            Debug.Log("Menu_Right");
            //fadeRight.enabled = false;
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
        if(currentLevel > 0)
        {
            currentLevel--;
        }
        // if less than 0
        else
        {
            // loop back around to the 'end'
            currentLevel = LEVEL_COUNT - 1;
        }
        MenuCycle(false); //GameObjectExtensions.SortChildren(this.gameObject);
        //fadeLeft.enabled = true;
    }

    /// <summary>
    /// Cycle -> or ^
    /// </summary>
    void MenuRight()
    {
        if (currentLevel < LEVEL_COUNT)
        {
            currentLevel++;
        }
        // if greater than the end
        else
        {
            // loop back around to the 'front'
            currentLevel = 0;
        }
        MenuCycle(true);
        //fadeRight.enabled = true;
    }

    /// <summary>
    /// Players are assumed to cycle right more
    /// often as they are the levels that they will see.
    /// </summary>
    /// <param name="isRight">Are we moving right?</param>
    void MenuCycle(bool isRight)
    {
        /*// Turn everything off
        for (ushort i = 0; i < LEVEL_COUNT; ++i)
        {
            r_LevelsShow[i].SetActive(false);
        } */

        if (isRight)
        {
            // Proceed to "Show" the next (3/ SHOW_COUNT) levels from our current level
            for (ushort i = currentLevel; i < SHOW_COUNT; ++i)
            {
                r_LevelsShow[0] = r_LevelsCount[i];
                Debug.Log("Menu_Right");
            }
        }
        // left
        else
        {
            // Proceed to "Show" the next (3/ SHOW_COUNT) levels from our current level
            for (ushort i = 0; i > SHOW_COUNT; --i)
            {
                r_LevelsShow[0] = r_LevelsCount[i];
                Debug.Log("Menu_Left");
            }
        }

        FixLayout();
    }

    void FixLayout()
    {
        // ---------------------------
        // Hacky way of applying tint
        // ---------------------------
        // Offset Position
        // ---------------------------
        r_LevelsShow[0].GetComponent<RectTransform>().position = centre;
        r_LevelsShow[1].GetComponent<RectTransform>().position = r_LevelsShow[0].GetComponent<RectTransform>().position += offset;
        r_LevelsShow[2].GetComponent<RectTransform>().position = r_LevelsShow[1].GetComponent<RectTransform>().position += offset;

        // ---------------------------
        // Tint
        // ---------------------------
        r_LevelsShow[0].GetComponent<Image>().color = colorClear;
        r_LevelsShow[1].GetComponent<Image>().color = colorMid;
        r_LevelsShow[2].GetComponent<Image>().color = colorDark;
    }
    #endregion
}
