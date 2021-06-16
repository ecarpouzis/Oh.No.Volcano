using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static bool IsPerson(this GameObject g, out Person person)
    {
        Person p = g.GetComponent<Person>();
        person = p;
        if (p != null)
        {
            return true;
        }
        return false;
    }
    public static bool IsPerson(this GameObject g)
    {
        Person p;
        return g.IsPerson(out p);
    }
}
