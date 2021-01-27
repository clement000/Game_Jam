using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBlob_Behaviour : MonoBehaviour
{
    public float detectionRadius = 5f;
    public float averageIdleTime = 1f;
    public float minJumpDistance = 1f;
    public float maxJumpDistance = 1f;

    private float jumpDuration = 0.5f;

    private Collider2D[] possibleTargets;
    private float minDist, dist;
    private GameObject target, selectedTarget;

    private bool isIdle = true;
    private bool isChasing = false;
    private float timer = 0f;
    private float idleTime, jumpDistance, jumpTheta, targetDistance;
    Vector3 jumpVector;
    RaycastHit2D hit;
    private Rigidbody2D redBlob;
    // Start is called before the first frame update
    void Start()
    {
        idleTime = Random.Range(0.5f, 1.5f) * averageIdleTime;
        redBlob = GetComponent<Rigidbody2D>();
        redBlob.velocity = new Vector2(0,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isIdle)
        {
            target = LookForTarget();
            if (target != null)
            {
                idleTime = averageIdleTime / 2;
            }
            if (timer > idleTime)
            {
                Debug.Log("Time to jump !");
                isIdle = false;
                timer = 0f;
                if (target != null)
                {
                    Debug.Log("Found a target ! " + target.name);
                    //Check if the target can be reached in 1 jump (it is in reach, and there is no obstacle between the two)
                    targetDistance = Vector3.Distance(target.transform.position, transform.position);
                    hit = Physics2D.Raycast(transform.position, target.transform.position - transform.position, targetDistance);
                    if (true)//targetDistance < maxJumpDistance && hit.transform.position == target.transform.position)
                    {
                        Debug.Log("Chasing " + target.name);
                        Debug.Log("Chasing /" + target.tag + "/");
                        isChasing = true;
                        if (target.tag == "GreenBlob")
                        {
                            Debug.Log(target.name + " is a GreenBlob");
                            target.GetComponent<GreenBlob_Behaviour>().Jumped();
                            //initiate a jump that will land right next to the target
                            jumpVector = (target.transform.position - transform.position) / jumpDuration;
                        }
                        else//target is the player : jump towards him
                        {
                            jumpVector = (target.transform.position - transform.position) * jumpDistance / jumpDuration;
                        }
                    }
                }
                else
                {
                    // Initialize a random Jump                
                    jumpTheta = Random.Range(0f, 2 * Mathf.PI);
                    jumpDistance = Random.Range(minJumpDistance, maxJumpDistance);
                    jumpVector.x = Mathf.Cos(jumpTheta) * jumpDistance / jumpDuration;
                    jumpVector.y = Mathf.Sin(jumpTheta) * jumpDistance / jumpDuration;
                }
            }
        }
        else
        {
            if (timer > jumpDuration)
            {
                timer = 0f;
                isIdle = true;
                if (isChasing)// if the blob is chasing a target, it jumps faster
                {
                    idleTime = Random.Range(1.25f, 1.75f) * averageIdleTime;
                }
                else
                {
                    idleTime = Random.Range(0.5f, 1.5f) * averageIdleTime;
                }
                Move(new Vector2(0, 0));
                isChasing = false;
            }
            else
            {
                Move(jumpVector);
            }
        }
        timer += Time.deltaTime;
    }

    private GameObject LookForTarget()
    {
        possibleTargets = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (Collider2D possibleTarget in possibleTargets)
        {
            minDist = detectionRadius;
            if (possibleTarget.gameObject.CompareTag("GreenBlob") | possibleTarget.gameObject.tag == "Player")
            {
                dist = Vector3.Distance(possibleTarget.gameObject.transform.position, transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    selectedTarget = possibleTarget.gameObject;
                }
            }
        }
        return selectedTarget;
    }

    void Move(Vector2 speed)
    {
        Vector2 velocity = speed;
        redBlob.velocity = velocity;
    }
}
