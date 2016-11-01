///<summary>
/// http://mathforum.org/library/drmath/view/60433.html
/// https://docs.unity3d.com/ScriptReference/RaycastHit-point.html
/// 
/// TODO: 
/// - Allow the UI ring to displayh in front of objects like the knife and bowls in kitchen.
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBossLevel : MonoBehaviour
{

    public Slider c_BossSlider;
    public Image c_WheelImage;
	public float fDisFromGround = 0.5f;
    public LayerMask mask = -1;
    private BossBlobs r_BossBlobs;

    // Use this for initialization
    void Start()
    {
        c_BossSlider = GetComponentInChildren<Slider>();
        r_BossBlobs = GetComponentInParent<BossBlobs>();
    }

    // Update is called once per frame
    void Update()
    {
        // normalise the power value = (between 0 and 1) then 
        // times by ten to make it equal between 0 and 100.
        c_BossSlider.value = 1 + (r_BossBlobs.m_Power - 1) * (100 - 1) / (100 - 1) / 2;

        RaycastHit hit;
        Ray ray = new Ray(transform.parent.position, -transform.parent.up);
        // shouldn't be an if check as we're always "hitting" something (floor)
        Physics.Raycast(ray, out hit, 100, mask.value);
        Vector3 prevPos = transform.position;
		transform.position = Vector3.Lerp(prevPos, new Vector3(transform.position.x, hit.point.y + fDisFromGround, transform.position.z), Time.deltaTime + 0.75f);// 0.5f);

        // This is to keep the object the correct rotation without flickering.
        transform.rotation = Quaternion.AngleAxis(-90.0f, Vector3.left);
    }
}
