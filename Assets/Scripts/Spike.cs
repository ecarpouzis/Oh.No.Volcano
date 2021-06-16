using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public Transform Spikes, SpikeUp, SpikeDown;
    AudioSource spikeSound;
    private BoxCollider killCollider;

    private List<Guid> list;

    private int numCanKill = 0;

    // Use this for initialization
    void Start()
    {
        spikeSound = GetComponent<AudioSource>();
        killCollider = GetComponent<BoxCollider>();
        StartCoroutine(SpikeCoroutine());
        Spikes.localPosition = SpikeUp.localPosition;
        list = new List<System.Guid>();
    }

    private void OnTriggerStay(Collider other)
    {
        Person p;
        if (other.gameObject.IsPerson(out p))
        {
            if (numCanKill > 0)
            {
                Objectives.IncrementCounter(ObjType.SpikeDeath);
                p.Die();
                numCanKill--;
            }
        }
    }

    IEnumerator SpikeCoroutine()
    {
        while (true)
        {
            const float moveTime = .1f;

            numCanKill = 10;
            Vector3 startPos = Spikes.localPosition;
            Vector3 desiredPos = SpikeUp.localPosition;
            float dt = 0;
            while (dt < moveTime)
            {
                yield return new WaitForEndOfFrame();
                dt += Time.deltaTime;
                float lerp = dt / moveTime;
                Spikes.localPosition = Vector3.Lerp(startPos, desiredPos, lerp);
                if (lerp > .5f)
                {
                    if (!spikeSound.isPlaying)
                    {
                        spikeSound.Play();
                    }
                    killCollider.enabled = true;
                }
            }
            yield return new WaitForSeconds(1f);

            startPos = Spikes.localPosition;
            desiredPos = SpikeDown.localPosition;
            dt = 0;
            while (dt < moveTime)
            {
                yield return new WaitForEndOfFrame();
                dt += Time.deltaTime;
                float lerp = dt / moveTime;
                Spikes.localPosition = Vector3.Lerp(startPos, desiredPos, lerp);
                if (lerp > .5f)
                {
                    killCollider.enabled = false;
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
