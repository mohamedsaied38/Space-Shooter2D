using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _limit = -5;
    [SerializeField] private BoxCollider2D _boxColider;
    [SerializeField] private Player player;
    [SerializeField] private Animator _anim;
    [SerializeField] private AudioSource _source;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _canFire = -.1f;


    private void Start()
    {
        _anim = GetComponent<Animator>();
        _source = GetComponent<AudioSource>();
      player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        Movement();
        ShootLaser();
    }

    void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < _limit)
        {
            float randomx = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomx, 7, 0);

        }
    }

    void ShootLaser()
    {
        if (Time.time > _canFire)
        {
            _canFire = Random.Range(3f, 7f);
            _canFire = Time.time + _canFire;
            GameObject laser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = laser.GetComponentsInChildren<Laser>();
            for(int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemeyLaser();
                lasers[i].tag = "Enemey";
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _boxColider.enabled = false;
            Debug.Log("hit player");
            other.transform.GetComponent<Player>().TakeDamage();
            _anim.SetTrigger("IsDestroyed");
            _speed = 0;
            _source.Play();

            Destroy(this.gameObject,1f);
        }

        if (other.tag=="Laser")
        {
            Destroy(_boxColider);
            
            Debug.Log("Laser");
           
            Destroy(other.gameObject);
           
            if (player != null)
            {
                player.AddScore(5);
            }
            else
            {
                Debug.LogWarning("player is null no more score ..");
            }
            _anim.SetTrigger("IsDestroyed");
            _speed = 0;
            _source.Play();
            Destroy(this.gameObject,1f);
        }
    }

    

}
