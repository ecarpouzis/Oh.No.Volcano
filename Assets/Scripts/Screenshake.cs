using UnityEngine;
using System.Collections;

public class Screenshake : MonoBehaviour
{
    // How long the object should shake for.
    public float shakeDuration = 1f;
    
    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.55f;
    float decreaseFactor = 1.0f;

    Vector3 originalPos;
    
    private int destroyContainer = 0;
    
    public bool AutoDestroy = true;

    private Transform ShakeContainer;

    void Awake()
    {
        ShakeContainer = GameObject.Find("ScreenshakeContainer").transform;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            ShakeContainer.localPosition = Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            if (AutoDestroy)
            {
                destroyContainer = Time.frameCount + 5;
            }
        }

        if (Time.frameCount == destroyContainer)
        {
            if (AutoDestroy)
            {
                Destroy(this);
            }
        }
    }
}

