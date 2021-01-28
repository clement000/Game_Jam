using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    float timer = 0f;
    float expireTimer = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > expireTimer)
        {
            Destroy(gameObject);
        }
        timer += Time.deltaTime;
    }
}
