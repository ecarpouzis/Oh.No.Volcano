using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnEnable : MonoBehaviour
{

    public AudioSource AudioSource;

    private void OnEnable()
    {
        if (AudioSource != null)
            AudioSource.Play();
    }
}
