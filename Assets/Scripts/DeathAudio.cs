using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAudio : MonoBehaviour {

    public List<AudioSource> DeathSounds;

    private static bool CanPlay = true;

    public void playSound()
    {
        if (CanPlay)
        {
            DeathSounds[Random.Range(0, DeathSounds.Count)].Play();
            StartCoroutine(cooldown());
        }
    }

    private IEnumerator cooldown() {
        CanPlay = false;
        yield return new WaitForSeconds(.3f);
        CanPlay = true;
    }
}
