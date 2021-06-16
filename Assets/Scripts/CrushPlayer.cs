using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushPlayer : MonoBehaviour
{
    void OnTriggerStay(Collider collider)
    {
        Person p;
        if (collider.gameObject.IsPerson(out p))
        {
            p.Die();
            Objectives.IncrementCounter(ObjType.BuildingDeath);
        }
    }
}
