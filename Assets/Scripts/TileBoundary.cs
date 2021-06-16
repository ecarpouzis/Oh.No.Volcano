using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoundary : MonoBehaviour {

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, .5f);
    }

}
