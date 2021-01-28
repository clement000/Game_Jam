using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBlob_Behaviour : MonoBehaviour
{
    public float averageIdleTime = 1f;
    public float minJumpDistance = 1f;
    public float maxJumpDistance = 1f;
    public float averageSplitTime = 30f;
    public float minimalDensityToSplit = 8f;
    public float health = 10f;
    GridSystem grid;

    public bool isBeeingJumpedOn = false;

    private float jumpDuration = 0.5f;
    private float jumpTimer = 0f;
    private float splitTimer = 0f;
    private float jumpedTimer = 0f;
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

    private Animator anim;

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
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBeeingJumpedOn)
        {
            grid = GameObject.Find("GameSystem").GetComponent<GameSystem>().greenBlobHeatmap;
            if (isIdle)
            {
                if (jumpTimer > idleTime)//it is time to jump !
                {
                    jumpTimer = 0f;
                    isIdle = false;
                    // Initialize the Jump
                    jumpVector = JumpInit();
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
                    idleTime = Random.Range(averageIdleTime * 0.5f, averageIdleTime * 1.5f);
                    if (blobWasInit)//only remove the blob if he was already added (this should be in start, but for some reason it does not work when put there...)
                    {
                        grid.RemoveBlobFromHeatMap(new Vector3(lastIdleX, lastIdleY));
                    }
                    grid.AddBlobToHeatMap(transform.position);
                    Stop();
                    blobWasInit = true;//if the blob was not yet added, it now is
                }
                else//we are in the middle of a jump
                {
                    Move(jumpVector);
                }
            }
            int xBlob, yBlob;
            grid.GetXY(transform.position, out xBlob, out yBlob);
            if (grid.value(xBlob, yBlob) < minimalDensityToSplit)
            {
                if (canSplit)
                {
                    Split();
                    canSplit = false;
                    splitTimer = 0f;
                    splitTime = Random.Range(0.8f, 1.2f) * averageSplitTime;
                }
                else
                {
                    if (splitTimer > splitTime)
                    {
                        canSplit = true;
                    }
                }
                splitTimer += Time.deltaTime;//only increment the split timer if the blob is in a situation where he could split
            }
            jumpTimer += Time.deltaTime;
        }
        else
        {
            Stop();
            if (jumpedTimer > 3 * averageIdleTime)
            {
                isBeeingJumpedOn = false;
            }
            jumpedTimer += Time.deltaTime;
        }
    }
    void Move(Vector2 translation2)
    {
        Vector2 velocity = translation2;
        GreenBlob.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)//when colliding with a wall, bounce
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
        anim.SetTrigger("divise");
        Invoke("EndSplit", 1.2f);
    }
    public void Jumped()
    {
        isBeeingJumpedOn = true;
        jumpedTimer = 0f;
    }

    private void Stop()
    {
        Move(new Vector2(0, 0));
    }

    private Vector2 JumpInit()
    {
        float privileged = PrivilegedDirection();
        theta = RandomAngle(privileged);
        distance = Random.Range(minJumpDistance, maxJumpDistance);
        jumpVector.x = Mathf.Cos(theta) * distance / jumpDuration;
        jumpVector.y = Mathf.Sin(theta) * distance / jumpDuration;
        return jumpVector;
    }

    public void RemoveBlob()//just removes the blob from the game
    {
        grid = GameObject.Find("GameSystem").GetComponent<GameSystem>().greenBlobHeatmap;
        grid.RemoveBlobFromHeatMap(new Vector3(lastIdleX, lastIdleY));
        Destroy(GreenBlob.gameObject);
    }

    public void DamageBlob(float amount)
    {
        health -= amount;
        if (health < 0)
        {
            Kill();
        }
    }

    public void Kill()//kills the blob (with death animation)
    {
        grid = GameObject.Find("GameSystem").GetComponent<GameSystem>().greenBlobHeatmap;
        grid.RemoveBlobFromHeatMap(new Vector3(lastIdleX, lastIdleY));
        Destroy(GreenBlob.gameObject);
    }

    void EndSplit()
    {
        GameObject newGreenBlob = Instantiate(GreenBlob.gameObject);
        Vector3 offset = new Vector3(0.1f, 0);
        newGreenBlob.transform.position = transform.position + offset;
        transform.position -= offset;
        jumpTimer = -2f;
        newGreenBlob.GetComponent<GreenBlob_Behaviour>().jumpTimer = -2f;
    }
}
