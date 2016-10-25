///<summary>
/// http://mathforum.org/library/drmath/view/60433.html
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBossLevel : MonoBehaviour {

    public Slider c_BossSlider;
    public Image c_WheelImage;
    public float fJumpHeight = 2.8f;
    public float fShelfHeight = 12f;
    //private Vector3 v3StartingPos;
    private BossBlobs r_BossBlobs;

	// Use this for initialization
	void Start ()
    {
        c_BossSlider = GetComponentInChildren<Slider>();
        r_BossBlobs = GetComponentInParent<BossBlobs>();
        fJumpHeight = 2.8f;
        //v3StartingPos = transform.localPosition;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // normalise the power value = (between 0 and 1) then 
        // times by ten to make it equal between 0 and 100.
        c_BossSlider.value = 1 + (r_BossBlobs.m_Power - 1) * (100 - 1) / (100 - 1) / 2;

        if (transform.parent.position.y < fJumpHeight)
        {
            transform.position = new Vector3(transform.position.x, -0.48f, transform.position.z);
        }
        else if (transform.parent.position.y > fShelfHeight)
        {
            transform.position = transform.position; // TODO: fix for shelves
        }

        // This is to keep the object the correct rotation without flickering.
        transform.rotation = Quaternion.AngleAxis(-90.0f, Vector3.left);
    }
}
