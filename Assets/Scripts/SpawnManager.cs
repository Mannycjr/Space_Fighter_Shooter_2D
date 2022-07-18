using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups; // 0 = Tripleshot. 1 = Speed. 2 = Shields. 3 = Ammo. 4 = Health. 5 = Wide Shot.
    float _yPositionLimit = 6f;
    float _xPositionLimit = 10.0f;
    float _randomX;
    float _randomY;
    [SerializeField]
    float _randomZangle;
    float _waitTime = 2.0f; // Enemy spawning looping wait time between enemies
    float _waitTimeWideShot = 5.0f;
    float _randomWaitTime;
    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartSpawning()
    {
        StartCoroutine(spawnEnemyRoutine());
        StartCoroutine(spawnRandomPowerupRoutine());
        StartCoroutine(spawnWideShotPowerupRoutine());
    }

    IEnumerator spawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f); // Initial wait at beginning of game

        //while loop (infinite loop)
        while (_stopSpawning == false)
        {
            // Instantiate enemy prefab
            _randomX = Random.Range(-_xPositionLimit, _xPositionLimit);
            _randomY = Random.Range(_yPositionLimit/2, _yPositionLimit);
            _randomZangle = Random.Range(-45f,45f);
            Vector3 spawnPosition = new Vector3(_randomX, _randomY, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.Euler(0, 0, _randomZangle));
            newEnemy.transform.parent = _enemyContainer.transform;
            // yield wait for 5 seconds
            yield return new WaitForSeconds(_waitTime);
        }
    }

    IEnumerator spawnRandomPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            // Every 3-7 seconds spawn in a powerup
            _randomWaitTime = Random.Range(2.0f, 5.0f);

            _randomX = Random.Range(-_xPositionLimit, _xPositionLimit);
            _randomY = Random.Range(_yPositionLimit / 2, _yPositionLimit);
            Vector3 spawnPosition = new Vector3(_randomX, _randomY, 0);
            int randomPowerUp = Random.Range(0, (_powerups.Length - 1)); // Do not include 5 = Wide Shot.
            GameObject newPowerup = Instantiate(_powerups[randomPowerUp], spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(_randomWaitTime);
        }
    }

    // Feature: Secondary Powerup: WideShot
    IEnumerator spawnWideShotPowerupRoutine()
    {
        yield return new WaitForSeconds(10.0f);

        while (_stopSpawning == false)
        {
            // Every 10-15 seconds spawn in a powerup
            _randomWaitTime = Random.Range(12.0f, 20.0f);

            _randomX = Random.Range(-_xPositionLimit, _xPositionLimit);
            _randomY = Random.Range(_yPositionLimit / 2, _yPositionLimit);
            Vector3 spawnPosition = new Vector3(_randomX, _randomY, 0);
            GameObject newPowerup = Instantiate(_powerups[5], spawnPosition, Quaternion.identity); // 5 = Wide Shot.

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
