using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceLab : MonoBehaviour
{
    public GameObject BulletPrefab;

    public Transform DomeRot1, DomeRot2;
    public Transform Dome;
    public Transform LaserOrigin;

    void Start()
    {
        StartCoroutine(MoveLaser());
        StartCoroutine(ShootLaser());
    }

    IEnumerator ShootLaser()
    {
        while (true)
        {
            while (GameController.Instance.CurrentState != GameController.State.InGame && !GameController.Instance.TutorialMode)
                yield return null;

            float delay = Random.Range(.07f, .2f);
            int max = Random.Range(4, 8);
            for (int i = 0; i < max; i++)
            {
                Shoot();
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitForSeconds(Random.Range(.3f, .7f));
        }
    }

    IEnumerator MoveLaser()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(.5f, 1.5f));

            const float moveTime = .3f;

            Quaternion startRot = Dome.localRotation;
            Quaternion desiredRot = Quaternion.Slerp(DomeRot1.localRotation, DomeRot2.localRotation, Random.Range(0f, 1f));
            float dt = 0;
            while (dt < moveTime)
            {
                yield return new WaitForEndOfFrame();
                dt += Time.deltaTime;
                Dome.localRotation = Quaternion.Slerp(startRot, desiredRot, dt / moveTime);
            }
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab, LaserOrigin.position, LaserOrigin.rotation);
        LaserBullet lb = bullet.GetComponent<LaserBullet>();
        lb.MyRigidbody.velocity = LaserOrigin.forward * -40f;
    }
}
