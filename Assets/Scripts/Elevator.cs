using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform Platform, Top, Bottom;
    
    private void Awake()
    {
        StartCoroutine(Elevate());
    }

    IEnumerator Elevate()
    {
        while (true)
        {
            const float moveTime = .4f;

            Vector3 startPos = Platform.localPosition;
            Vector3 desiredPos = Top.localPosition;
            float dt = 0;
            while (dt < moveTime)
            {
                yield return null;
                dt += Time.deltaTime;
                float lerp = dt / moveTime;
                Platform.localPosition = Vector3.Lerp(startPos, desiredPos, lerp);
            }
            yield return new WaitForSeconds(.8f);

            startPos = Platform.localPosition;
            desiredPos = Bottom.localPosition;
            dt = 0;
            while (dt < moveTime)
            {
                yield return null;
                dt += Time.deltaTime;
                float lerp = dt / moveTime;
                Platform.localPosition = Vector3.Lerp(startPos, desiredPos, lerp);
            }

            yield return new WaitForSeconds(1.2f);
        }
    }
}
