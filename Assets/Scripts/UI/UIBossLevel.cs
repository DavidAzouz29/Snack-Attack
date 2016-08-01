///<summary>
/// http://mathforum.org/library/drmath/view/60433.html
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBossLevel : MonoBehaviour {

    public Slider c_BossSlider;
    public Image c_SkullImage;
    public float deleteMe;
    private BossBlobs r_BossBlobs;

	// Use this for initialization
	void Start ()
    {
        c_BossSlider = GetComponentInChildren<Slider>();
        r_BossBlobs = GetComponentInParent<BossBlobs>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // normalise the power value = (between 0 and 1) then 
        // times by ten to make it equal between 0 and 100.
        deleteMe = 1 + (r_BossBlobs.m_Power - 1) * (100 - 1) / (100 - 1) / 2;
        c_BossSlider.value = deleteMe;
    }

    public void SkullOn()
    {
        c_SkullImage.gameObject.SetActive(true);
    }

    public void SkullOff()
    {
        c_SkullImage.gameObject.SetActive(false);
    }
}
