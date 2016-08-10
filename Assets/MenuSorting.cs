using UnityEngine;
using System.Collections;

public class MenuSorting : MonoBehaviour
{
    private float fSensitivity = 0.2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if the positive or negative value "amount pushed" is
        // greater than our sensitivity boundary
        if(Mathf.Abs(Input.GetAxis("Horizontal")) > fSensitivity)
        {
            GameObjectExtensions.SortChildren(this.gameObject);
        }

        if(Mathf.Abs(Input.GetAxis("Vertical")) > fSensitivity)
        {

        }
	
	}
}
