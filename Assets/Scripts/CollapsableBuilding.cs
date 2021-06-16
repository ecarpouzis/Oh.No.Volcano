using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsableBuilding : MonoBehaviour
{
    public Rigidbody Building;
    public Transform BuildingUp, BuildingDown;
    public Collider CrushTrigger;

    public AnimationCurve aCurve;

    private bool fell = false;

    void Awake()
    {
        Building.MovePosition(BuildingUp.position);
        Building.Sleep();
        CrushTrigger.enabled = false;

        Building.MovePosition(BuildingUp.position);
        Building.MoveRotation(BuildingUp.rotation);
        Building.Sleep();
    }

    void Update()
    {
        if (GameController.Instance.CurrentState == GameController.State.InGame && !fell)
        {
            Vector3 cameraPosition = GameController.Instance.PersonManager.LeftGuide.position;
            cameraPosition.y = 0;
            cameraPosition.x = 0;
            Vector3 myAdjustedPosition = transform.position;
            myAdjustedPosition.y = 0;
            myAdjustedPosition.x = 0;
            if (Vector3.Distance(cameraPosition, myAdjustedPosition) < 20f)
            {
                StartCoroutine(TriggerFall());
            }
        }
    }

    private IEnumerator TriggerFall()
    {
        fell = true;

        float dt = 0f;
        while (dt < 1f)
        {
            float lerp = aCurve.Evaluate(dt);
            

            if (lerp > .15f)
                CrushTrigger.enabled = true;

            Vector3 newPos = Vector3.Lerp(BuildingUp.position, BuildingDown.position, lerp);
            Quaternion newRot = Quaternion.Slerp(BuildingUp.rotation, BuildingDown.rotation, lerp);

            Building.MovePosition(newPos);
            Building.MoveRotation(newRot);
            yield return new WaitForFixedUpdate();
            dt += Time.deltaTime * .7f;
        }

        CrushTrigger.enabled = false;
    }
}
