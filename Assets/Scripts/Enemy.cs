using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;

    float _verticalPositionLimit = 6f;
    float _horizontalPositionLimit = 10.0f;
    private SpawnManager _spawnManager; // get script SpawnManager of GameObject Spawn_Manager
    private Player _player;

    // Make handle to animator component
    private Animator _explosionAnimation;
    private float _explosionAnimLength = 2.6f;

    [SerializeField]
    private AudioClip _sfxClipExplosion;
    private AudioSource _sfxExplosion;

    private float _fireRate = 3.0f;
    private float _canFireAtTime = -1;

    private bool _waveEnded = false;

    public int enemyID; // 0 = Default. 1 = Big Laser. 2 = Zigzag movement

    private GameObject _enemyLaserBeam;

    [SerializeField] private float _BeamLargeYpos;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Enemy::Start() Called. _waveEnded=" + _waveEnded.ToString());

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

        _sfxExplosion = GetComponent<AudioSource>();
        if (_sfxExplosion == null)
        {
            Debug.LogError("_sfxExplosion is NULL.");
        }
        else
        {
            _sfxExplosion.clip = _sfxClipExplosion;
        }
    }

    // Update is called once per frame
    void Update()
    {

        switch (enemyID)
        {
            default:
                CalculateMovementRegular();
                FireLaserRegular();
                break;
            case 1: // big laser
                CalculateMovementRegular();
                FireLaserBeamLarge();
                break;
            case 2: // zigzag movement
                break;
        }

    }

    private void FireLaserRegular()
    {
        Debug.Log("Enemy::FireLaserRegular: Begin");
        if (Time.time > _canFireAtTime)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFireAtTime = Time.time + _fireRate;
            Vector3 _newLaserSpawnPos = transform.position + new Vector3(0, -1, 0); // offset laser spawn position down

            GameObject enemyLaser = Instantiate(_laserPrefab, _newLaserSpawnPos, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();

            }

        }
    }

    private void FireLaserBeamLarge()
    {
        Debug.Log("Enemy::FireLaserRegular: Begin");
        if (Time.time > _canFireAtTime)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFireAtTime = Time.time + _fireRate;
            Vector3 _newLaserSpawnPos = transform.position + new Vector3(0, _BeamLargeYpos, 0); // offset Large laser spawn position down

            _enemyLaserBeam = Instantiate(_laserPrefab, _newLaserSpawnPos, Quaternion.identity);
            _enemyLaserBeam.transform.parent = this.transform;

            Laser[] laserElement = _enemyLaserBeam.GetComponentsInChildren<Laser>();


            for (int i = 0; i < laserElement.Length; i++)
            {
                laserElement[i].AssignEnemyLaser();
                laserElement[i].AssignEnemyBigLaser(); 
            }
        }

    }

    public void ClearField()
    {
        _canFireAtTime = -1;
        _waveEnded = true;
    }

    void CalculateMovementRegular()
    {
        // move down at speed "_speed"
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -_verticalPositionLimit)
        {
            if (_explosionAnimation.GetCurrentAnimatorStateInfo(0).IsName("enemy_destroyed_anim") || _waveEnded == true )
            {
                Destroy(this.gameObject);
            }
            // if bottom of screen and still alive
            // respawn at top with a new random x position
            else
            {
                if (_spawnManager.get_stopSpawning() == "false")
                {
                    moveToTopRandomXPosition();
                }
                else
                {
                    Debug.Log("Player is dead. No need this enemy.");
                    Destroy(this.gameObject);
                }

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
            DestroyEnemy();
        }
        
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            
            if(_player != null)
            {
                _player.ScoreUpdate(10);
            }
            DestroyEnemy();
        }

    }


    private void DestroyEnemy()
    {
        // Destory Big Laser first
        if (enemyID == 1 && _enemyLaserBeam != null)
        {
            //Destroy(_enemyLaserBeam.gameObject);
            Destroy(_enemyLaserBeam);
            Debug.Log("Enemy::DestroyEnemy: Destroyed Big Laser Beam ");
        }

        // trigger anim
        _explosionAnimation.SetTrigger("OnEnemyDeath");
        Destroy(GetComponent<Collider2D>()); //Saves RAM to just destroy rather than disable.
        _speed = 0;
        _sfxExplosion.Play(0);

        Destroy(this.gameObject, _explosionAnimLength);
        Debug.Log("Enemy::DestroyEnemy: Destroyed Enemy ");


    }
}
