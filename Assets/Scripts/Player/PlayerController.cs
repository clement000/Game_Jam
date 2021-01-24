using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float movSpeed;
    // [SerializeField] float movAccel;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
    }

    void Move(Vector2 _dir)
    {
        Vector2 velocity = _dir * movSpeed;
        transform.Translate(velocity);
    }
}
