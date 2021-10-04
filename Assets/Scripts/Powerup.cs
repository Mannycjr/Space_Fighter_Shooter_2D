using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    float _verticalPositionLimit = 6f;
    private SpawnManager _spawnManager_Powerups; // get script SpawnManager of GameObject Spawn_Manager

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
        // Only be collectable by the Player (by tags)
        // on collected, destroy
        if (other.tag == "Player")
        {
            // Get the player and assign to a handle
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                // Enable powerup for 5 seconds
                player.TripleshotActive();
            }

            Destroy(this.gameObject);
        }
    }

}
