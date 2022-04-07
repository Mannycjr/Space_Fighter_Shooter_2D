using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    float _verticalPositionLimit = 6f;
    private SpawnManager _spawnManager_Powerups; // get script SpawnManager of GameObject Spawn_Manager
    [SerializeField] // 0 = Triple Shot; 1 = Speed; 2 = Shields; 3 = Ammo;
    private int _powerupID;

    [SerializeField]
    private AudioClip _sfxClipPowerup;
    //private Renderer _rend;

    /*
    void Start()
    {
        _sfxPowerup = GetComponent<AudioSource>();
        if (_sfxPowerup == null)
        {
            Debug.LogError("Powerup::Start() Called. _sfxPowerup is NULL.");
        }

        _rend = GetComponent<Renderer>();
        if (_rend == null)
        {
            Debug.LogError("Powerup::Start() Called. _rend is NULL.");
        }
    }
    */
    
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
                    default:
                        Debug.Log("Default powerup");
                        break;
                }
                
            }
            // turn off visibility
            //_rend.enabled = false;

            Destroy(this.gameObject);
        }
    }

}
