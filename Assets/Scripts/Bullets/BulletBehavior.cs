using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
        if(other.CompareTag("GreenBlob"))
        {
            other.gameObject.GetComponent<GreenBlob_Behaviour>().DamageBlob(100);
        }
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<RedBlob_Behaviour>().DamageBlob(100);
        }
    }
}
