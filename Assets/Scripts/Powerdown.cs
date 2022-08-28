using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerdown : MonoBehaviour
{
    private float _speed = 3.0f;
    float _verticalPositionLimit = 6f;
    private SpawnManager _spawnManager_Powerdown; // get script SpawnManager of GameObject Spawn_Manager
    [SerializeField] // 0 = Triple Shot; 1 = Speed; 2 = Shields; 3 = Ammo; 4 = Health; 5 = Wide Shot;
    private int _powerdownID;

    [SerializeField]
    private AudioClip _sfxClipPowerup;

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
            AudioSource.PlayClipAtPoint(_sfxClipPowerup, new Vector3(0, 0, -9));

            // Get the player and assign to a handle
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerdownID)
                {
                    case 0:
                        Debug.Log("No Ammo");
                        player.NoAmmo();
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
