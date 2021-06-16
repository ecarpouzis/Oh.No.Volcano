using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public Transform DoorT;
    public Collider DoorCollider;

    public override void OnInteract(object o)
    {
        bool b = (bool)o;
        if (b)
        {
            OpenAmount = 1f;
        }
        else
        {
            OpenAmount = 0f;
        }
    }

    private Vector3 DoorClosed;
    private Vector3 DoorOpen;

    [Range(0f, 1f)]
    public float OpenAmount;
    private Vector3 DesiredPosition;
    private float doorSpeed = 22f;

    // Use this for initialization
    void Start()
    {
        DoorClosed = DoorT.localPosition;
        DoorOpen = DoorT.localPosition;
        DoorOpen.y = 0f;

        DesiredPosition = DoorClosed;
    }

    // Update is called once per frame
    void Update()
    {
        float openY = Mathf.Lerp(DoorClosed.y, DoorOpen.y, OpenAmount);
        float curY = DoorT.localPosition.y;
        float newY = curY;
        if (openY > curY)
        {
            float delta = doorSpeed * Time.deltaTime;
            if (curY + delta > openY)
                newY = openY;
            else
                newY = curY + delta;
        }
        else
        {
            float delta = doorSpeed * Time.deltaTime;
            if (curY - delta < openY)
                newY = openY;
            else
                newY = curY - delta;
        }
        Vector3 newPos = DoorT.localPosition;
        newPos.y = newY;
        DoorT.localPosition = newPos;

        if (OpenAmount < .95f)
        {
            DoorCollider.enabled = true;
        }
        else
        {
            DoorCollider.enabled = false;
        }
    }
}
