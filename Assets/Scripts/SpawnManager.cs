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
    private GameObject _powerupTripleShot;
    [SerializeField]
    private GameObject _powerupContainer;
    float _yPositionLimit = 6f;
    float _xPositionLimit = 10.0f;
    float _randomX;
    float _randomY;
    float _waitTime = 5.0f;
    float _randomWaitTime;
    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemyRoutine());
        StartCoroutine(spawnPowerupTripleshotRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator spawnEnemyRoutine()
    {

        //while loop (infinite loop)
        while (_stopSpawning == false)
        {
            // Instantiate enemy prefab
            _randomX = Random.Range(-_xPositionLimit, _xPositionLimit);
            _randomY = Random.Range(_yPositionLimit/2, _yPositionLimit);
            Vector3 spawnPosition = new Vector3(_randomX, _randomY, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            // yield wait for 5 seconds
            yield return new WaitForSeconds(_waitTime);
        }
    }

    IEnumerator spawnPowerupTripleshotRoutine()
    {
        while (_stopSpawning == false)
        {
            // Every 3-7 seconds spawn in a powerup
            _randomWaitTime = Random.Range(3.0f, 7.0f);

            _randomX = Random.Range(-_xPositionLimit, _xPositionLimit);
            _randomY = Random.Range(_yPositionLimit / 2, _yPositionLimit);
            Vector3 spawnPosition = new Vector3(_randomX, _randomY, 0);

            GameObject newPowerupTripleShot = Instantiate(_powerupTripleShot, spawnPosition, Quaternion.identity);
            newPowerupTripleShot.transform.parent = _powerupContainer.transform;

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
