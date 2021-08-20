using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    // Start is called before the first frame update
    void Start()
    {

        // take the current position = new position
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float _horizontalInput = Input.GetAxis("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");
        float _verticalPositionLimit = 3.8f;
        float _horizontalPositionLimit = 11.30f;

        Vector3 _direction = new Vector3(_horizontalInput, _verticalInput, 0);

        transform.Translate(_direction * _speed * Time.deltaTime);

        // if player position on the y is greater than 3.8
        // y position = 3.8
        // else if player position on the y is less than 3.8
        // y position = -3.8

        if (transform.position.y >= _verticalPositionLimit)
        {
            transform.position = new Vector3(transform.position.x, _verticalPositionLimit, 0); 
        } else if (transform.position.y <= -_verticalPositionLimit)
        {
            transform.position = new Vector3(transform.position.x, -_verticalPositionLimit, 0);
        }

        // if player on the x > 11
        // x pos = -11
        // else if player on the x is less than -11
        // x pos = 11
        if (transform.position.x >= _horizontalPositionLimit)
        {
            transform.position = new Vector3(-_horizontalPositionLimit, transform.position.y, 0);
        } else if (transform.position.x <= -_horizontalPositionLimit)
        {
            transform.position = new Vector3(_horizontalPositionLimit, transform.position.y, 0);
        }

    }
}
