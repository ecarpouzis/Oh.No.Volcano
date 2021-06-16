using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public static InGameUI Instance;

    public Text CountdownText;

    public Text ScorePointsDisplay, RawPointsDisplay;
    public Text LivingPlayersDisplay;
    public Text FinalScoreDisplay, HighScoreDisplay;

    public Text PowerupTimeDisplay;
    public Image PowerupImage;

    public Graphic FadeIn;

    public GameObject PreGameGO, InGameGO, PostGameGO;

    private float MaxPowerupbarWidth;

    private bool startGame = false;

    void Awake()
    {
        Instance = this;
        MaxPowerupbarWidth = PowerupImage.rectTransform.rect.width;
        FadeIn.gameObject.SetActive(true);
        FadeIn.color = new Color(0, 0, 0, 1);
        StartCoroutine(FadeInCoroutine());
        if (GameController.Instance.StartGame)
        {
            GameController.Instance.StartGame = false;
            startGame = true;
        }
    }

    private IEnumerator FadeInCoroutine()
    {
        yield return new WaitForSeconds(1.5f);

        {
            float fr = (1.0f / Time.smoothDeltaTime); //framerate
            if (fr < 10)
                yield return new WaitForSeconds(2f);
        }
        float maxDt = 1f;
        float dt = 0f;
        while (dt < maxDt)
        {
            FadeIn.color = new Color(0, 0, 0, ((maxDt - dt) / maxDt));
            yield return new WaitForEndOfFrame();
            dt += Time.deltaTime;
        }
        FadeIn.color = new Color(0, 0, 0, 0);
        if (startGame)
            GameController.Instance.StartGame = true;
    }

    private void Update()
    {
        if (GameController.Instance.CurrentState == GameController.State.PreGame)
        {
            float time = GameController.Instance.PreGameCountdown;
            float lerp = 1f - (time - Mathf.Floor(time));
            //lerp from big to small

            CountdownText.transform.localScale = Vector3.Lerp(Vector3.one * 1.4f, Vector3.one * 1f, lerp);
            CountdownText.text = Mathf.CeilToInt(time).ToString();
        }

        if (GameController.Instance.IsPowerup)
        {
            const float maxBarSeconds = 10f;
            float time = GameController.Instance.SpeedPowerupDuration;
            float f = time / maxBarSeconds;

            if (f > 1f)
                f = 1f;
            if (f < 0f)
                f = 0f;

            float newWidth = Mathf.Lerp(-MaxPowerupbarWidth, 0f, f);
            float curWidth = PowerupImage.rectTransform.offsetMax.x;

            float realWidth = Mathf.Lerp(curWidth, newWidth, .45f);

            PowerupImage.rectTransform.offsetMax = new Vector2(realWidth, PowerupImage.rectTransform.offsetMax.y);


            int i = (int)(time * 10);
            float j = (float)i / 10f;
            PowerupTimeDisplay.text = j + "s";

            PowerupTimeDisplay.gameObject.SetActive(true);
            PowerupImage.gameObject.SetActive(true);
        }
        else
        {
            PowerupTimeDisplay.gameObject.SetActive(false);
            PowerupImage.gameObject.SetActive(false);
        }
    }

    public void SetState(GameController.State state)
    {
        PreGameGO.SetActive(false);
        InGameGO.SetActive(false);
        PostGameGO.SetActive(false);

        if (state == GameController.State.PreGame)
            PreGameGO.SetActive(true);
        else if (state == GameController.State.InGame)
            InGameGO.SetActive(true);
        else if (state == GameController.State.PostGame)
        {
            FinalScoreDisplay.text = "Score: " + ScorePointsDisplay.text;
            HighScoreDisplay.text = "High Score: " + Leaderboards.HighScore;
            PostGameGO.SetActive(true);
        }
    }

    public void SetPointsDisplay(float rawMeters, float scoreMeters)
    {
        int i = (int)(rawMeters * 10f);
        i /= 10;
        RawPointsDisplay.text = i + "m";

        i = (int)(scoreMeters * 10f);
        i /= 10;
        ScorePointsDisplay.text = "" + i;
    }
    public void SetLivingPlayers(int alive)
    {
        LivingPlayersDisplay.text = alive + " Survivors";
    }
}
