using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBlob_Behaviour : MonoBehaviour
{
    public float averageIdleTime = 1f;
    public float minJumpDistance = 1f;
    public float maxJumpDistance = 1f;

    private float jumpDuration = 1f;
    private float timer = 0f;
    private bool isIdle = true;
    private float idleTime;

    private Vector2 jumpVector = Vector2.zero;
    private float theta;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        Transform transform = gameObject.GetComponent<Transform>();
        idleTime = Random.Range(averageIdleTime - averageIdleTime / 2, averageIdleTime + averageIdleTime / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (isIdle)
        {
            if (timer > idleTime)
            {
                timer = 0f;
                isIdle = false;
                // Initialize the Jump
                theta = Random.Range(0f, 2 * Mathf.PI);
                distance = Random.Range(minJumpDistance, maxJumpDistance);
                jumpVector.x = Mathf.Cos(theta) * distance;
                jumpVector.y = Mathf.Sin(theta) * distance;
            }
        }
        else
        {
            if (timer > jumpDuration)
            {
                timer = 0f;
                isIdle = true;
                idleTime = Random.Range(averageIdleTime - averageIdleTime / 2, averageIdleTime + averageIdleTime / 2);
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
}



