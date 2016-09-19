using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsAndControls : MonoBehaviour
{
    [SerializeField]
    GameObject image;

    private bool m_imageShown = false;

    // Update is called once per frame
    void Update()
    {
        if (m_imageShown)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                image.GetComponent<Image>().enabled = false;
            }
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            image.GetComponent<Image>().enabled = true;
            m_imageShown = true;
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