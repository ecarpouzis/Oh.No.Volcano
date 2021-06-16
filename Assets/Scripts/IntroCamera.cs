using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCamera : MonoBehaviour
{
    public Material daySky;
    public Material nightSky;
    Material skybox;
    float introTimer = 0f;
    public MainMenuEvents GameUI;
    public static bool playIntro = true;

    public List<Camera> cameras = new List<Camera>();
    public GameObject noLava;
    public GameObject lava;
    public GameObject introBus;
    public GameObject busBegin;
    public GameObject busEnd;
    public SoundSubClip IntroAudio1;
    public SoundSubClip IntroAudio2;
    public SoundSubClip IntroAudio3;

    float timeToFirstCamera = 3f;
    float timeToSecondCamera = 6f;
    float timeToThirdCamera = 9f;
    float timeToFourthCamera = 15f;

    private Coroutine introToken;

    private void Awake()
    {
        MainMenuEvents.Instance = GameUI;
    }

    // Use this for initialization
    void Start()
    {
        if (playIntro)
        {
            playIntro = false;
            StartIntro();
        }
        else
        {
            StopIntro();
        }
    }

    void SetSkybox(Camera camera, Material material)
    {
        Skybox skybox = camera.GetComponent<Skybox>();
        if (skybox == null)
            skybox = camera.gameObject.AddComponent<Skybox>();
        skybox.material = material;
    }

    public void StartIntro()
    {
        lava.SetActive(false);
        noLava.SetActive(true);
        cameras[0].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(false);
        cameras[2].gameObject.SetActive(false);
        cameras[3].gameObject.SetActive(false);
        introBus.transform.position = busBegin.transform.position;
        GameUI.gameObject.SetActive(true);
        GameUI.gameObject.SetActive(false);
        introToken = StartCoroutine(IntroCoroutine());
    }

    public void StopIntro()
    {
        if (introToken != null)
        {
            StopCoroutine(introToken);
            introToken = null;
        }
        lava.SetActive(true);
        noLava.SetActive(false);
        IntroAudio1.Stop();
        IntroAudio2.Stop();
        IntroAudio3.Stop();
        cameras[0].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(false);
        cameras[2].gameObject.SetActive(false);
        cameras[3].gameObject.SetActive(true);
        GameUI.gameObject.SetActive(true);

        GameUI.MainMenuPerson.transform.position = GameUI.MainMenuPerson.BottomPosition.position;
        GameUI.MainMenuPerson.RaisePerson();
    }

    IEnumerator IntroCoroutine()
    {
        IntroAudio1.Play(.2f, 2.4f);
        SetSkybox(cameras[0], daySky);
        SetSkybox(cameras[1], daySky);
        SetSkybox(cameras[2], daySky);
        cameras[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);

        IntroAudio1.Stop();
        IntroAudio2.Play(2.6f, 5f);
        cameras[0].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);

        IntroAudio2.Stop();
        IntroAudio3.Play(0f, 3.2f);
        cameras[1].gameObject.SetActive(false);
        cameras[2].gameObject.SetActive(true);
        SetSkybox(cameras[3], nightSky);
        yield return new WaitForSeconds(4f);
        
        cameras[2].gameObject.SetActive(false);
        cameras[3].gameObject.SetActive(true);

        StopIntro();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("CombineGuides"))
        {
            StopIntro();
        }

        if (introBus.transform.position.x < busEnd.transform.position.x)
        {
            Vector3 newBusPos = introBus.transform.position;
            newBusPos.x += Time.deltaTime * 7f;
            introBus.transform.position = newBusPos;
        }
    }
}
