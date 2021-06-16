using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Worldbuilder : MonoBehaviour
{
    public static Worldbuilder Instance;
    public string testingTile;

    public string RunwayTileSceneName = "RunwayTile";

    public List<string> sceneList = new List<string>();

    private List<Tile> tileList = new List<Tile>();
    private float worldLength = 0f;

    private const int minTileRepeat = 5;
    private List<int> recentTiles = new List<int>();
    void Awake()
    {
        Instance = this;
        CreateTile(RunwayTileSceneName);
        if (testingTile != "")
        {
            CreateTile(testingTile); CreateTile(testingTile); CreateTile(testingTile); CreateTile(testingTile); CreateTile(testingTile); CreateTile(testingTile); CreateTile(testingTile); CreateTile(testingTile); CreateTile(testingTile); CreateTile(testingTile); CreateTile(testingTile); CreateTile(testingTile); CreateTile(testingTile);
        }
    }

    //Called by Tile in awake since loaded scenes aren't available in the same frame they're created
    public void AddTile(Tile givenTile)
    {
        tileList.Add(givenTile);
        givenTile.transform.position = new Vector3(0f, 0f, worldLength);
        worldLength += givenTile.tileLength;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.CameraContainer.transform.position.z > worldLength - 400f)
        {
            int index = Random.Range(0, sceneList.Count);
            while (recentTiles.Contains(index))
            {
                index = Random.Range(0, sceneList.Count);
            }
            if (recentTiles.Count == minTileRepeat)
            {
                recentTiles.RemoveAt(0);
            }
            recentTiles.Add(index);
            string s = sceneList[index];
            CreateTile(s);
        }
        if (tileList.Count > 0)
        {
            Tile t = tileList[0];
            float tileBackZ = t.transform.position.z + t.tileLength;
            if (tileBackZ + 30f < GameController.Instance.CameraContainer.transform.position.z)
            {
                tileList.Remove(t);
                Destroy(t.gameObject);
            }
        }
    }

    private void CreateTile(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
}
