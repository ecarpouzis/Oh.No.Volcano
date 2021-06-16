using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObjType { RawMeters, ScoreMeters, MaxSurvivors, CarDeath, BuildingDeath, MeteorDeath, LaserDeath, SpikeDeath, TotalNewSurvivors, SpringBounces, ButtonPresses }

public static class Objectives
{
    public static List<Objective> CurrentObjectives { get { if (CurrentStage < totalProgression.Count) return totalProgression[CurrentStage]; return null; } }

    private const string stage = "obj-stage";
    private const string sub0 = "obj-sub0";
    private const string sub1 = "obj-sub1";
    private const string sub2 = "obj-sub2";

    private static Dictionary<ObjType, int> objectiveCounters;

    public static int CurrentStage { get; private set; }
    public static int MaxStage { get { return totalProgression.Count - 1; } }

    static Objectives()
    {
        ResetCounters();
        SetObjectives();
    }
    private static void SetObjectives()
    {
        if (PlayerPrefs.HasKey(stage))
        {
            int i = PlayerPrefs.GetInt(stage);
            CurrentStage = i;
            if (i >= totalProgression.Count)
            {
                return;
            }
            else
            {
                CurrentObjectives[0].Complete = PlayerPrefs.GetString(sub0) == "true";
                CurrentObjectives[1].Complete = PlayerPrefs.GetString(sub1) == "true";
                CurrentObjectives[2].Complete = PlayerPrefs.GetString(sub2) == "true";
            }
        }
        else
        {
            CurrentStage = 0;
            PlayerPrefs.SetInt(stage, CurrentStage);
            PlayerPrefs.SetString(sub0, "false");
            PlayerPrefs.SetString(sub1, "false");
            PlayerPrefs.SetString(sub2, "false");
        }
    }

    public static void ResetCounters()
    {
        objectiveCounters = new Dictionary<ObjType, int>();
        objectiveCounters.Add(ObjType.BuildingDeath, 0);
        objectiveCounters.Add(ObjType.ButtonPresses, 0);
        objectiveCounters.Add(ObjType.CarDeath, 0);
        objectiveCounters.Add(ObjType.LaserDeath, 0);
        objectiveCounters.Add(ObjType.MaxSurvivors, 0);
        objectiveCounters.Add(ObjType.MeteorDeath, 0);
        objectiveCounters.Add(ObjType.RawMeters, 0);
        objectiveCounters.Add(ObjType.ScoreMeters, 0);
        objectiveCounters.Add(ObjType.SpringBounces, 0);
        objectiveCounters.Add(ObjType.SpikeDeath, 0);
        objectiveCounters.Add(ObjType.TotalNewSurvivors, 0);
    }

    public static void CheckObjectives()
    {
        foreach (KeyValuePair<ObjType, int> kvp in objectiveCounters)
        {
            UpdateObjective(kvp.Key, kvp.Value);
        }
    }
    public static void UpdateObjective(ObjType objType, int value)
    {
        int i = 0;
        if (CurrentObjectives != null)
            foreach (Objective o in CurrentObjectives)
            {
                if (o.Type == objType)
                {
                    if (value >= o.Val)
                    {
                        o.Complete = true;
                        PlayerPrefs.SetString("obj-sub" + i, "true");
                    }
                }
                i++;
            }
    }
    public static void SetMax(ObjType objType, int newVal) { if (newVal > objectiveCounters[objType]) objectiveCounters[objType] = newVal; }
    public static void IncrementCounter(ObjType objType) { objectiveCounters[objType]++; }
    public static int GetCounter(ObjType objType) { return objectiveCounters[objType]; }

    public static void UnlockNewObjectives()
    {
        CurrentStage++;
        PlayerPrefs.SetInt(stage, CurrentStage);
        PlayerPrefs.SetString(sub0, "false");
        PlayerPrefs.SetString(sub1, "false");
        PlayerPrefs.SetString(sub2, "false");
        SetObjectives();
    }

    private static List<List<Objective>> totalProgression = new List<List<Objective>>()
    {
        new List<Objective>() { new Objective(ObjType.RawMeters, 500), new Objective(ObjType.ScoreMeters, 10000), new Objective(ObjType.TotalNewSurvivors, 10) },
        new List<Objective>() { new Objective(ObjType.SpringBounces, 5), new Objective(ObjType.ButtonPresses, 3), new Objective(ObjType.MeteorDeath, 20) },
        new List<Objective>() { new Objective(ObjType.BuildingDeath, 20), new Objective(ObjType.RawMeters, 1000), new Objective(ObjType.MaxSurvivors, 40) },
        new List<Objective>() { new Objective(ObjType.CarDeath, 20), new Objective(ObjType.LaserDeath, 20), new Objective(ObjType.TotalNewSurvivors, 20) },
        new List<Objective>() { new Objective(ObjType.ScoreMeters, 30000), new Objective(ObjType.SpikeDeath, 20), new Objective(ObjType.SpringBounces, 10) },
        new List<Objective>() { new Objective(ObjType.MeteorDeath, 40), new Objective(ObjType.MaxSurvivors, 45), new Objective(ObjType.ScoreMeters, 40000) },
        new List<Objective>() { new Objective(ObjType.BuildingDeath, 40), new Objective(ObjType.ButtonPresses, 5), new Objective(ObjType.RawMeters, 2000) },
        new List<Objective>() { new Objective(ObjType.CarDeath, 40), new Objective(ObjType.LaserDeath, 40), new Objective(ObjType.TotalNewSurvivors, 30) },
        new List<Objective>() { new Objective(ObjType.SpikeDeath, 40), new Objective(ObjType.MaxSurvivors, 50), new Objective(ObjType.RawMeters, 3000) },
        new List<Objective>() { new Objective(ObjType.ScoreMeters, 60000), new Objective(ObjType.TotalNewSurvivors, 40), new Objective(ObjType.SpringBounces, 30) },
    };
}
public class Objective
{
    public ObjType Type;
    public int Val;
    public bool Complete;

    public string Description = "";

    public Objective(ObjType t, int valRequirement)
    {
        Type = t;
        Val = valRequirement;
        Complete = false;
        Description = "Invalid Description";

        switch (t)
        {
            case ObjType.BuildingDeath:
                Description = "- Crush " + Val + " survivors with buildings in a single run";
                break;
            case ObjType.ButtonPresses:
                Description = "- Press " + Val + " buttons in a single run";
                break;
            case ObjType.CarDeath:
                Description = "- Run over " + Val + " survivors with cars in a single run";
                break;
            case ObjType.LaserDeath:
                Description = "- Lose " + Val + " survivors to laser beams in a single run";
                break;
            case ObjType.MaxSurvivors:
                Description = "- Get " + Val + " survivors in your entire group";
                break;
            case ObjType.MeteorDeath:
                Description = "- Smash " + Val + " survivors with meteors in a single run";
                break;
            case ObjType.RawMeters:
                Description = "- Outrun the lava for  " + Val + "m in a single run";
                break;
            case ObjType.ScoreMeters:
                Description = "- Reach a score of " + Val;
                break;
            case ObjType.SpikeDeath:
                Description = "- Impale " + Val + " survivors with spikes in a single run";
                break;
            case ObjType.SpringBounces:
                Description = "- Hit " + Val + " springs in a single run";
                break;
            case ObjType.TotalNewSurvivors:
                Description = "- Gain " + Val + " new survivors in a single run";
                break;
        }
    }
}
