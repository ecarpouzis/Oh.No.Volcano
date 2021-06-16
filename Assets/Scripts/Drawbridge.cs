using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawbridge : Interactable
{
    private enum State { Opening, Open, Closing, Closed }

    public Transform BridgeFloor;
    public GameObject Rope1Start;
    public GameObject Rope1End;
    public LineRenderer Rope1;
    public GameObject Rope2Start;
    public GameObject Rope2End;
    public LineRenderer Rope2;
    public AnimationCurve OpenAnimation;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private float totalDuration;
    private float animationPosition = 0f;

    private State currentState;

    private void Awake()
    {
        closedRotation = BridgeFloor.localRotation;
        Vector3 ea = BridgeFloor.localRotation.eulerAngles;
        ea.x += 90f;
        openRotation = Quaternion.Euler(ea);
        totalDuration = OpenAnimation.keys[OpenAnimation.length - 1].time;
        currentState = State.Closed;
    }

    void Update()
    {
        if (currentState == State.Opening)
        {
            BridgeFloor.localRotation = Quaternion.Slerp(closedRotation, openRotation, OpenAnimation.Evaluate(animationPosition));

            animationPosition += Time.deltaTime;
            
            if (animationPosition > totalDuration)
            {
                currentState = State.Open;
                BridgeFloor.localRotation = openRotation;
                animationPosition = 1f;
            }
        }
        else if (currentState == State.Closing)
        {
            BridgeFloor.localRotation = Quaternion.Slerp(closedRotation, openRotation, OpenAnimation.Evaluate(animationPosition));

            animationPosition -= Time.deltaTime;
            
            if (animationPosition < 0f)
            {
                currentState = State.Closed;
                BridgeFloor.localRotation = closedRotation;
                animationPosition = 0f;
            }
        }

        UpdateRopes();
    }

    private void UpdateRopes()
    {
        Rope1.numPositions = 2;
        Rope1.SetPosition(0, Rope1Start.transform.position);
        Rope1.SetPosition(1, Rope1End.transform.position);

        Rope2.numPositions = 2;
        Rope2.SetPosition(0, Rope2Start.transform.position);
        Rope2.SetPosition(1, Rope2End.transform.position);
    }

    public override void OnInteract(object o)
    {
        bool b = (bool)o;
        if (b)
        {
            currentState = State.Opening;
        }
        else
        {
            currentState = State.Closing;
        }
    }
}
