using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tile : MonoBehaviour
{
    public int tileLength;
    public GameObject TileLight;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name != "Default")
        {
            return;
        }

        gameObject.name = "Tile1";
        Worldbuilder.Instance.AddTile(this);

        if (Worldbuilder.Instance != null)
        {
            Destroy(TileLight);
        }
    }
}