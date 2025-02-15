using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _health = 3;
    [SerializeField] private int _score = 0;
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private int _speedMultiplyer=1;
    [SerializeField] private float _hLimt=9.2f;
    [SerializeField] private float _vLimt=5;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private Transform _laserPos;
    [SerializeField] private float _canFire ;
    [SerializeField] private float _fireRate=.15f;
    public static bool isLive { get
        {
           
            return _isLive;
        }
    }
    private static bool _isLive = true;
    [SerializeField] private bool _trippleShootON = false;
    [SerializeField] private float _trippleTime=7;
    [SerializeField] private float _speedTime = 15;
    [SerializeField] private float _shiledTime = 10;
    [SerializeField] private GameObject _tripplePrefab;
    [SerializeField] private bool _shiledOn = false;
    [SerializeField] private GameObject _shiledVisual;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private GameObject _hitLeft, _hitRight;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _clip;


    // Start is called before the first frame update
    void Start()
    {
        _shiledVisual.SetActive (false);
        _isLive = true;
        _source = GetComponent<AudioSource>();
        transform.position = Vector3.zero;
        _hitLeft.SetActive(false); 
        _hitRight.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if (Input.GetKeyDown(KeyCode.Space)&&Time.time>_canFire)
        {
            Shoot();
        }
    }


    void Movement()
    {
        float horizontalInput, verticalInput;
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        float speed = _speed * _speedMultiplyer;

        transform.Translate(new Vector3(horizontalInput, verticalInput) * speed * Time.deltaTime);
        //constraint the player to the screen 
        if (transform.position.x > _hLimt) {

            transform.position = new Vector3(_hLimt, transform.position.y); 

        }
        else if (transform.position.x < -_hLimt) {

            transform.position = new Vector3(-_hLimt, transform.position.y); 
        }

        if (transform.position.y > _vLimt) {

            transform.position = new Vector3(transform.position.x, _vLimt);
        
        }
        else if (transform.position.y < -_vLimt) { 

            transform.position = new Vector3(transform.position.x, -_vLimt); 
        }
    }
    void Shoot()
    {
        
            _canFire = Time.time + _fireRate;
        if (_trippleShootON)
        {
            _source.PlayOneShot(_clip);
            Instantiate(_tripplePrefab, _laserPos.position, Quaternion.identity);
            return;
        }
        _source.PlayOneShot(_clip);
        Instantiate(_laserPrefab,_laserPos.position,Quaternion.identity);
        
    }

    public void TakeDamage()
    {
        if (_shiledOn)
        {
            return;
        }
        Debug.Log("decrease health ..");
        _health--;
        _uiManager.LivesChange(_health);

        if (_health == 2)
        {
            _hitLeft.SetActive(true);

        }
        else if (_health == 1)
        {
            _hitRight.SetActive(true);
        }

        if (_health < 1)
        {
            // game ove 
            _isLive = false;

            
            Destroy(this.gameObject);
        }

    }


    IEnumerator TrippleShootRoutine()
    {
        _trippleShootON = true;
       yield return new WaitForSeconds( _trippleTime);
        TrippleShootOff();
    }
    public void TrippleShoot()
    {
        StartCoroutine(TrippleShootRoutine());
    }
    void TrippleShootOff()
    {
        Debug.Log("tripple shoot off..");
        _trippleShootON = false;
    }

    IEnumerator Speed2XRoutine()
    {
        _speedMultiplyer = 2;
        yield return new WaitForSeconds ( _speedTime);
        Speed2XOff();
    }

    public void Speed2X()
    {
       StartCoroutine(Speed2XRoutine());
    }
    void Speed2XOff()
    {
        Debug.Log("Speed2X off..");
        _speedMultiplyer = 1;
    }
    IEnumerator ShiledOnRoutine()
    {
        _shiledOn = true;
        _shiledVisual.SetActive(true);
        //show the shield around the player
        yield return new WaitForSeconds  ( _shiledTime);
        ShiledOff();
    }

    public void ShiledOn()
    {
        StartCoroutine(ShiledOnRoutine());
    }
    void ShiledOff()
    {
        Debug.Log("shiled off..");
        _shiledVisual.SetActive(false);
        _shiledOn = false;
    }
    public void AddScore(int amount)
    {
        _score += amount;
        //refresh UI score
        _uiManager.RefreshScore(_score);
    }
    public void StopPowerups()
    {
        StopCoroutine(ShiledOnRoutine());
        StopCoroutine(Speed2XRoutine());
        StopCoroutine(TrippleShootRoutine());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag== "Enemey")
        {
            TakeDamage();
            Destroy(other.gameObject);
        }
    }


}
