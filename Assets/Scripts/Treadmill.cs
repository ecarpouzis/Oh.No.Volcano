using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treadmill : MonoBehaviour
{
    Renderer render;
    float offset;
    float speed;
    static List<Person> onAnyTreadmill = new List<Person>();
    List<Person> onMe = new List<Person>();

    void Awake()
    {
        render = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.IsPerson())
        {
            Person foundPerson = collision.GetComponent<Person>();
            if (!onAnyTreadmill.Contains(foundPerson))
            {
                onAnyTreadmill.Add(foundPerson);
                onMe.Add(foundPerson);
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.IsPerson())
        {
            Person foundPerson = collision.GetComponent<Person>();
            if (onMe.Contains(foundPerson))
            {
                onMe.Remove(foundPerson);
                onAnyTreadmill.Remove(foundPerson);
            }
        }
    }

    void OnTriggerStay(Collider collision)
    {
        Person p;
        if (collision.gameObject.IsPerson(out p))
        {
            if (!onAnyTreadmill.Contains(p))
            {
                onAnyTreadmill.Add(p);
                onMe.Add(p);
            }
            if (onMe.Contains(p))
            {
                Vector3 newPos = p.MyRigidbody.position;
                newPos.z += Time.deltaTime * speed;
                p.MyRigidbody.MovePosition(newPos);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        offset += Time.deltaTime;
        if (GameController.Instance.TutorialMode)
        {
            speed = -8f;
        }
        else if (!GameController.Instance.IsPowerup)
        {
            speed = (GameController.Instance.PersonManager.GuideSpeed - GameController.Instance.LavaSpeed) * -2f;
        }
        else
        {
            speed = -10f;
        }
        render.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
