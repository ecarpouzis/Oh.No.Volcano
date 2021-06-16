using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    Animator animator;
    float force = 25f;
    AudioSource springSound;
    private bool objectiveTrack = false;

    void Awake()
    {
        springSound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        Person p;
        if (other.gameObject.IsPerson(out p))
        {
            animator.SetTrigger("DoSpring");
            p.MyRigidbody.velocity = Vector3.up * force;

            if (!springSound.isPlaying)
            {
                springSound.Play();
            }
            if (!objectiveTrack)
            {
                objectiveTrack = true;
                Objectives.IncrementCounter(ObjType.SpringBounces);
            }
        }
    }


}
