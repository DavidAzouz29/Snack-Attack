/// ----------------------------------
/// <summary>
/// Name: PlayVideo.cs
/// Author: David Azouz
/// Date Created: 28/09/2016
/// Date Modified: 6/2016
/// ----------------------------------
/// Brief: Plays videos for level select
/// viewed: Unity 5 UI Tutorial - How to Add Video https://youtu.be/dWncJP6KMxc
/// 
/// *Edit*
/// - Movies playing - David Azouz 28/09/2016
/// -  - David Azouz 20/10/2016
/// TODO:
/// - Get audio from Movie is not required
/// - 
/// </summary>
/// ----------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayVideo : MonoBehaviour
{
    public RawImage c_KitchenImage;
    public RawImage c_BanquetImage;

    public MovieTexture c_KitchenMovie;
    public MovieTexture c_BanquetMovie;

    // Use this for initialization
    void Start ()
    {
        c_KitchenImage.texture = c_KitchenMovie as MovieTexture;
        c_BanquetImage.texture = c_BanquetMovie as MovieTexture;

        c_KitchenMovie.Play();
        c_BanquetMovie.Play();

        c_KitchenMovie.loop = true;
        c_BanquetMovie.loop = true;
    }
	
	// Update is called once per frame
	/*void Update ()
    {
	
	}*/
}
