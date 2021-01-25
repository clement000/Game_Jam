using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int height;
    private float cellSize;
    private float[,] gridArray;

    public float readCellSize()
    {
        return cellSize;
    }

    public GridSystem(int width, int height, float cellSize, bool debug = false)
    {
        this.cellSize = cellSize;
        this.width = width;
        this.height = height;

        gridArray = new float[width, height];

        Debug.Log("gridarray " + width + " " + height);

        if (debug)
        {
            DrawGrid();
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x - height / 2, y - width / 2) * cellSize;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize) + height/2;
        y = Mathf.FloorToInt(worldPosition.y / cellSize) + width/2;
    }

    public void Add(int x, int y, float value, bool debug = false)
    {
        gridArray[x, y] += value;
        if (debug)
        {
            GameObject cell = GameObject.Find("World_Text" + x + "/" + y);
            TextMesh cellContent = cell.GetComponent<TextMesh>();
            cellContent.text = gridArray[x, y].ToString();
        }
    }
    public void Add(Vector3 worldPosition, float value, bool debug = false)
    {
        int x;
        int y;
        GetXY(worldPosition, out x, out y);
        gridArray[x, y] += value;
        if (debug)
        {
            GameObject cell = GameObject.Find("World_text" + x + "/" + y);
            if (cell)
            {
                TextMesh cellContent = cell.GetComponent<TextMesh>();
                cellContent.text = gridArray[x, y].ToString();
            }
            else
            {
                Debug.Log("World_text" + x + "/" + y + " does not exist");
            }
        }
    }

    public float value(int x, int y)
    {
        return gridArray[x, y];
    }
    
    public float value(Vector3 coord)
    {
        int x, y;
        GetXY(coord, out x, out y);
        return gridArray[x, y];
    }

    private void DrawCell(int x, int y)
    {
        GameObject gameObject = new GameObject("World_text" + x + "/" + y, typeof(TextMesh));
        Transform transform = gameObject.transform;
        Vector3 worldPosition = GetWorldPosition(x, y);
        worldPosition.z = 0f;
        transform.localPosition = worldPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = gridArray[x, y].ToString();
        textMesh.fontSize = 100;
        textMesh.characterSize = 0.02f;
        textMesh.color = Color.white;
        textMesh.anchor = TextAnchor.MiddleCenter;
    }
    public void DrawGrid()
    {
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    DrawCell(x,y);
                }
            }
        }
    }

    public void RemoveBlobFromHeatMap(Vector3 worldposition, bool debug = false)
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Vector3 cellPosition = GetWorldPosition(x, y);
                float value = gaussian2D(worldposition.x, worldposition.y, cellPosition.x, cellPosition.y);
                if (value > 1E-10)
                {
                    Add(x, y, -value);
                }
            }
        }
        if (debug)
        {
            Debug.Log("Value at 0,0 : " + gaussian2D(worldposition.x, worldposition.y, 0, 0));
            int x0;
            int y0;
            GetXY(new Vector3(0, 0), out x0, out y0);
            Debug.Log("value in the grid : " + gridArray[x0, y0]);
        }
    }

    public void AddBlobToHeatMap(Vector3 worldposition, bool debug = false)
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Vector3 cellPosition = GetWorldPosition(x, y);
                float value = gaussian2D(worldposition.x, worldposition.y, cellPosition.x, cellPosition.y);
                if (value > 1E-10)
                {
                    Add(x, y, value);
                }
            }
        }
        if (debug)
        {
            Debug.Log("Value at 0,0 : " + gaussian2D(worldposition.x, worldposition.y, 0, 0));
            int x0;
            int y0;
            GetXY(new Vector3(0, 0), out x0, out y0);
            Debug.Log("value in the grid : " + gridArray[x0, y0]);
        }
    }


    private float gaussian2D(float xmean, float ymean, float x, float y)
    {
        float sigma = 1;

        return (1 / sigma * Mathf.Sqrt(2 * Mathf.PI) * Mathf.Exp(-Mathf.Pow(x - xmean, 2) / (2 * Mathf.Pow(sigma, 2))) * (1 / sigma * Mathf.Sqrt(2 * Mathf.PI) * Mathf.Exp(-Mathf.Pow(y - ymean, 2) / (2 * Mathf.Pow(sigma, 2)))));
    }
}
