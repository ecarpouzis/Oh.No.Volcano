using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonButton : MonoBehaviour
{
    
    public Material buttonDownMaterial;
    public Material buttonUpMaterial;
    public GameObject buttonTop;
    public Interactable Interactable;

    private Renderer myRenderer;
    private Animator animator;
    private List<Person> peopleOnButton;

    private bool objectiveTrack = false;

    void Awake()
    {
        myRenderer = buttonTop.GetComponent<Renderer>();
        animator = GetComponent<Animator>();
        peopleOnButton = new List<Person>();
    }

    void OnTriggerEnter(Collider other)
    {
        Person p;
        if (other.gameObject.IsPerson(out p))
        {
            if (!peopleOnButton.Contains(p))
            {
                peopleOnButton.Add(p);
                if (peopleOnButton.Count >= 1)
                {
                    myRenderer.material = buttonDownMaterial;
                    animator.speed = 2;
                    animator.SetTrigger("ButtonDown");
                    if (!objectiveTrack)
                    {
                        objectiveTrack = true;
                        Objectives.IncrementCounter(ObjType.ButtonPresses);
                    }
                    if (Interactable != null)
                    {
                        Interactable.Interact(true);
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Person p;
        if (other.gameObject.IsPerson(out p))
        {
            if (!peopleOnButton.Contains(p))
                Debug.LogError("Error person not on button tried to remove");

            peopleOnButton.Remove(p);
            if (peopleOnButton.Count < 1)
            {
                myRenderer.material = buttonUpMaterial;
                animator.speed = 2;
                animator.SetTrigger("ButtonUp");
                if (Interactable != null)
                {
                    Interactable.Interact(false);
                }
            }
        }
    }
}
