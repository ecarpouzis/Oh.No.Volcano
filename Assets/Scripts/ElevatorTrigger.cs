using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Person p;
        if (other.gameObject.IsPerson(out p))
        {
            p.transform.parent = transform;
            p.MyRigidbody.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Person p;
        if (other.gameObject.IsPerson(out p))
        {
            p.transform.parent = GameController.Instance.PeopleSpawner.transform;
            p.MyRigidbody.useGravity = true;
        }
    }
}
