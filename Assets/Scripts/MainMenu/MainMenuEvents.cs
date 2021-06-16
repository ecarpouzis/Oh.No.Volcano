using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuEvents : MonoBehaviour
{
    public static MainMenuEvents Instance;

    public Text[] Buttons;

    public IntroCamera IntroCamera;
    public MainMenuPerson MainMenuPerson;
    public Text SteamActiveText;
    public GameObject ProfileContainer;

    public Image SteamIcon;
    public Text NameText;

    public Text ObjectivesText;
    public Text Objective1, Objective2, Objective3;
    public Color CompletedObjectiveColor;

    private int index = 0;
    private bool moved = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (SteamManager.Initialized)
        {
            SteamActiveText.gameObject.SetActive(false);
            ProfileContainer.SetActive(true);

            Texture2D tex = SteamIcons.GetMySteamIcon();
            Rect rec = new Rect(0, 0, tex.width, tex.height);
            SteamIcon.rectTransform.sizeDelta = new Vector2(0f, SteamIcon.rectTransform.rect.width);
            SteamIcon.sprite = Sprite.Create(tex, rec, new Vector2(0.5f, 0.5f), 100);
            NameText.text = Steamworks.SteamFriends.GetPersonaName();
        }
        else
        {
            SteamActiveText.gameObject.SetActive(true);
            ProfileContainer.SetActive(false);
        }

        bool success = SetObjectives();
        if (!success)
        {
            MainMenuPerson.UpgradeModel = true;
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector2 LeftStick = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        LeftStick.x = 0f;
        if (LeftStick.y < -.4f)
        {
            if (!moved)
            {
                if (index < Buttons.Length - 1)
                {
                    Buttons[index].color = Color.white;
                    index++;
                    Buttons[index].color = Color.red;
                    moved = true;
                }
            }
        }
        else if (LeftStick.y > .4f)
        {
            if (!moved)
            {
                if (index > 0)
                {
                    Buttons[index].color = Color.white;
                    index--;
                    Buttons[index].color = Color.red;
                    moved = true;
                }
            }
        }
        else
        {
            moved = false;
        }

        if (Input.GetButtonDown("CombineGuides"))
        {
            switch (index)
            {
                case 0:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Default");
                    break;
                case 1:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
                    break;
                case 2:
                    IntroCamera.playIntro = true;
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                    break;
                case 3:
                    Application.Quit();
                    break;
            }
        }
    }

    public bool SetObjectives()
    {
        int stage = Objectives.CurrentStage;
        if (stage > Objectives.MaxStage)
        {
            ObjectivesText.text = "You've completed all objectives, great job!";
            Objective1.text = "";
            Objective2.text = "";
            Objective3.text = "";
        }
        else
        {
            ObjectivesText.text = "";
            Objective1.text = "";
            Objective2.text = "";
            Objective3.text = "";

            int numLeft = 0;
            List<Objective> objs = Objectives.CurrentObjectives;

            if (objs == null)
                return true;

            if (objs.Count == 3)
            {
                bool allComplete = true;
                for (int i = 0; i < 3; i++)
                {
                    if (!objs[i].Complete)
                        allComplete = false;
                }
                if (allComplete)
                {
                    Objectives.UnlockNewObjectives();
                    return false;
                }

                for (int i = 0; i < objs.Count; i++)
                {
                    if (!objs[i].Complete)
                        numLeft++;
                }


                Objective1.text = objs[0].Description;
                if (objs[0].Complete)
                {
                    Objective1.color = CompletedObjectiveColor;
                }
                else
                {
                    Objective1.color = Color.white;
                }

                Objective2.text = objs[1].Description;
                if (objs[1].Complete)
                {
                    Objective2.color = CompletedObjectiveColor;
                }
                else
                {
                    Objective2.color = Color.white;
                }


                Objective3.text = objs[2].Description;
                if (objs[2].Complete)
                {
                    Objective3.color = CompletedObjectiveColor;
                }
                else
                {
                    Objective3.color = Color.white;
                }


                GameObject next = MainMenuPerson.NextModel;
                if (next != null)
                {
                    string rewardName = next.name;
                    ObjectivesText.text = "Complete " + numLeft + " more objectives to unlock " + rewardName;
                }
                else
                {
                    ObjectivesText.text = "";
                }
            }
            else
            {
                Debug.LogError("Invalid objectives count: " + objs.Count);
            }
        }
        return true;
    }
}
