using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractBullet : MonoBehaviour
{
    public float radius;
    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LayerMask mask = LayerMask.GetMask("GroundItem");
        RaycastHit2D[] hitColliders = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero, 0, mask);
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.rigidbody.transform.position = Vector3.MoveTowards(hitCollider.rigidbody.transform.position, transform.position, speed);
        }
    }

}
