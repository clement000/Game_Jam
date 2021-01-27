using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobPusher : MonoBehaviour
{
    bool init = false;
    // Start is called before the first frame update
    void Awake()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (!init)
        {
            GridSystem grid = GameObject.Find("GameSystem").GetComponent<GameSystem>().greenBlobHeatmap;
            grid.AddBlobToHeatMap(transform.position, 4);
            Debug.Log("Added a BlobPusher !");
            init = true;
        }
    }
}
