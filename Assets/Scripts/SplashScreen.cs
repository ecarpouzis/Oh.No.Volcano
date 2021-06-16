using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    public Image FadeOut;
    public GameObject everythingElse;

    IEnumerator Start()
    {
        Cursor.visible = false;
        everythingElse.SetActive(true);
        FadeOut.gameObject.SetActive(true);
        FadeOut.color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(2f);
        float maxDt = 1f;
        float dt = 0;
        while (dt < maxDt)
        {
            FadeOut.color = new Color(0, 0, 0, dt / maxDt);
            yield return new WaitForEndOfFrame();
            dt += Time.deltaTime;
        }
        FadeOut.color = new Color(0, 0, 0, 1);
        everythingElse.SetActive(false);
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        if (Input.GetButtonDown("CombineGuides"))
            SceneManager.LoadScene("MainMenu");
    }
}
