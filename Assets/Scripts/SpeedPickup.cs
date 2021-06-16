using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPickup : MonoBehaviour
{
    public float Duration = 5f;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!activated)
        {
            if (other.gameObject.IsPerson())
            {
                activated = true;
                GameController.Instance.SpeedPowerupDuration += Duration;
                Destroy(gameObject);
            }
        }
    }
}
