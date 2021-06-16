using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource Intro, Loop;

    private Coroutine c;
    
    private void OnEnable()
    {
        c = StartCoroutine(Loopc());
    }

    private void OnDisable()
    {
        if (c != null)
            StopCoroutine(c);

        Intro.Stop();
        Loop.Stop();
    }

    IEnumerator Loopc()
    {
        Intro.Play();
        yield return new WaitForSeconds(Intro.clip.length);
        while (true)
        {
            Loop.Play();
            yield return new WaitForSeconds(Loop.clip.length);
        }
    }
}
