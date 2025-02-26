using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _health = 3;
    [SerializeField] private int _score = 0;
    [SerializeField] private float _normalSped = 5f;
    [SerializeField] private float _thrusterSpeed = 10f;
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private int _speedMultiplyer=1;
    [SerializeField] private float _hLimt=9.2f;
    [SerializeField] private float _vLimt=5;
    [SerializeField] private GameObject[] _laserPrefabs;
    [SerializeField] private int _laserId;
    [SerializeField] private Transform _laserPos;
    [SerializeField] private float _canFire ;
    [SerializeField] private float _fireRate=15f;
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
    [SerializeField] private float _trippleEndTime, _speedEndTime, _ShiledEndTime;
    [SerializeField] private GameObject _tripplePrefab;
    [SerializeField] private bool _shiledOn = false;
    [SerializeField] private GameObject _shiledVisual;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private GameObject _hitLeft, _hitRight;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _clip;
    private  float horizontalInput, verticalInput;
    public float maxThrusterCharge = 100f;
    [SerializeField] private float _currentThrusterCharge;
    [SerializeField] private float _thrusterRate=10f;
    [SerializeField] private int _shiledStrength;
    [SerializeField] private int _currentAmmo, _defaultAmmo;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private CameraShake _camera;



    // Start is called before the first frame update
    void Start()
    {
        _shiledVisual.SetActive (false);
        _speed = _normalSped;
        _isLive = true;
        _source = GetComponent<AudioSource>();
        transform.position = Vector3.zero;
        _hitLeft.SetActive(false); 
        _hitRight.SetActive(false);
        maxThrusterCharge = 100f;
        _currentThrusterCharge = maxThrusterCharge;
        _thrusterRate = 10f;
        _uiManager.thrusterBar.value = _currentThrusterCharge;
        _shiledStrength = 3;
        _defaultAmmo = 15;
        _currentAmmo = _defaultAmmo;
        _uiManager.RefreshAmmo(_currentAmmo);
        _laserId = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            ThrusterON();
           

        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _normalSped;
        }
        else
        {
            RechargeThruster();
        }


        Movement();
        if (Input.GetKeyDown(KeyCode.Space)&&Time.time>_canFire)
        {
            if (_currentAmmo>0)
            {
                Shoot();
            }
            
        }
        


    }


    void Movement()
    {
        
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
            _currentAmmo--;
            _uiManager.RefreshAmmo(_currentAmmo);
            _source.PlayOneShot(_clip);
            Instantiate(_tripplePrefab, _laserPos.position, Quaternion.identity);
            return;
        } 
        else
        {
            _currentAmmo--;
            _uiManager.RefreshAmmo(_currentAmmo);
            _source.PlayOneShot(_clip);
            Instantiate(_laserPrefabs[_laserId], _laserPos.position, Quaternion.identity);
        }
        
        
    }

    public void TakeDamage()
    {
        if (_shiledOn)
        {
            _shiledStrength--;
            _uiManager.ChangeShieldImage(_shiledStrength);
            //refresh Shiled UI Image
            return;
        }
        Debug.Log("decrease health ..");
        _health--;
        _camera.TriggerShake();
        

        if (_health == 2)
        {
            _hitLeft.SetActive(true);
            _hitRight.SetActive(false);

        }
        else if (_health == 1)
        {
            _hitLeft.SetActive(true);
            _hitRight.SetActive(true);
        }
        

        if (_health < 1)
        {
            // game ove 
            _isLive = false;

            _health = 0;
            Destroy(this.gameObject);
        }
        _uiManager.LivesChange(_health);

    }


    IEnumerator TrippleShootRoutine()
    {
        _trippleShootON = true;
        _trippleEndTime = Time.time + _trippleTime;
       yield return new WaitForSeconds( _trippleTime);
        TrippleShootOff();
    }
    public void TrippleShoot()
    {
        StopCoroutine(TrippleShootRoutine());
        StartCoroutine(TrippleShootRoutine());
    }
    void TrippleShootOff()
    {
        if (_trippleEndTime <= Time.time)
        {
            Debug.Log("tripple shoot off..");
            _trippleShootON = false;

        }
    }

    IEnumerator Speed2XRoutine()
    {
        _speedMultiplyer = 2;
        _speedEndTime = Time.time + _speedTime;
        yield return new WaitForSeconds ( _speedTime);
        Speed2XOff();
    }

    public void Speed2X()
    {
        StopCoroutine(Speed2XRoutine());
        StartCoroutine(Speed2XRoutine());
    }
    void Speed2XOff()
    {
        if (_speedEndTime <= Time.time)
        {
            Debug.Log("Speed2X off..");
            _speedMultiplyer = 1;
        }
       
    }
    IEnumerator ShiledOnRoutine()
    {
        //visualize shiled UI Image
        _uiManager.ShiledOn();
        _shiledStrength = 3;
        _shiledOn = true;
        _ShiledEndTime = Time.time + _shiledTime;
        _shiledVisual.SetActive(true);
        //show the shield around the player
        yield return new WaitForSeconds  ( _shiledTime);
        ShiledOff();
    }

    public void ShiledOn()
    {
        StopCoroutine(ShiledOnRoutine());
        StartCoroutine(ShiledOnRoutine());
    }
    void ShiledOff()
    {
        if (_ShiledEndTime <= Time.time)
        {
            Debug.Log("shiled off..");
            _shiledVisual.SetActive(false);
            _shiledOn = false;
            _uiManager.ShiledOff();
        }
       
    }
    public void ShiledDameged()
    {
        StopCoroutine(ShiledOnRoutine());
        _shiledVisual.SetActive(false);
        _shiledOn = false;
    }
    public void AddScore(int amount)
    {
        _score += amount;
        //refresh UI score
        _uiManager.RefreshScore(_score);
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag== "Enemy")
        {
            other.GetComponent<BoxCollider2D>().enabled = false;
            TakeDamage();
            Destroy(other.gameObject);
        }
    }

    void ThrusterON()
    {
        
        if (_currentThrusterCharge <= 0)
        {
            _speed = _normalSped;
            _currentThrusterCharge = 0;
            return;
        }
        else
        {
            _speed = _thrusterSpeed;
            _currentThrusterCharge -= Time.deltaTime * _thrusterRate;
            _uiManager.thrusterBar.value = _currentThrusterCharge / 100;

        }
        
    }

    void RechargeThruster()
    {
        if (_currentThrusterCharge < maxThrusterCharge)
        {
            _currentThrusterCharge += Time.deltaTime * _thrusterRate * 2;
            _uiManager.thrusterBar.value = _currentThrusterCharge/100;
        }
    }
   public void IncreaseHealth()
    {
        _health++;
        if (_health >= 3) 
        {
            _health = 3;
            _uiManager.LivesChange(_health);
        }
        if (_health == 2)
        {
            _hitLeft.SetActive(true);
            _hitRight.SetActive(false);

        }
        else if (_health == 1)
        {
            _hitLeft.SetActive(true);
            _hitRight.SetActive(true);
        }
        else
        {
            _hitLeft.SetActive(false);
            _hitRight.SetActive(false);
        }
        _uiManager.LivesChange(_health);
    }

    public void IncreaseAmmo()
    {
        _currentAmmo +=_defaultAmmo ;
        _uiManager.RefreshAmmo(_currentAmmo);

    }

    public void SecondLaser()
    {
        _laserId = 1;
        Invoke("SecondLaserOff", 5f);
    }

    void SecondLaserOff()
    {
        
        Debug.Log("seecond laser back");
        _laserId = 0;
    }

}
