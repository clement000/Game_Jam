using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBlob_Behaviour : MonoBehaviour
{
    public float averageIdleTime = 1f;
    public float minJumpDistance = 1f;
    public float maxJumpDistance = 1f;
    GridSystem grid;

    private float jumpDuration = 0.5f;
    private float timer = 0f;
    private bool isIdle = true;
    private float idleTime;
    private float lastIdleX;
    private float lastIdleY;

    private Vector2 jumpVector = Vector2.zero;
    private float theta;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        Transform transform = gameObject.GetComponent<Transform>();
        idleTime = Random.Range(averageIdleTime - averageIdleTime / 2, averageIdleTime + averageIdleTime / 2);
        lastIdleX = transform.position.x;
        lastIdleY = transform.position.y;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isIdle)
        {
            if (timer > idleTime)//it is time to jump !
            {
                timer = 0f;
                isIdle = false;
                // Initialize the Jump                
                float privileged = PrivilegedDirection();
                theta = RandomAngle(privileged);
                //theta = Random.Range(0f, 2 * Mathf.PI);
                distance = Random.Range(minJumpDistance, maxJumpDistance);
                jumpVector.x = Mathf.Cos(theta) * distance;
                jumpVector.y = Mathf.Sin(theta) * distance;
                lastIdleX = transform.position.x;
                lastIdleY = transform.position.y;
                grid = GameObject.Find("GameSystem").GetComponent<GameSystem>().greenBlobHeatmap;
                grid.AddBlobToHeatMap(transform.position);
            }
        }
        else
        {
            if (timer > jumpDuration)//it is time to stop jumping !
            {
                timer = 0f;
                isIdle = true;
                idleTime = Random.Range(averageIdleTime - averageIdleTime / 2, averageIdleTime + averageIdleTime / 2);
                grid = GameObject.Find("GameSystem").GetComponent<GameSystem>().greenBlobHeatmap;
                grid.RemoveBlobFromHeatMap(new Vector3(lastIdleX,lastIdleY));
            }
            else
            {
                Move(jumpVector * (Time.deltaTime / jumpDuration));
            }
        }
        timer += Time.deltaTime;
    }
    void Move(Vector2 translation2)
    {
        Vector3 translation3 = translation2;
        transform.Translate(translation3);
    }

    private float PrivilegedDirection()
    {
        grid = GameObject.Find("GameSystem").GetComponent<GameSystem>().greenBlobHeatmap;

        float[] grad = new float[8];
        int x, y, indMin;
        float min ;
        grid.GetXY(transform.position, out x, out y);

        //compute the gradient in all 8 directions
        grad[0] = (grid.value(x, y + 1) - grid.value(x, y)) / grid.readCellSize();
        grad[1] = (grid.value(x + 1, y + 1) - grid.value(x, y)) / Mathf.Sqrt(2*Mathf.Pow(grid.readCellSize(),2));
        grad[2] = (grid.value(x, y + 1) - grid.value(x, y)) / grid.readCellSize();
        grad[3] = (grid.value(x - 1, y + 1) - grid.value(x, y)) / Mathf.Sqrt(2 * Mathf.Pow(grid.readCellSize(), 2));
        grad[4] = (grid.value(x - 1, y) - grid.value(x, y)) / grid.readCellSize();
        grad[5] = (grid.value(x - 1, y - 1) - grid.value(x, y)) / Mathf.Sqrt(2 * Mathf.Pow(grid.readCellSize(), 2));
        grad[6] = (grid.value(x, y - 1) - grid.value(x, y)) / grid.readCellSize();
        grad[7] = (grid.value(x + 1, y - 1) - grid.value(x, y)) / Mathf.Sqrt(2 * Mathf.Pow(grid.readCellSize(), 2));

        min = grad[0];
        indMin = 0;
        for(int i = 1; i < 8; i++)
        {
            if (grad[i] < min)
            {
                indMin = i;
                min = grad[i];
            }
        }
        float[] dir = new float[8];
        for (int i = 0; i < 8; i++)
        {
            dir[i] = i * Mathf.PI / 4;
        }
        return dir[indMin];
    }
        private float RandomAngle(float privilegedDirection)
    {
        float r = Gaussian();
        return privilegedDirection + r;
    }

    private static float Gaussian()
    {
        float v1, v2, s;
        do
        {
            v1 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            v2 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);

        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

        return v1 * s;
    }
}



