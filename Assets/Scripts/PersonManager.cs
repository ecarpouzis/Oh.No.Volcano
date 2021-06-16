using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonManager : MonoBehaviour
{
    public float GuideSpeed = 25f;
    public Transform Guide1, Guide2;
    public Transform CameraContainer;
    public Transform CameraFrontPosition, CameraBackPosition;
    public Transform GuideFrontPosition, GuideBackPosition;

    public int LivingPeople { get { return LeftGroup.Count + RightGroup.Count; } }
    public Transform LeftGuide { get { return Guide1; } }
    public Transform RightGuide { get { return Guide2; } }

    private bool UseTwoGuides { get { return !UseOneGuide; } set { UseOneGuide = !value; } }
    private bool UseOneGuide = false;

    private Vector2 GuideSplitDelta = Vector2.zero;

    private GuideGroup LeftGroup;
    private GuideGroup RightGroup;
    private Vector2 RightStick;
    private Vector2 LeftStick;

    private void Awake()
    {
        LeftGroup = GuideGroup.NewGuideGroup(LeftGuide);
        RightGroup = GuideGroup.NewGuideGroup(RightGuide);
        LeftGroup.name = "Left GuideGroup";
        RightGroup.name = "Right GuideGroup";
        CombineGuides();
    }

    private void Update()
    {
        if (GameController.Instance.CurrentState == GameController.State.InGame || GameController.Instance.TutorialMode)
        {
            RightStick = new Vector2(Input.GetAxis("RightHorizontal"), Input.GetAxis("RightVertical"));
            LeftStick = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            UpdateGuidePositions();
            UpdateCameraPosition();

            if (UseOneGuide)
            {
                if (GuideSplitDelta.magnitude > .8f)
                {
                    SplitGuides();
                }
                else if (RightStick.magnitude > .1f)
                {
                    GuideSplitDelta += RightStick;
                }
                else
                {
                    Vector2.Lerp(RightStick, Vector2.zero, .1f);
                }
            }
        }
    }
    private void UpdateGuidePositions()
    {
        Vector3 lPos = LeftGuide.position;

        lPos.x += LeftStick.x * GuideSpeed * Time.deltaTime;
        lPos.z += LeftStick.y * GuideSpeed * Time.deltaTime;
        if (lPos.z > GuideFrontPosition.position.z)
        {
            lPos.z = GuideFrontPosition.position.z;
        }
        else if (lPos.z < GuideBackPosition.position.z)
        {
            lPos.z = GuideBackPosition.position.z;
        }

        if (lPos.z > (GuideFrontPosition.position.z - 15f))
        {
            GameController.Instance.LavaCatchupMode = true;
        }
        else
        {
            GameController.Instance.LavaCatchupMode = false;
        }

        LeftGuide.position = lPos;

        if (UseTwoGuides)
        {
            if (Input.GetButtonDown("CombineGuides"))
            {
                CombineGuides();
            }
            else
            {
                Vector3 rPos = RightGuide.position;
                rPos.x += RightStick.x * GuideSpeed * Time.deltaTime;
                rPos.z += RightStick.y * GuideSpeed * Time.deltaTime;

                if (rPos.z > GuideFrontPosition.position.z)
                {
                    rPos.z = GuideFrontPosition.position.z;
                }
                else if (rPos.z < GuideBackPosition.position.z)
                {
                    rPos.z = GuideBackPosition.position.z;
                }

                RightGuide.position = rPos;
            }

            if (Input.GetButtonDown("GrowLeft"))
            {
                MovePersonLeft();
            }

            if (Input.GetButtonDown("GrowRight"))
            {
                MovePersonRight();
            }
        }
    }
    private void UpdateCameraPosition()
    {
        float delta = LeftGuide.position.z - GuideBackPosition.position.z;
        float total = GuideFrontPosition.position.z - GuideBackPosition.position.z;

        float minDist = total / 5;
        delta -= minDist;
        total -= minDist;
        float lerp = delta / total;

        if (lerp < 0)
            lerp = 0f;

        Vector3 desiredPos = Vector3.Lerp(CameraBackPosition.position, CameraFrontPosition.position, lerp);

        Person ll = LeftGroup.GetLeader();
        Person rl = RightGroup.GetLeader();
        if (ll != null)
        {
            float personHt = ll.MyRigidbody.position.y - 1f;

            if (rl != null)
            {
                if ((rl.MyRigidbody.position.y - 1) > personHt)
                {
                    personHt = rl.MyRigidbody.position.y - 1f;
                }
            }

            if (personHt > 0)
            {
                desiredPos.y += personHt;
            }
        }

        Quaternion desiredRot = Quaternion.Slerp(CameraBackPosition.rotation, CameraFrontPosition.rotation, lerp);

        CameraContainer.position = Vector3.Lerp(CameraContainer.position, desiredPos, .15f);
        CameraContainer.rotation = desiredRot;
    }

    public void AddPerson(Person p)
    {
        p.transform.parent = GameController.Instance.PeopleSpawner.transform;
        if (UseOneGuide)
        {
            AddLeft(p);
        }
        else
        {
            Vector3 level = LeftGuide.position;
            level.y = p.transform.position.y;
            float lDist = Vector3.Distance(level, p.transform.position);

            level = RightGuide.position;
            level.y = p.transform.position.y;
            float rDist = Vector3.Distance(level, p.transform.position);

            if (lDist > rDist)
            {
                AddLeft(p);
            }
            else
            {
                AddRight(p);
            }
        }
    }

    public void OnPersonDie(Person p)
    {
        Objectives.SetMax(ObjType.MaxSurvivors, LivingPeople);

        if (p != null && p.GuideToken != null)
        {
            if (p.GuideToken.Group == RightGroup && p.GuideToken.Group.Count == 1)
            {
                CombineGuides();
            }
            p.GuideToken.Destroy();
        }

        if (LivingPeople == 0)
            GameController.Instance.GameOver();
    }

    private void AddLeft(Person p)
    {
        if (p != null && p.MyCollider != null)
        {
            p.CollisionLayer = 12;
            p.MyCollider.enabled = false;
            p.MyCollider.enabled = true;
        }
        LeftGroup.GetNewToken(p);
    }
    private void AddRight(Person p)
    {
        if (p != null && p.MyCollider != null)
        {
            p.CollisionLayer = 13;
            p.MyCollider.enabled = false;
            p.MyCollider.enabled = true;
        }
        RightGroup.GetNewToken(p);
    }

    private void CombineGuides()
    {
        UseOneGuide = true;

        GuideSplitDelta = Vector2.zero;
        RightGuide.gameObject.SetActive(false);
        RightGroup.gameObject.SetActive(false);

        while (RightGroup.Count > 0)
        {
            Person p = RightGroup.RemoveLastPerson();
            AddLeft(p);
        }
    }
    private void SplitGuides()
    {
        UseTwoGuides = true;

        Vector3 rightPos = LeftGuide.position;
        rightPos.x += GuideSplitDelta.x;
        rightPos.z += GuideSplitDelta.y;
        RightGuide.position = rightPos;
        RightGuide.gameObject.SetActive(true);
        RightGroup.gameObject.SetActive(true);

        int max = LeftGroup.Count / 2;
        for (int i = 0; i < max; i++)
        {
            Person p = LeftGroup.RemoveLastPerson();
            AddRight(p);
        }
    }
    private void MovePersonLeft()
    {
        if (UseTwoGuides && RightGroup.Count > 1)
        {
            Person p = RightGroup.RemoveLastPerson();
            AddLeft(p);
        }
    }
    private void MovePersonRight()
    {
        if (UseTwoGuides && LeftGroup.Count > 1)
        {
            Person p = LeftGroup.RemoveLastPerson();
            AddRight(p);
        }
    }
}
