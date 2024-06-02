using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource[] audiosToPlay;

    void PlaySounds()
    {
        if (audiosToPlay != null)
        {
            foreach (AudioSource audio in audiosToPlay)
            {
                audio.Play();
            }
        }
        else
        {
            GetComponent<AudioSource>().Play();
        }
        
    }
}
