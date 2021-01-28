using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BigBulletBehaviour : MonoBehaviour
{
    public float explosionRadius;
    public GameObject Explosion;
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
        GameObject[] targets = LookForTargets();
        Destroy(gameObject);
        Explosion = Instantiate(Explosion);
        Explosion.transform.position = transform.position;
        foreach(GameObject target in targets)
        {
            if (target.tag == "GreenBlob")
            {
                target.GetComponent<GreenBlob_Behaviour>().Kill();
            }
            if (target.tag == "Enemy")
            {
                target.GetComponent<RedBlob_Behaviour>().Kill();
            }

        }
    }

    private GameObject[] LookForTargets()
    {
        Collider2D[] possibleTargets = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        GameObject[] selectedTargets;
        int count = 0;
        foreach (Collider2D possibleTarget in possibleTargets)
        {
            if (possibleTarget.gameObject.CompareTag("GreenBlob") | possibleTarget.gameObject.tag == "Enemy")
            {
                count += 1;
            }
        }
        selectedTargets = new GameObject[count];
        int i = 0;
        foreach (Collider2D possibleTarget in possibleTargets)
        {
            if (possibleTarget.gameObject.CompareTag("GreenBlob") | possibleTarget.gameObject.tag == "Enemy")
            {
                selectedTargets[i] = possibleTarget.gameObject;
                i += 1;
            }
        }
        return selectedTargets;
    }
}
