/// ----------------------------------
/// <summary>
/// Name: ScreenshotInEditor.cs
/// Author: Jonathan Czeck (aarku)
/// Date Created: 20/07/2016
/// Date Modified: 2/8/2016
/// ----------------------------------
/// Brief: Take Screenshots in Unity
/// Place this script in YourProject/Assets/Editor and a menu item
/// will automatically appear in the Custom menu after it is compiled.
/// viewed: http://wiki.unity3d.com/index.php/TakeScreenshotInEditor
/// *Edit*
/// - based used from J.C. - David Azouz 2/08/2016
/// TODO:
/// - 
/// </summary>
/// ----------------------------------
using UnityEngine;
using UnityEditor;

public class ScreenshotInEditor : MonoBehaviour
{
    public static string fileName = "./!Art/Screenshots/"; //Assets/Editor/";// Editor Screenshot ";
    public static int startNumber = 1;

    [MenuItem("Custom/Take Screenshot of Game View %^s")]
    static void TakeScreenshot()
    {
        int number = startNumber;
        string name = "" + number;

        while (System.IO.File.Exists(fileName + name + ".png"))
        {
            number++;
            name = "" + number;
        }

        startNumber = number + 1;

        Application.CaptureScreenshot(fileName + name + ".png");
    }
}

