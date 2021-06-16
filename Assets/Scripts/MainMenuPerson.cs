using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPerson : MonoBehaviour
{
    private Transform[] models;

    public Transform BottomPosition;
    public Transform TopPosition;

    public GameObject CurrentModel { get; private set; }
    public GameObject NextModel { get; private set; }
    public bool UpgradeModel = false;
    public UnityEngine.UI.Text ModelName;
    public Transform MoveTarget;
    private Screenshake Screenshake;

    private void Awake()
    {
        UpgradeModel = false;
        Screenshake = gameObject.GetComponent<Screenshake>();
        models = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            models[i] = transform.GetChild(i);
        }

        ResetModel();
        MoveTarget.position = BottomPosition.position;
    }

    public void ResetModel()
    {
        int baseUnlockedMaxIndex = models.Length - Objectives.MaxStage - 2; //index of all runners you get by default
        int highestUnlockedModelIndex = baseUnlockedMaxIndex + Objectives.CurrentStage; //highest index of any model we have unlocked

        CurrentModel = null;
        foreach (Transform t in models)
        {
            t.gameObject.SetActive(false);
        }

        if (highestUnlockedModelIndex > baseUnlockedMaxIndex)
        {
            CurrentModel = models[highestUnlockedModelIndex].gameObject;
            int next = highestUnlockedModelIndex + 1;
            if (next < models.Length)
            {
                NextModel = models[next].gameObject;
            }
            else
            {
                NextModel = null;
            }
            CurrentModel.SetActive(true);
        }
        else
        {
            NextModel = models[baseUnlockedMaxIndex + 1].gameObject;
        }


        if (CurrentModel != null)
            ModelName.text = CurrentModel.name;
        else
            ModelName.text = "";
    }

    public void RaisePerson() { StartCoroutine(MovePerson(TopPosition.position)); }
    public void LowerPerson() { StartCoroutine(MovePerson(BottomPosition.position)); }

    private IEnumerator MovePerson(Vector3 dest)
    {
        Vector3 start = MoveTarget.position;
        float maxDt = 4f;
        float dt = 0f;

        if (CurrentModel != null)
        {
            Screenshake.shakeDuration = maxDt;

            while (dt < maxDt)
            {
                MoveTarget.position = Vector3.Lerp(start, dest, dt / maxDt);
                yield return new WaitForEndOfFrame();
                dt += Time.deltaTime;
            }
        }
        else
        {
            MoveTarget.position = dest;
        }

        if (UpgradeModel && dest == TopPosition.position)
        {
            yield return new WaitForSeconds(1f);
            LowerPerson();
        }
        else if (UpgradeModel && dest == BottomPosition.position)
        {
            ResetModel();
            RaisePerson();
            UpgradeModel = false;
            MainMenuEvents.Instance.SetObjectives();
        }
    }
}
