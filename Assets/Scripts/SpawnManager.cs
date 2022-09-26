using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    // 0 = Tripleshot. 1 = Speed. 2 = Shields. 3 = Ammo. 4 = Health. 5 = No Ammo. 6 = Wide Shot.
    [SerializeField] private GameObject[] _powerUpsDowns; 

    int _wideShotIndex;
    float _yPositionLimit = 6f;
    float _xPositionLimit = 10.0f;
    float _randomX;
    float _randomY;

    float _randomZangle;
    float _waitTimeWideShot = 5.0f;
    float _randomWaitTime;

    float _waitTimeEnemy = 5.0f; // Enemy spawning looping wait time between individual enemies
    float _waitTimeWaves = 7.0f; // Waves spawning looping wait time between waves of enemies
    int _maxEnemies = 1;
    int _enemiesSpawned = 0;
    private GameManager _gameManager;
    private bool _stopSpawning = false;


    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("SpawnManager::Start() Called. The Game Manager is NULL.");
        }

        _wideShotIndex = _powerUpsDowns.Length-1;

        Debug.Log("SpawnManager::Start() Called. _stopSpawning=" + _stopSpawning.ToString() + " _wideShotIndex=" + _wideShotIndex);

        
    }

    private void Update()
    {
        Debug.Log("SpawnManager::Update() Called. _stopSpawning=" + _stopSpawning.ToString());
    }

    public void StartSpawning(int _waveID)
    {
        Debug.Log("SpawnManager::StartSpawning() Called");
        _stopSpawning = false;
        GetWaveInfo(_waveID);
        StartCoroutine(spawnEnemyRoutine(_waitTimeEnemy));
        StartCoroutine(spawnRandomPowerupRoutine());
        StartCoroutine(spawnWideShotPowerupRoutine());
    }

    public void StopSpawning()
    {
        _stopSpawning = true;
        ClearEnemies();
    }

    private void ClearEnemies()
    {
        Debug.Log("SpawnManager::ClearEnemies() Called");
        Enemy[] _activeEnemies = _enemyContainer.GetComponentsInChildren<Enemy>();

        foreach (Enemy _enemy in _activeEnemies)
        {
            _enemy.ClearField();
        }
    }

    private void GetWaveInfo(int _waveID)
    {
        Debug.Log("SpawnManager::GetWaveInfo() Called");
        WaitForSeconds _respawnTime = new WaitForSeconds(10);
 

        switch (_waveID)
        {
            case 1:
                _maxEnemies = 2;
                _waitTimeEnemy = 3.5f;
                break;
            case 2:
                _maxEnemies = 4;
                _waitTimeEnemy = 3.0f;
                break;
            case 3:
                _maxEnemies = 6;
                _waitTimeEnemy = 2.5f;
                break;
            case 4:
                _maxEnemies = 8;
                _waitTimeEnemy = 2.0f;
                break;
            case 5:
                _maxEnemies = 10;
                _waitTimeEnemy = 1.0f;
                break;
        }

    }


    //int _enemyPool = _enemyPrefab.Length;

    IEnumerator spawnEnemyRoutine(float _waitTimeEnemy)
    {
        Debug.Log("SpawnManager::spawnEnemyRoutine() Called");
        //yield return new WaitForSeconds(3.0f); // Initial wait at beginning of game

        while (!_stopSpawning)
        {
            for (int i = 0; i < _maxEnemies; i++)
            {
                yield return new WaitForSeconds(_waitTimeEnemy);

                if (!_stopSpawning)
                {
                    // Instantiate enemy prefab
                    float _randomX = Random.Range(-_xPositionLimit, _xPositionLimit);
                    _randomZangle = Random.Range(-45f,45f);
                    Vector3 spawnPosition = new Vector3(_randomX, _yPositionLimit, 0);
                    GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.Euler(0, 0, _randomZangle));
                    _enemiesSpawned++;
                    newEnemy.transform.parent = _enemyContainer.transform;
                }
                
            }

            yield return new WaitForSeconds(_waitTimeWaves);
        }
    }

    IEnumerator spawnRandomPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            // Every 4-8 seconds spawn in a powerup
            _randomWaitTime = Random.Range(4.0f, 8.0f);

            _randomX = Random.Range(-_xPositionLimit, _xPositionLimit);
            _randomY = Random.Range(_yPositionLimit / 2, _yPositionLimit);
            Vector3 spawnPosition = new Vector3(_randomX, _randomY, 0);
            int randomPowerUp = Random.Range(0, (_powerUpsDowns.Length-1)); // Exclude WideShot powerup ID=6
            GameObject newPowerup = Instantiate(_powerUpsDowns[randomPowerUp], spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(_randomWaitTime);
        }
    }

    // Feature: Secondary Powerup: WideShot
    IEnumerator spawnWideShotPowerupRoutine()
    {
        Debug.Log("SpawnManager::spawnWideShotPowerupRoutine() Called");
        yield return new WaitForSeconds(10.0f);

        while (_stopSpawning == false)
        {
            // Every 10-15 seconds spawn in a powerup
            _randomWaitTime = Random.Range(15.0f, 20.0f);

            _randomX = Random.Range(-_xPositionLimit, _xPositionLimit);
            _randomY = Random.Range(_yPositionLimit / 2, _yPositionLimit);
            Vector3 spawnPosition = new Vector3(_randomX, _randomY, 0);
            GameObject newPowerup = Instantiate(_powerUpsDowns[_wideShotIndex], spawnPosition, Quaternion.identity); 

            yield return new WaitForSeconds(_randomWaitTime);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public string get_stopSpawning()
    {
        string returnValue;

        if (_stopSpawning)
        {
            returnValue = "true";
        } else
        {
            returnValue = "false";
        }

        return returnValue;
    }

}
