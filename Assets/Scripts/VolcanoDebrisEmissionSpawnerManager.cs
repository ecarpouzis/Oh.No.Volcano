using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoDebrisEmissionSpawnerManager : MonoBehaviour
{
    public GameObject prefab;

    float time = 0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (time <= 0f)
        {
            StartCoroutine(TriggerEvent());
        }
        time -= Time.deltaTime;
    }

    private IEnumerator TriggerEvent()
    {
        float eventTick = Random.Range(.3f, .7f);
        int eventCount = Random.Range(1, 4);

        for (int i = 0; i < eventCount; i++)
        {
            yield return new WaitForSeconds(eventTick);
            Spawn();
        }

        time = Random.Range(.8f, 1.5f);

    }
    private void Spawn()
    {
        GameObject g = Instantiate(prefab);
        //float newScale = Random.Range(.75f, 3f);
        g.transform.localScale = new Vector3(Random.Range(.075f, .3f), Random.Range(.075f, .3f), Random.Range(.075f, .3f));

        var rb = g.GetComponent<Rigidbody>();
        rb.Sleep();

        rb.position = transform.position;

        Vector3 vel = new Vector3(Random.Range(-9f, 9f), Random.Range(20f, 40f), Random.Range(-9f, 9f));
        rb.velocity = vel;

        Vector3 angVel = new Vector3();
        float rem = 90f;
        angVel.x = Random.Range(0f, rem);
        rem -= angVel.x;
        angVel.y = Random.Range(0f, rem);
        rem -= angVel.y;
        angVel.z = Random.Range(0f, rem);
        rb.angularVelocity = angVel;
    }
}
