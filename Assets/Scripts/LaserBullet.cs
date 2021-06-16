using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    public Rigidbody MyRigidbody;

    private void Awake()
    {
        MyRigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        Person p;
        if (other.gameObject.IsPerson(out p))
        {
            p.Die();
            Objectives.IncrementCounter(ObjType.LaserDeath);
        }
        else
        {
            if (other.name != "ScienceLab")
            {
                Destroy(this.gameObject);
            }
        }
    }
}
