/// ----------------------------------
/// <summary>
/// Name: PlayerSelect.cs
/// Author: David Azouz
/// Date Created: 28/08/2016
/// Date Modified: 8/2016
/// ----------------------------------
/// Brief: Player Select class that selects players with individual controllers
/// viewed: http://beesbuzz.biz/code/hsv_color_transforms.php
/// http://answers.unity3d.com/questions/379752/mantain-color-brightness-and-change-only-hue.html
/// http://wiki.unity3d.com/index.php/HSBColor
/// 
/// *Edit*
/// - Background colour working - David Azouz 4/10/2016
/// -  - David Azouz 28/08/2016
/// 
/// TODO:
/// - 
/// - 
/// </summary>
/// ----------------------------------

using UnityEngine;
using System.Collections;


public class BackgroundColorChanger : MonoBehaviour
{
    [SerializeField] private Camera c_Camera;
    public bool isForward = true;
    public float fLerpSpeed = 0.5f;
    public Color m_cStartingColor;
    float fCurrentHue;
    float sH;
    float sS;
    float sV;

    // Use this for initialization
    void Start ()
    {
        c_Camera = Camera.main;
        Color.RGBToHSV(m_cStartingColor, out sH, out sS, out sV);
        fCurrentHue = sH;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isForward)
        {
            fCurrentHue += Time.deltaTime * fLerpSpeed;
        }
        else
        {
            fCurrentHue -= Time.deltaTime * fLerpSpeed;
        }

        fCurrentHue = Mathf.Repeat(fCurrentHue, 1);
        c_Camera.backgroundColor = Color.HSVToRGB(fCurrentHue, sS, sV);
    }
}
