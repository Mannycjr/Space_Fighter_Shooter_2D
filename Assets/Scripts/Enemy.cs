using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 4.0f;
    float _verticalPositionLimit = 6f;
    float _horizontalPositionLimit = 10.0f;
    private SpawnManager _spawnManager; // get script SpawnManager of GameObject Spawn_Manager
    private Player _player;

    // Make handle to animator component
    private Animator _explosionAnimation;
    private float _explosionAnimLength = 2.6f;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Enemy::Start() Called. The Player is NULL.");
        }

        // assign the animator component
        _explosionAnimation = GetComponent<Animator>();
        if (_explosionAnimation == null)
        {
            Debug.LogError("Enemy::Start() Called. The enemy explosion anim controller is NULL.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        // move down 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // if bottom of screen and still alive
        // respawn at top with a new random x position
        if (transform.position.y <= -_verticalPositionLimit)
        {
            if (_spawnManager.get_stopSpawning() == "false")
            {
                moveToTopRandomXPosition();
            } else
            {
                Debug.Log("Player is dead. No need this enemy.");
                Destroy(this.gameObject);
            }

        }

    }

    private void moveToTopRandomXPosition ()
    {
        float _randomHorizontalPosition;

        _randomHorizontalPosition = Random.Range(-_horizontalPositionLimit, _horizontalPositionLimit);
        transform.position = new Vector3(_randomHorizontalPosition, _verticalPositionLimit, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.transform.name + " tag: " + other.tag);

        // if other is Player
        if (other.tag == "Player")
        {
            // Get the player
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            // trigger anim
            _explosionAnimation.SetTrigger("OnEnemyDeath");
            _speed = 0;

            Destroy(this.gameObject, _explosionAnimLength);
        }
        
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            
            if(_player != null)
            {
                _player.ScoreUpdate(10);
            }
            // trigger anim
            _explosionAnimation.SetTrigger("OnEnemyDeath");
            _speed = 0;

            Destroy(this.gameObject, _explosionAnimLength);
        }

    }

}
