using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public GameObject meteorExplosionPrefab;
    public Transform explosionPosition;
    float speed = 13f;
    float explosionRadius = 5f;

    void Update()
    {
        if (GameController.Instance.CurrentState != GameController.State.InGame && !GameController.Instance.TutorialMode)
            return;

        Vector3 cameraPosition = GameController.Instance.PersonManager.LeftGuide.position;
        cameraPosition.y = 0;
        cameraPosition.x = 0;
        Vector3 myAdjustedPosition = transform.position;
        myAdjustedPosition.y = 0;
        myAdjustedPosition.x = 0;
        if (Vector3.Distance(cameraPosition, myAdjustedPosition) < 25f)
        {
            GetComponent<Rigidbody>().velocity = -1 * transform.up * speed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        Instantiate(meteorExplosionPrefab, explosionPosition.position, Quaternion.identity, transform.parent);

        RaycastHit hit;

        // Cast a sphere wrapping character controller 10 meters forward
        // to see if it is about to hit anything.

        foreach (Collider collider in Physics.OverlapSphere(transform.position, explosionRadius))
        {
            Person p;
            if (collider.gameObject.IsPerson(out p))
            {
                p.Die();
                Objectives.IncrementCounter(ObjType.MeteorDeath);
            }
        }
        Destroy(gameObject);
    }

}
