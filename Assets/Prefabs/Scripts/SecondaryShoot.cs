using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryShoot : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    public float explosionRadius = 5f;
    public int damage = 10;
    public float timeToExplode = 2f;
    // Start is called before the first frame update
    void Start()
    {
       //Invoke("Explode", timeToExplode);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }


    void CalculateMovement()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8)
        {

            if (transform.parent != null)
            {
                //for 2 laysser
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hit in colliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<Enemy>().TakeDamage(damage);
                //Destroy(hit, 1);
                Debug.Log("second shoot");
                Destroy(gameObject,.1f);
            }
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("secandry shoot hit " + other.name);
            Explode();
           
        }
    }
}
