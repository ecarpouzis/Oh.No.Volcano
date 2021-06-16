using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Transform[] Checkpoints;
    public Transform EndCheckpoint;

    public Transform CameraRig { get { return gc.CameraContainer; } }

    private GameController gc;
    private PersonManager pm;
    private int chkInd = 0;
    
    private void Awake()
    {
        gc = GetComponent<GameController>();
        pm = GetComponent<PersonManager>();
    }

    void Update()
    {
        while (chkInd + 1 < Checkpoints.Length && pm.LeftGuide.position.z > Checkpoints[chkInd + 1].position.z)
        {
            chkInd++;
            if (chkInd >= Checkpoints.Length - 1)
            {
                gc.TutorialEnd = true;
                chkInd = 99999;
            }
        }

        if (chkInd < Checkpoints.Length && !gc.TutorialEnd)
        {
            Vector3 desiredPos = CameraRig.position;
            desiredPos.z = Checkpoints[chkInd].position.z - 5f;
            CameraRig.position = Vector3.MoveTowards(CameraRig.position, desiredPos, 15f * Time.deltaTime);
        }
        else if (pm.LeftGuide.position.z > EndCheckpoint.position.z)
        {
            Vector3 desiredPos = CameraRig.position;
            desiredPos.z = EndCheckpoint.position.z - 5f;
            CameraRig.position = Vector3.MoveTowards(CameraRig.position, desiredPos, 15f * Time.deltaTime);
        }
    }
}
