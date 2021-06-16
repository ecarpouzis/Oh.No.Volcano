using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    public Transform startPos;
    public Rigidbody crusher;
    public Transform endPos;

    float crushLerp = 0f;
    bool extending = true;
    bool crushing = true;

    void Start()
    {
        StartCoroutine(Crush());
    }

    IEnumerator Crush()
    {
        while (true)
        {
            float maxTick = 1f;
            float tick = 0f;
            while (tick < maxTick)
            {
                yield return new WaitForFixedUpdate();
                tick += Time.deltaTime;
                float lerp = tick / maxTick;
                crusher.MovePosition(Vector3.Lerp(startPos.position, endPos.position, lerp));

            }
            tick = 0f;
            while (tick < maxTick)
            {
                yield return new WaitForFixedUpdate();
                tick += Time.deltaTime;
                float lerp = tick / maxTick;
                crusher.MovePosition(Vector3.Lerp(endPos.position, startPos.position, lerp));

            }
        }
    }

}
