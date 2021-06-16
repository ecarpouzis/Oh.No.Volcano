using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{

    float speed = -40f;
    float triggerDistance = 45f;
    AudioSource carSound;
    private Rigidbody MyRigidbody;

    private void Awake()
    {
        carSound = GetComponent<AudioSource>();
        MyRigidbody = GetComponent<Rigidbody>();
    }

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
        if (Vector3.Distance(cameraPosition, myAdjustedPosition) < triggerDistance)
        {
            StartCar();
        }
    }

    void StartCar()
    {
        if (!carSound.isPlaying)
        {
            carSound.Play();
        }
        MyRigidbody.velocity = Vector3.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        Person p;
        if (collision.gameObject.IsPerson(out p))
        {
            p.MyRigidbody.useGravity = true;
            p.MyRigidbody.velocity = Vector3.Lerp(transform.right, transform.forward, Random.Range(0f, 1f)) * 35f;
            
            p.Die();
            Objectives.IncrementCounter(ObjType.CarDeath);
        }
    }
}