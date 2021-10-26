﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    float _verticalPositionLimit = 6f;
    private SpawnManager _spawnManager_Powerups; // get script SpawnManager of GameObject Spawn_Manager
    [SerializeField] // 0 = Triple Shot; 1 = Speed; 2 = Shields
    private int _powerupID;

    // Update is called once per frame
    void Update()
    {
        // move down at a speed of 3
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // When we leave the screen, destroy this object
        if (transform.position.y <= -_verticalPositionLimit)
        {    
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // Get the player and assign to a handle
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch(_powerupID)
                {
                    case 0:
                        player.TripleshotActive();
                        break;
                    case 1:
                        Debug.Log("Speed powerup");
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        Debug.Log("Shields powerup");
                        break;
                    default:
                        Debug.Log("Default powerup");
                        break;
                }

            }

            Destroy(this.gameObject);
        }
    }

}
