﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    float _verticalPositionLimit = 6f;
    private SpawnManager _spawnManager_Powerups; // get script SpawnManager of GameObject Spawn_Manager
    [SerializeField] // 0 = Triple Shot; 1 = Speed; 2 = Shields; 3 = Ammo; 4 = Health; 5 = No Ammo; 6 = Wide Shot;
    private int _powerupID;

    [SerializeField] private AudioClip _sfxClipPowerup;

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
            //_sfxPowerup.Play(0);
            AudioSource.PlayClipAtPoint(_sfxClipPowerup, new Vector3(0,0,-9));
            
            // Get the player and assign to a handle
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        Debug.Log("TripleShot powerup");
                        player.TripleshotActive();
                        break;
                    case 1:
                        Debug.Log("Speed powerup");
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        Debug.Log("Shields powerup");
                        player.ShieldsActive();
                        break;
                    case 3:
                        Debug.Log("Ammo powerup");
                        player.RefillAmmo();
                        break;
                    case 4:
                        Debug.Log("Health powerup");
                        player.AddLife();
                        break;
                    case 5:
                        Debug.Log("NoAmmo powerdown");
                        player.NoAmmo();
                        break;
                    case 6:
                        Debug.Log("WideShot powerup");
                        player.WideShotActive();
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
