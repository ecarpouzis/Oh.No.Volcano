using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLeaderboard : MonoBehaviour
{
    public GameObject EntryPrefab;
    public Text RefreshText;

    private const int MaxEntries = 15;
    private List<MenuLeaderboardEntry> Entries;

    void Update()
    {
        OnEnable();
    }

    private void Awake()
    {
        RefreshText.gameObject.SetActive(true);

        Entries = new List<MenuLeaderboardEntry>();

        for (int i = 0; i < MaxEntries; i++)
        {
            GameObject g = (GameObject)Instantiate(EntryPrefab, transform);
            g.transform.parent = this.transform;
            RectTransform t = g.GetComponent<RectTransform>();

            MenuLeaderboardEntry lbe = g.GetComponent<MenuLeaderboardEntry>();
            lbe.NameText.text = "Kildon";
            lbe.RankText.text = (i + 1) + ".";
            lbe.ScoreText.text = "The worst";
            Entries.Add(lbe);
        }

        Refresh();
    }

    void OnEnable()
    {
        float height = 0f;
        foreach (MenuLeaderboardEntry entry in Entries)
        {
            var t = entry.GetComponent<RectTransform>();
            t.offsetMin = Vector2.zero;
            t.offsetMax = Vector2.zero;

            Vector3 v = t.position;
            v.y -= height;
            t.position = v;
            height += t.rect.height;
        }
    }

    public void Refresh()
    {
        foreach (MenuLeaderboardEntry entry in Entries)
        {
            entry.gameObject.SetActive(false);
        }

        RefreshText.gameObject.SetActive(true);
        Leaderboards.GetTop15((arr) =>
        {
            for (int i = 0; i < MaxEntries && i < arr.Length; i++)
            {
                LBEntry lbe = arr[i];
                Entries[i].NameText.text = lbe.Name;
                Entries[i].ScoreText.text = lbe.Score.ToString();
                Entries[i].gameObject.SetActive(true);
            }
            RefreshText.gameObject.SetActive(false);
        });
    }
}
