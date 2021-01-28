using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBlob_Behaviour : MonoBehaviour
{
    public float detectionRadius = 5f;
    public float averageIdleTime = 1f;
    public float minJumpDistance = 1f;
    public float maxJumpDistance = 1f;
    public float health = 10f;

    private float jumpDuration = 0.5f;

    private Collider2D[] possibleTargets;
    private float minDist, dist;
    private GameObject possibleTarget, target, selectedTarget;

    private bool isIdle = true;
    private bool isChasing = false;
    public bool isAbleToEat = true;
    private float timer = 0f, ableToEatTimer = 0f;
    private float ableToEatTime = 30f;
    private float idleTime, jumpDistance, jumpTheta, targetDistance;
    Vector3 jumpVector;
    RaycastHit2D hit;
    private Rigidbody2D redBlob;
    // Start is called before the first frame update
    void Start()
    {
        idleTime = Random.Range(0.5f, 1.5f) * averageIdleTime;
        redBlob = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isIdle)
        {
            if (ableToEatTimer > ableToEatTime)
            {
                isAbleToEat = true;
            }
            possibleTarget = LookForTarget();
            if (possibleTarget != null)
            {
                idleTime = Random.Range(0.4f,0.6f) * averageIdleTime;
            }
            if (timer > idleTime)
            {
                isIdle = false;
                timer = 0f;
                if (possibleTarget != null)
                {
                    target = possibleTarget;
                    //Check if the target can be reached in 1 jump
                    targetDistance = Vector3.Distance(target.transform.position, transform.position);
                    isChasing = true;
                    if (target.tag == "Player")//target is the player : jump towards him
                    {
                        jumpVector = JumpTowardsTargetInit(target);
                    }
                    else if (targetDistance < maxJumpDistance && isAbleToEat == true)//the target is a blob, that can be reached in one jump
                    {
                        jumpVector = JumpOnGreenBlobInit(target);
                    }
                    else if (isAbleToEat == true)//target is too far : jump towards it
                    {
                        jumpVector = JumpTowardsTargetInit(target);
                    }
                }
                else
                {
                    jumpVector = RandomJumpInit();
                }
            }
        }
        else
        {
            if (timer > jumpDuration)//time to stop jumping
            {
                timer = 0f;
                isIdle = true;
                if (isChasing && target != null)
                {
                    idleTime = Random.Range(0.5f, 0.75f) * averageIdleTime;// if the blob is chasing a target, it jumps faster
                    Vector3 vector = target.transform.position - transform.position;
                    if (vector.magnitude < 0.01 && target.gameObject.tag == "GreenBlob")
                    {
                        Eat(target);
                        isAbleToEat = false;
                        ableToEatTimer = 0;
                    }
                }
                else
                {
                    idleTime = Random.Range(0.5f, 1.5f) * averageIdleTime;
                }
                Move(new Vector2(0, 0));
                isChasing = false;
            }
            else//we are in the middle of a jump : continue moving normally
            {
                Move(jumpVector);
            }
        }
        timer += Time.deltaTime;
        ableToEatTimer += Time.deltaTime;
    }

    private GameObject LookForTarget()
    {
        possibleTargets = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        minDist = detectionRadius;
        selectedTarget = null;
        foreach (Collider2D possibleTarget in possibleTargets)
        {
            if ((possibleTarget.gameObject.CompareTag("GreenBlob") &&  possibleTarget.gameObject.GetComponent<GreenBlob_Behaviour>().isBeeingJumpedOn == false)| possibleTarget.gameObject.tag == "Player")
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

    void Move(Vector3 speed)
    {
        Vector2 speed2D = speed;
        redBlob.velocity = speed2D;
    }

    public void unableToEat()
    {
        isAbleToEat = false;
        this.ableToEatTime = Random.Range(0.8f,1.2f) * ableToEatTime;
        this.ableToEatTimer = 0f;
    }

    Vector3 JumpTowardsTargetInit(GameObject target)//jump towards the target, with a random jump distance
    {
        jumpDistance = Random.Range(minJumpDistance, maxJumpDistance);
        Vector3 vector = target.transform.position - transform.position;
        vector.z = 0;
        jumpVector = vector.normalized * jumpDistance / jumpDuration;
        return jumpVector;
    }

    Vector3 JumpOnGreenBlobInit(GameObject target)//jump exactly on the target
    {
        target.GetComponent<GreenBlob_Behaviour>().Jumped();
        //initiate a jump that will land right next to the target
        jumpVector = (target.transform.position - transform.position) / jumpDuration;
        return jumpVector;
    }

    Vector3 RandomJumpInit()
    {
        jumpTheta = Random.Range(0f, 2 * Mathf.PI);
        jumpDistance = Random.Range(minJumpDistance, maxJumpDistance);
        jumpVector.z = 0;
        jumpVector.x = Mathf.Cos(jumpTheta) * jumpDistance / jumpDuration;
        jumpVector.y = Mathf.Sin(jumpTheta) * jumpDistance / jumpDuration;
        return jumpVector;
    }

    void Eat(GameObject target)
    {
        //play the animation
        target.GetComponent<GreenBlob_Behaviour>().RemoveBlob();
        target = null;
        RedBlob_Behaviour newRedBlob = Instantiate(redBlob).GetComponent<RedBlob_Behaviour>();
        newRedBlob.unableToEat();
    }

    public void DamageBlob(float amount)
    {
        //play the animation
        health -= amount;
        if (health < 0)
        {
            Kill();
        }
    }

    public void Kill()//kills the blob (with death animation)
    {
        //play the animation
        Destroy(redBlob.gameObject);
    }
}
