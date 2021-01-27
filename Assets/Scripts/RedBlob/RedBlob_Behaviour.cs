using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBlob_Behaviour : MonoBehaviour
{
    public float detectionRadius = 5f;
    public float averageIdleTime = 1f;
    public float minJumpDistance = 1f;
    public float maxJumpDistance = 1f;

    private Collider[] possibleTargets;
    private float minDist, dist;
    private GameObject target;

    private bool isIdle = true;
    private float timer = 0f;
    private float idleTime;
    // Start is called before the first frame update
    void Start()
    {
        idleTime = Random.Range(0.5f, 1.5f) * averageIdleTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isIdle)
        {
            if (timer > idleTime)
            {
                GameObject target = LookForTarget();
            }
        }
        else
        {

        }
    }

    private GameObject LookForTarget()
    {
        possibleTargets = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider possibleTarget in possibleTargets)
        {
            if (possibleTarget.gameObject.CompareTag("GreenBlob") | possibleTarget.gameObject.CompareTag("Player"))
            {
                dist = Vector3.Distance(possibleTarget.gameObject.transform.position, transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    target = possibleTarget.gameObject;
                }
            }
        }
        return (target);
    }

}
