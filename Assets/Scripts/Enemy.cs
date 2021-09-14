using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int _speed = 4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float _verticalPositionLimit = 6f;
        float _horizontalPositionLimit = 10.0f;
        float _randomHorizontalPosition;


        // move down 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // if bottom of screen
        // respawn at top with a new random x position
        if (transform.position.y <= -_verticalPositionLimit)
        {
            _randomHorizontalPosition = Mathf.Round(Random.Range(-_horizontalPositionLimit, _horizontalPositionLimit) * 100f) * 0.01f;
            transform.position = new Vector3(_randomHorizontalPosition, _verticalPositionLimit, 0);
        }



    }
}
