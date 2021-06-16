using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingLava : MonoBehaviour
{

    void OnTriggerStay(Collider collider)
    {
        Person p;
        if (collider.gameObject.IsPerson(out p))
        {
            p.Die();
        }
    }

}
