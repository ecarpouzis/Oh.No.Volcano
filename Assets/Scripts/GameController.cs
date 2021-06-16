using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum State { PreGame, InGame, PostGame }

    public static GameController Instance;

    public Transform CameraContainer;
    public PeopleSpawner PeopleSpawner;

    public float LavaSpeed { get; private set; }
    public bool IsPowerup { get { return SpeedPowerupDuration > 0f; } }
    public float SpeedPowerupDuration = 0f;
    public float RawMeters { get; private set; }
    public float ScoreMeters { get; private set; }
    public PersonManager PersonManager { get; private set; }
    public State CurrentState { get; private set; }
    public float PreGameCountdown { get; private set; }
    public bool LavaCatchupMode { get; set; }
    public bool StartGame = true;
    public bool TutorialMode = false;
    public bool TutorialEnd = false;

    void Awake()
    {
        LavaSpeed = 0f;
        LavaCatchupMode = false;
        Instance = this;
        PersonManager = GetComponent<PersonManager>();
        PreGameCountdown = 3f;
        int i = Leaderboards.HighScore;
    }
    private void Start()
    {
        if (!TutorialMode)
        {
            CurrentState = State.PreGame;
            InGameUI.Instance.SetState(State.PreGame);
            Objectives.ResetCounters();
        }
    }

    void Update()
    {
        if (TutorialMode)
        {
            PersonManager.GuideSpeed = 14f;

            if (LavaCatchupMode || TutorialEnd)
            {
                float spd = 12f;
                if (IsPowerup)
                    spd = 7f;
                if (LavaCatchupMode)
                    spd = 15f;
                float dz = spd * Time.deltaTime;

                Vector3 v = new Vector3(0f, 0f, dz);
                CameraContainer.position += v;
            }

            if (Input.GetButtonDown("BackButton"))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        else
        {
            if (CurrentState == State.PreGame && StartGame) { PreGameUpdate(); }
            else if (CurrentState == State.InGame) { InGameUpdate(); }
            else if (CurrentState == State.PostGame) { PostGameUpdate(); }
        }
    }

    private void PreGameUpdate()
    {
        PreGameCountdown -= Time.deltaTime;

        if (PreGameCountdown <= 0f)
        {
            CurrentState = State.InGame;
            InGameUI.Instance.SetState(State.InGame);
        }
    }
    private void InGameUpdate()
    {
        const float maxSpeedMeters = 3000f;
        const float minSpeed = 18f;
        const float maxSpeed = 35f;

        float lerp = RawMeters / maxSpeedMeters;
        if (lerp > 1f)
            lerp = 1f;

        float spd;
        if (IsPowerup)
        {
            spd = 7f;
            PersonManager.GuideSpeed = minSpeed + 4f + 3f * lerp;
            SpeedPowerupDuration -= Time.deltaTime;
        }
        else
        {
            float ds = maxSpeed - minSpeed;
            spd = minSpeed + ds * lerp;
            PersonManager.GuideSpeed = spd + 3f + 2f * lerp;
        }
        LavaSpeed = spd;
        if (LavaCatchupMode)
        {
            spd = PersonManager.GuideSpeed;
        }
        float dz = spd * Time.deltaTime;

        Vector3 v = new Vector3(0f, 0f, dz);
        CameraContainer.position += v;

        RawMeters += dz;
        ScoreMeters += PersonManager.LivingPeople * dz;

        InGameUI.Instance.SetPointsDisplay(RawMeters, ScoreMeters);
        InGameUI.Instance.SetLivingPlayers(PersonManager.LivingPeople);
    }
    private void PostGameUpdate()
    {
        if (Input.GetButtonDown("CombineGuides"))
        {
            SceneManager.LoadScene("Default");
        }
        else if (Input.GetButtonDown("BackButton"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void GameOver()
    {
        if (!TutorialMode)
        {
            CurrentState = State.PostGame;

            Leaderboards.SetMyScore((int)ScoreMeters);

            Objectives.SetMax(ObjType.ScoreMeters, (int)ScoreMeters);
            Objectives.SetMax(ObjType.RawMeters, (int)RawMeters);

            Objectives.CheckObjectives();

            int passedCt = 0;
            if (Objectives.CurrentObjectives != null)
            {
                foreach (Objective o in Objectives.CurrentObjectives)
                {
                    if (o.Complete)
                        passedCt++;
                }
            }
            if (passedCt == 3)
            {
                SceneManager.LoadScene("MainMenu");
            }

            InGameUI.Instance.SetState(State.PostGame);
        }
        else
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
}
