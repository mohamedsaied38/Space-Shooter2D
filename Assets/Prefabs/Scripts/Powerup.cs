using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    [SerializeField] private float _ylimit = 6f;
    [SerializeField] private int id;
    [SerializeField] private AudioClip _clip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < _ylimit) 
        {

            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // on detect player make triple shoot 
        // destroy this gameobject
        if (collision.tag == "Player")
        {
            Debug.Log("Hit Player with powerup..."+id);
            Player player = collision.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, new Vector3(0,0,-10),20f);
            switch (id)
            {
                
                case 0:
                    
                    player.TrippleShoot();
                    break;
                case 1:
                    //speed powerup
                   
                    player.Speed2X();
                    break;
                case 2:
                    //shield powerup
                   
                    player.ShiledOn();
                    break;
                case 3:
                    player.IncreaseAmmo();
                    break;
                case 4 :
                    player.IncreaseHealth();
                        break;
                case 5 :
                    player.SecondLaser();
                    break;
                default:
                    Debug.Log("default power up id : " + id);
                    break;
                       
            }

            

            Destroy(this.gameObject);
        }
    }
}
