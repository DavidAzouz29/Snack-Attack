using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource;
    public AudioSource musicSource;
    public static SoundManager instance = null;
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.0f;

	void Awake ()
    {
	    if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
	}

    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void RandomizeSfx(AudioClip clip)
    {
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clip;
        efxSource.Play();

        Debug.Log("SoundMan rPitch" + randomPitch);
    }
}
