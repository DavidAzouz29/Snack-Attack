using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;



public class PlayButton : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
	
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(1);
        }
    }

    void OnMouseEnter()
    {
        GetComponentInChildren<Light>().enabled = true;
        
    }

    void OnMouseExit()
    {
        GetComponentInChildren<Light>().enabled = false;
    }
}
