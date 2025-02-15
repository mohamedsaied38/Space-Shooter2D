using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]private GameObject _enemyPrefab;
    [SerializeField] private float _spawnDelay = 1.5f;
    [SerializeField] private GameObject _container;
    [SerializeField] private GameObject _powerup;
    [SerializeField] private float _powerupDelayMin = 5f;
    [SerializeField] private float _powerupDelayMax = 7f;
    [SerializeField] private GameObject[] powerups;
    [SerializeField] private bool _spawnEnemey = false;
    float randomx;
    GameObject enemy;
    Vector3 pos;
    int idRange;
    float spawn;
    private void Start()
    {
        _spawnEnemey = false;
    }

    public void SpawnGame()
    {
        _spawnEnemey = true;
        StartCoroutine(SpawnEnemey());
        StartCoroutine(SpawnPowerup());
    }

    IEnumerator SpawnEnemey()
    {
        Debug.Log("instatiate enemy");
        while (Player.isLive)
        {
             randomx = Random.Range(-9f, 9f);
             pos = new Vector3(randomx, 7, 0);
             enemy= Instantiate(_enemyPrefab,pos,Quaternion.identity);
            enemy.transform.parent = _container.transform;
            yield return new WaitForSeconds(_spawnDelay);
        }
        
       
    }

    IEnumerator SpawnPowerup()
    {
        Debug.Log("instatiate powerup");
        while (Player.isLive)
        {
            randomx = Random.Range(-9.2f, 9.2f);
           pos = new Vector3(randomx, 7, 0);
             idRange = Random.Range(0, 3);
            GameObject powerup = Instantiate(powerups[idRange], pos, Quaternion.identity);
            
             spawn = Random.Range(_powerupDelayMin, _powerupDelayMax);
            yield return new WaitForSeconds(spawn);
        }


    }
}
