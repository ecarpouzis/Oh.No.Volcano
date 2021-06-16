using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public static readonly int LeftCollisionLayer = 12;
    public static readonly int RightCollisionLayer = 13;
    public static readonly int NoCollisionLayer = 14;
    public static readonly int PickupLayer = 15;
    public static readonly int DeadPerson = 16;

    public bool IsPreplacedPerson = false;

    public List<GameObject> models = new List<GameObject>();

    public Collider MyCollider;

    public GuideGroup.Token GuideToken;
    public Rigidbody MyRigidbody { get; private set; }

    public int CollisionLayer = NoCollisionLayer;
    public bool IsMoving { get; private set; }
    public bool AddedToGC { get; private set; }
    public bool Dead { get; private set; }

    private float speed { get { return GameController.Instance.PersonManager.GuideSpeed; } }

    private bool IsTriggered = false;

    private bool lastPush;
    private Vector3 lastPushVelocity;
    private Animator ModelAnimator;
    private GameObject ModelGO;
    private Vector3 prevPosition;
    
    private bool IsIdle = false;
    private bool IsRunning = false;

    void Awake()
    {
        Dead = false;
        MyRigidbody = GetComponent<Rigidbody>();
        foreach (GameObject go in models)
        {
            go.SetActive(false);
        }

        int baseUnlockedMaxIndex = models.Count - Objectives.MaxStage - 2; //index of all runners you get by default
        int highestUnlockedModelIndex = baseUnlockedMaxIndex + Objectives.CurrentStage; //highest index of any model we have unlocked
        ModelGO = models[Random.Range(0, Mathf.Min(models.Count, highestUnlockedModelIndex + 1))]; // +1 on random because it's exclusive at upper bound
        ModelGO.SetActive(true);
        ModelAnimator = ModelGO.GetComponent<Animator>();

        AddedToGC = false;

        if (IsPreplacedPerson)
        {
            MyCollider.isTrigger = true;
            MyRigidbody.useGravity = false;
            gameObject.layer = PickupLayer;
        }
    }
    void Update()
    {
        if (transform.position.y < -18)
        {
            Die();
        }
    }
    void FixedUpdate()
    {
        if (!Dead && AddedToGC)
        {
            if (GuideToken != null && GuideToken.Index >= 0)
            {
                Vector3 pos = MyRigidbody.position;
                Vector3 target = GuideToken.GetTargetPosition();
                target.y = pos.y;

                float dist = Vector3.Distance(pos, target);
                if (dist < .01f)
                {
                    IsMoving = false;
                    gameObject.layer = CollisionLayer;
                }
                else
                {
                    IsMoving = true;
                    gameObject.layer = NoCollisionLayer;

                    if (pos.y > .8f)
                    {
                        Vector3 dv = (target - pos);
                        dv.y = 0f;
                        dv.Normalize();
                        dv *= speed;
                        lastPushVelocity = dv;
                        dv *= Time.deltaTime;
                        Vector3 desiredPos = MyRigidbody.position + dv;

                        if (Vector3.Distance(MyRigidbody.position, target) < dv.magnitude)
                        {
                            MyRigidbody.MovePosition(target);
                        }
                        else
                        {
                            MyRigidbody.MovePosition(desiredPos);
                        }

                        lastPush = false;
                    }
                    else
                    {
                        if (!lastPush)
                        {
                            lastPushVelocity.y = MyRigidbody.velocity.y;
                            MyRigidbody.velocity = lastPushVelocity;
                            lastPush = true;
                        }
                    }
                }

                ////////////////////////////////


                Vector3 targetPos = GuideToken.GetTargetPosition();
                targetPos.y = this.transform.position.y;

                Vector3 mov1 = MyRigidbody.position;
                mov1.y = 0f;
                Vector3 mov2 = prevPosition;
                mov2.y = 0f;

                float spd = (mov1.magnitude - mov2.magnitude) / Time.deltaTime;
                if (Mathf.Abs(spd) > .1f)
                {
                    if (!IsRunning)
                    {
                        IsRunning = true;
                        IsIdle = false;
                        ModelAnimator.Play("Run");
                    }
                }
                else
                {
                    if (!IsIdle)
                    {
                        IsRunning = false;
                        IsIdle = true;
                        ModelAnimator.Play("Idle");
                    }
                }

                if (IsMoving)
                    transform.LookAt(targetPos);
                prevPosition = MyRigidbody.position;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (IsPreplacedPerson)
        {
            Person p;
            if (other.gameObject.IsPerson(out p))
            {
                if (!IsTriggered)
                {
                    Objectives.IncrementCounter(ObjType.TotalNewSurvivors);
                    transform.parent = GameController.Instance.PeopleSpawner.transform;
                    IsTriggered = true;
                    IsPreplacedPerson = false;
                    MyCollider.isTrigger = false;
                    MyRigidbody.useGravity = true;
                    CollisionLayer = p.CollisionLayer;
                    MyCollider.enabled = false;
                    MyCollider.enabled = true;
                    AddedToGC = true;
                    p.GuideToken.Group.GetNewToken(this);
                }
            }
        }
    }

    public void ForceSetPosition(Vector3 v)
    {
        MyRigidbody.Sleep();
        transform.position = v;
    }
    public void AddToGC()
    {
        AddedToGC = true;
        GameController.Instance.PersonManager.AddPerson(this);
    }

    public void Die()
    {
        if (AddedToGC)
        {
            AddedToGC = false;
            Dead = true;
            GameController.Instance.PersonManager.OnPersonDie(this);
            gameObject.layer = DeadPerson;
            MyCollider.enabled = false;
            MyCollider.enabled = true;
            GetComponent<DeathAudio>().playSound();
            ModelAnimator.Play("Death");
            var sd = gameObject.AddComponent<SelfDestruct>();
            sd.givenTime = 5f;
            Destroy(this);
        }
    }
}
