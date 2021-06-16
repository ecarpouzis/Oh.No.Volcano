using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilegen : MonoBehaviour
{
    public List<GameObject> floorTiles = new List<GameObject>();
    int groundTileLength = 21;

    // Use this for initialization
    void Start()
    {
        GenerateRandomFloor();
    }

    private void GenerateFlat()
    {
        Tile t = GetNewTile();
        for (int i = 0; i < 5; i++)
        {
            Instantiate(floorTiles[0], new Vector3(0f, 0f, i * groundTileLength), Quaternion.identity, t.transform);
            t.tileLength += groundTileLength;
        }
    }
    private void GenerateRandomFloor()
    {
        Tile t = GetNewTile();
        for (int i = 0; i < 5; i++)
        {
            int isFlat = Random.Range(0, 3);
            //0 = flat
            int tileToSpawn = 0;
            if (isFlat < 2)
            {
                int tileDifficulty = Random.Range(0, 10);
                if (tileDifficulty < 5)
                {
                    //1 = single hole; 2 = double hole
                    tileToSpawn = Random.Range(1, 3);
                }
                else if (tileDifficulty < 8)
                {
                    //3 = big hole; 4 = narrow
                    tileToSpawn = Random.Range(3, 5);
                }
                else
                {
                    //5 = full break
                    tileToSpawn = 5;
                }
            }

            int isBackwards = Random.Range(0, 2);
            if (isBackwards == 1)
            {
                Quaternion backwards = Quaternion.Euler(0f, 180f, 0f);
                Instantiate(floorTiles[tileToSpawn], new Vector3(0f, 0f, i * groundTileLength), backwards, t.transform);
                t.tileLength += groundTileLength;
            }
            else
            {
                Instantiate(floorTiles[tileToSpawn], new Vector3(0f, 0f, i * groundTileLength), Quaternion.identity, t.transform);
                t.tileLength += groundTileLength;
            }
        }
    }

    private Tile GetNewTile()
    {
        GameObject go = new GameObject();
        Tile t = go.AddComponent<Tile>();
        go.name = "Tile";

        t.TileLight = GameObject.Find("TileLight");
        t.TileLight.transform.parent = go.transform;
        return t;
    }
}
