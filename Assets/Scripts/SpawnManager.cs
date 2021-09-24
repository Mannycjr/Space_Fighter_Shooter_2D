using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    float _yPositionLimit = 6f;
    float _xPositionLimit = 10.0f;
    float _randomX;
    float _randomY;
    float _waitTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnRoutine());        
    }

    // Update is called once per frame
    void Update()
    {


    }

    //spawn gameobjects every 5 seconds
    // Create a coroutine of type IEnumerator -- Yield Events
    // while loop

    IEnumerator spawnRoutine()
    {

        //while loop (infinite loop)
        while (true)
        {
            // Instantiate enemy prefab
            _randomX = Random.Range(-_xPositionLimit, _xPositionLimit);
            _randomY = Random.Range(0, _yPositionLimit);
            Vector3 spawnPosition = new Vector3(_randomX, _randomY, 0);
            Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);

            // yield wait for 5 seconds
            yield return new WaitForSeconds(_waitTime);
        }
    }

}
