using System;
using System.Net;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public static class Leaderboards
{
    public static int HighScore { get; private set; }

    private static WebClient _client;

    static Leaderboards()
    {
        _client = new WebClient();

        if (SteamManager.Initialized)
        {
            HighScore = GetHighScore(SteamUser.GetSteamID().m_SteamID);
        }
        else
        {
            if (PlayerPrefs.HasKey("hs"))
                HighScore = PlayerPrefs.GetInt("hs");
            else
                HighScore = 0;
        }
    }

    public static void GetTop15(Action<LBEntry[]> OnComplete)
    {
        const string Url = "http://eric.carpouzis.com/Volcano/Home/LeaderboardGet15";
        string data = _client.DownloadString(Url);
        object o = MiniJSON.Json.Deserialize(data);
        List<object> arr = o as List<object>;
        List<LBEntry> lbEntries = new List<LBEntry>();

        if (arr != null)
        {
            foreach (object obj in arr)
            {
                const string nameK = "Username";
                const string scoreK = "Score";
                var dict = obj as Dictionary<string, object>;
                if (obj != null)
                {
                    LBEntry lbe = new LBEntry();
                    lbe.Name = (string)dict[nameK];
                    lbe.Score = (int)((long)dict[scoreK]);
                    lbEntries.Add(lbe);
                }
            }
        }
        if (OnComplete != null)
        {
            OnComplete(lbEntries.ToArray());
        }
    }

    public static void SetMyScore(int score)
    {
        if (score > HighScore)
        {
            HighScore = score;
            PlayerPrefs.SetInt("hs", score);

            if (!SteamManager.Initialized)
                return;

            string name = SteamFriends.GetPersonaName();
            ulong id = SteamUser.GetSteamID().m_SteamID;

            string Url = "http://eric.carpouzis.com/Volcano/Home/LeaderboardEntry?SteamID=" + id + "&Username=" + name + "&Score=" + score;

            _client.DownloadData(new Uri(Url));
        }
    }

    public static int GetHighScore(ulong id)
    {
        string Url = "http://eric.carpouzis.com/Volcano/Home/LeaderboardGet/" + id;
        string s = _client.DownloadString(Url);

        const string scoreK = "Score";
        object o = MiniJSON.Json.Deserialize(s);
        List<object> arr = o as List<object>;

        if (arr != null)
        {
            if (arr.Count > 0)
            {
                Dictionary<string, object> dict = arr[0] as Dictionary<string, object>;
                if (dict != null)
                {
                    return (int)((long)dict[scoreK]);
                }
            }
        }

        return 0;
    }
}

public class LBEntry
{
    public int Score;
    public string Name;
}

