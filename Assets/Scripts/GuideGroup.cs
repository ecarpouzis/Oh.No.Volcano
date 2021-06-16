using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideGroup : MonoBehaviour
{
    public class Token
    {
        public int Index { get; internal set; }
        public GuideGroup Group { get; internal set; }
        public Person MyPerson { get; internal set; }

        public Vector3 GetTargetPosition()
        {
            if (Index < 0)
                Debug.LogError("Index < 0");
            return Group.GetWorldPoint(Index);
        }

        public void Destroy()
        {
            if (Group != null)
            {
                Group.RemoveToken(this);
            }
        }

        internal Token() { }
    }

    private static List<Vector3> childPoints;
    private static int childPointsRadius = 0;

    static GuideGroup()
    {
        childPoints = new List<Vector3>();

        while (childPoints.Count < 25)
        {
            CreateMoreChildPoints();
        }
    }

    public static GuideGroup NewGuideGroup(Transform guide)
    {
        GameObject g = new GameObject("gg");
        g.transform.position = guide.position;
        GuideGroup gg = g.AddComponent<GuideGroup>();
        gg.moveTarget = guide;
        return gg;
    }
    private static void CreateMoreChildPoints()
    {
        if (childPoints.Count == 0)
        {
            childPoints.Add(Vector3.zero);
            childPointsRadius++;
            return;
        }

        const float personDiameter = .55f;
        float radiusB = personDiameter * childPointsRadius;
        float circB = (2 * radiusB) * Mathf.PI;
        int numA = (int)(circB / (personDiameter));

        for (int i = 0; i < numA; i++)
        {
            float x = 0f;
            if (i != 0)
                x = ((Mathf.PI * 2f) / numA) * i;
            float rawX = -Mathf.Cos(x);
            float rawZ = -Mathf.Sin(x);

            Vector3 v = new Vector3();
            v.x = rawX * radiusB;
            v.z = rawZ * radiusB;

            childPoints.Add(v);
        }

        childPointsRadius++;
    }

    public int Count { get; private set; }

    private List<Token> groupMembers = new List<Token>();
    private Transform moveTarget;

    private void Awake()
    {
        Count = 0;
    }

    private void Update()
    {
        const float followDist = .3f;

        Vector3 level = moveTarget.position;
        level.y = transform.position.y;
        float dist = Vector3.Distance(transform.position, level);
        if (dist > followDist)
        {
            Vector3 dv = (level - transform.position).normalized * followDist;
            transform.position = moveTarget.position - dv;
        }
    }

    private Vector3 GetWorldPoint(int pointIndex)
    {
        while (pointIndex >= childPoints.Count)
        {
            CreateMoreChildPoints();
        }

        return transform.TransformPoint(childPoints[pointIndex]);
    }

    public Token GetNewToken(Person p)
    {
        Token t = new Token();
        t.Index = groupMembers.Count;
        t.Group = this;
        t.MyPerson = p;
        groupMembers.Add(t);
        if (p != null)
        {
            p.GuideToken = t;
        }
        Count++;
        return t;
    }

    public void RemoveToken(Token t)
    {
        OrganizeGroup(t.Index);
        Count--;

        t.Group = null;
        t.Index = -1;

        groupMembers.Remove(t);
    }

    public Person RemoveLastPerson()
    {
        Token t = groupMembers[groupMembers.Count - 1];
        RemoveToken(t);
        return t.MyPerson;
    }
    public Person GetLeader()
    {
        if (groupMembers.Count > 0)
            return groupMembers[0].MyPerson;
        return null;
    }

    //"Bubbles" tokens from high indexes to fill in 
    //gaps caused by Person deaths
    private void OrganizeGroup(int removedIndex)
    {
        if (removedIndex == groupMembers.Count - 1)
        {
            return;
        }

        Token rem = groupMembers[removedIndex];

        int lastInd = groupMembers.Count - 1;
        Token last = groupMembers[lastInd];
        last.Index = removedIndex;

        groupMembers[removedIndex] = last;
        groupMembers[lastInd] = rem;
    }
}


