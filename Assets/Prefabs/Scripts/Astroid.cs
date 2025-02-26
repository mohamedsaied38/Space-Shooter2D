using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 3f;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private SpawnManager spawnManager;
    bool _playOnce;
    // Start is called before the first frame update
    void Start()
    {
        _playOnce = true;
        spawnManager = GameObject.FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            PlayOnce();
        }
    }

    void PlayOnce()
    {
       
        if (_playOnce)
        {
            _playOnce = false;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            spawnManager.SpawnGame();
            Destroy(this.gameObject, .5f);

        }
        
    }

}
