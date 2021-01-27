using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDropping : MonoBehaviour
{
    [SerializeField] public float minDelay;
    [SerializeField] public float maxDelay;

    public GameObject BulletDrop;

    private float delay;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        delay = Random.Range(minDelay, maxDelay);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > delay) {
            timer = 0;
            Instantiate(BulletDrop, gameObject.transform);
        }

    }
}
