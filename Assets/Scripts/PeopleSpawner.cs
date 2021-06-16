using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleSpawner : MonoBehaviour
{
    public GameObject personPrefab;

    void Start()
    {
        SpawnPeople(35);
    }

    public void SpawnPeople(int numPeople)
    {
        while (numPeople > 0)
        {
            const float spawnRadius = 5f;
            
            Vector2 inUC = Random.insideUnitCircle;
            Vector3 spawn = transform.TransformPoint(new Vector3(inUC.x * spawnRadius, 1f, inUC.y * spawnRadius)); //spawn circle radius

            GameObject person = Instantiate(personPrefab, spawn, Quaternion.identity);

            Person p = person.GetComponent<Person>();
            if (p == null)
            {
                Debug.LogError("Null Person Script");
            }
            else
            {
                p.AddToGC();
                p.ForceSetPosition(p.GuideToken.GetTargetPosition());
            }

            numPeople--;
        }
    }

}
