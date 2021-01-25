using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{

    public GridSystem greenBlobHeatmap;
    // Start is called before the first frame update
    void Start()
    {
        greenBlobHeatmap = new GridSystem(100, 100, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
