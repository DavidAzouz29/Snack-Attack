/// <summary>
/// Author: 		David Azouz
/// Date Created: 	4/11/16
/// Date Modified: 	/16
/// --------------------------------------------------
/// Brief: A recieves audio from the game manager for the main menu
/// viewed 
/// 
/// ***EDIT***
/// - Implemented - David Azouz 4/11/16
/// - 
/// </summary>

using UnityEngine;
using System.Collections;

public class AudioMenuSetup : MonoBehaviour
{
    // Use this for initialization
    void Awake ()
    {
        AudioSource[] audioSourceArrayLocal = GetComponentsInChildren<AudioSource>();
        AudioSource[] audioSourceArrayGM = GameManager.Instance.transform.GetChild(GameManager.iChInGame).GetComponentsInChildren<AudioSource>();

        int i = 0;
        foreach (AudioSource audioSource in audioSourceArrayLocal)
        {
            audioSource.clip = audioSourceArrayGM[i].clip;
            audioSource.outputAudioMixerGroup = audioSourceArrayGM[i].outputAudioMixerGroup;
            audioSource.playOnAwake = audioSourceArrayGM[i].playOnAwake;
            audioSource.loop = audioSourceArrayGM[i].loop;
            audioSource.volume = audioSourceArrayGM[i].volume;
            audioSource.pitch = audioSourceArrayGM[i].pitch;
            i++;
        }
	
	}
}
