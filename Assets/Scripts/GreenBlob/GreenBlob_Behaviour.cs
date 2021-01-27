using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBlob_Behaviour : MonoBehaviour
{
    public float averageIdleTime = 1f;
    public float minJumpDistance = 1f;
    public float maxJumpDistance = 1f;
    public float averageSplitTime = 30f;
    public float minimalDensityToSplit = 15f;
    GridSystem grid;

    public bool isBeeingJumpedOn = false;

    private float jumpDuration = 0.5f;
    private float jumpTimer = 0f;
    private float splitTimer = 0f;
    private bool isIdle = true;
    private bool canSplit = false;
    private float idleTime;
    private float lastIdleX;
    private float lastIdleY;
    private float splitTime;

    private Vector2 jumpVector = Vector2.zero;
    private float theta;
    private float distance;

    private bool blobWasInit = false;

    private Rigidbody2D GreenBlob;

    // Start is called before the first frame update
    void Start()
    {
        jumpTimer = jumpDuration;
        GreenBlob = GetComponent<Rigidbody2D>();
        Transform transform = gameObject.GetComponent<Transform>();
        idleTime = Random.Range(averageIdleTime - averageIdleTime / 2, averageIdleTime + averageIdleTime / 2);
        lastIdleX = transform.position.x;
        lastIdleY = transform.position.y;
        splitTime = Random.Range(1.5f, 1) * averageSplitTime;
    }

    // Update is called once per frame
    void Update()
    {   if (!isBeeingJumpedOn)
        {
            grid = GameObject.Find("GameSystem").GetComponent<GameSystem>().greenBlobHeatmap;
            if (isIdle)
            {
                if (jumpTimer > idleTime)//it is time to jump !
                {
                    jumpTimer = 0f;
                    isIdle = false;
                    // Initialize the Jump                
                    float privileged = PrivilegedDirection();
                    theta = RandomAngle(privileged);
                    //theta = Random.Range(0f, 2 * Mathf.PI);
                    distance = Random.Range(minJumpDistance, maxJumpDistance);
                    jumpVector.x = Mathf.Cos(theta) * distance / jumpDuration;
                    jumpVector.y = Mathf.Sin(theta) * distance / jumpDuration;
                    lastIdleX = transform.position.x;
                    lastIdleY = transform.position.y;

                }
            }
            else
            {
                if (jumpTimer > jumpDuration)//it is time to stop jumping !
                {
                    jumpTimer = 0f;
                    isIdle = true;
                    idleTime = Random.Range(averageIdleTime - averageIdleTime / 2, averageIdleTime + averageIdleTime / 2);
                    if (blobWasInit)
                    {
                        grid.RemoveBlobFromHeatMap(new Vector3(lastIdleX, lastIdleY));
                    }
                    grid.AddBlobToHeatMap(transform.position);
                    Move(new Vector2(0, 0));
                    blobWasInit = true;
                }
                else
                {
                    Move(jumpVector);
                }
            }
            int xBlob, yBlob;
            grid.GetXY(transform.position, out xBlob, out yBlob);
            if (grid.value(xBlob, yBlob) < 8)
            {
                if (canSplit)
                {
                    Split();
                    canSplit = false;
                    splitTimer = 0f;
                    splitTime = Random.Range(1.5f, 1) * averageSplitTime;
                }
                else
                {
                    if (splitTimer > splitTime)
                    {
                        canSplit = true;
                    }
                }
                splitTimer += Time.deltaTime;
            }
            jumpTimer += Time.deltaTime;
        }
        
    }
    void Move(Vector2 translation2)
    {
        Vector2 velocity = translation2;
        GreenBlob.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpVector = -1 * jumpVector;
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
    private void Split()
    {
        Debug.Log("split !");
        Instantiate(GreenBlob);
    }
}



