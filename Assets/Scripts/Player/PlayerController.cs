using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float movSpeed;
    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;

    [Header("Weapon")]
    [SerializeField] float bulletSpeed;
    [SerializeField] float alternatBulletSpeed;

    // [SerializeField] float movAccel;

    Rigidbody2D Player;
    public GameObject GreenBlob;
    public Rigidbody2D Bullet, BigBullet;
    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<Rigidbody2D>();
        Player.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (Input.GetMouseButtonDown(0))
            if (GameObject.Find("GameSystem").GetComponent<BulletCounter>().bulletNumber > 0)
            {
                Fire();
                GameObject.Find("GameSystem").GetComponent<BulletCounter>().bulletNumber -= 1;
            }
        if (Input.GetMouseButtonDown(1))
            if (GameObject.Find("GameSystem").GetComponent<BulletCounter>().bulletNumber > 9)
            {
                AlternateFire();
                GameObject.Find("GameSystem").GetComponent<BulletCounter>().bulletNumber -= 10;
            }
    }

    private void FixedUpdate() 
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
    {
        // limit movement speed diagonally, so you move at 70% speed
        horizontal *= moveLimiter;
        vertical *= moveLimiter;
    }
        Move(new Vector2(horizontal, vertical));
    }

    void Move(Vector2 _dir)
    {
        Vector2 velocity = _dir * movSpeed;
        Player.velocity = velocity;
    }

    void Fire()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        var projectile = Instantiate(Bullet, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
        Vector2 BulletVelocity = new Vector2(dir.x, dir.y).normalized * bulletSpeed;
        projectile.velocity = BulletVelocity;


    }
    void AlternateFire()
    {
        Debug.Log("BIGBULLET");
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        var projectile = Instantiate(BigBullet, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
        Vector2 BulletVelocity = new Vector2(dir.x, dir.y).normalized * alternatBulletSpeed;
        projectile.velocity = BulletVelocity;
    }
}
