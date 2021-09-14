using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.2f;
    private float _canFire = -1f;

    // Start is called before the first frame update
    void Start()
    {

        // take the current position = new position
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float _horizontalInput = Input.GetAxis("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");
        float _verticalPositionLimit = 3.8f;
        float _horizontalPositionLimit = 11.30f;

        Vector3 _direction = new Vector3(_horizontalInput, _verticalInput, 0);

        transform.Translate(_direction * _speed * Time.deltaTime);

        // Limit the player's vetical movement to _verticalPositionLimit
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y,-_verticalPositionLimit, _verticalPositionLimit), 0);

        // Wrap the horizontal player movement within the bounds set by _horizontalPositionLimit
        if (transform.position.x >= _horizontalPositionLimit)
        {
            transform.position = new Vector3(-_horizontalPositionLimit, transform.position.y, 0);
        }
        else if (transform.position.x <= -_horizontalPositionLimit)
        {
            transform.position = new Vector3(_horizontalPositionLimit, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
    }
}
