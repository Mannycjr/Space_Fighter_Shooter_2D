using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // speed variable of 8
    private int _speed = 8;
    private int _position_limit = 8;

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > _position_limit)
        {
            Destroy(this.gameObject);
        }

    }
}