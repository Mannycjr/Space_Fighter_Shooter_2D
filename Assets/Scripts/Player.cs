using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _defaultSpeed = 5.0f;
    [SerializeField]
    private float _shiftSpeedIncrease = 2.5f;
    private float _speed;
    [SerializeField]
    private float _speedMultiplier = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotLaserPrefab;
    [SerializeField]
    private float _fireRate = 0.2f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager; // get script SpawnManager of GameObject Spawn_Manager
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldsActive = false;
    [SerializeField]
    private float _powerupTimeLimit = 5.0f;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private GameObject _damageEngineLeft, _damageEngineRight;

    [SerializeField]
    private int _score;

    private UIManager _uiManagerScript;

    [SerializeField]
    private AudioClip _sfxClipLaser;
    [SerializeField]
    private AudioClip _sfxClipExplosion;

    private AudioSource _sfxAudioSource;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private int _shieldStrength = 0;

    // Start is called before the first frame update
    void Start()
    {
        _speed = _defaultSpeed;

        // take the current position = new position
        transform.position = new Vector3(0, -11.3f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManagerScript = GameObject.Find("Canvas").GetComponent<UIManager>();
        _sfxAudioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Player::Start() Called. The Spawn Manager is NULL.");
        }

        if (_uiManagerScript == null)
        {
            Debug.LogError("Player::Start() Called. The UI Manager is NULL.");
        }

        if (_sfxAudioSource == null)
        {
            Debug.LogError("Player::Start() Called. _sfxAudioSource is NULL.");
        } else
        {
            _sfxAudioSource.clip = _sfxClipLaser;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Thrusters: 
        // ● Move the player at an increased rate when the ‘Left Shift’ key is pressed down 
        // ● Reset back to normal speed when the ‘Left Shift’ key is released
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed = _defaultSpeed + _shiftSpeedIncrease;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _defaultSpeed;
        }

        CalculateMovement();

        // Cooldown system
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float _horizontalInput = Input.GetAxis("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");
        float _verticalPositionLimit = 4.0f;
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

        // if tripleshotActive is true
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotLaserPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        // Play Laser SFX
        _sfxAudioSource.Play(0);  
    }

    public void Damage()
    {
        if (_isShieldsActive)
        {
            if (_shieldStrength >= 1)
            {
                --_shieldStrength;
                Debug.Log("shieldStrength="+ _shieldStrength);
            }
            else if (_shieldStrength < 1)
            {
                _isShieldsActive = false;
                _shieldVisualizer.SetActive(false);
            }

            _uiManagerScript.UpdateShieldsStrength(_shieldStrength);
            return;
        }

        _lives--;
        _uiManagerScript.UpdateLives(_lives);

        if (_lives == 2)
        {
            _damageEngineLeft.SetActive(true);
        }
        else if (_lives == 1)
        {
            _damageEngineRight.SetActive(true);
        }

        if (_lives < 1)
        {
            playExplosionAnim();
            // Communicate with Spawn Manager
            // Let them know to stop spawning
            _spawnManager.OnPlayerDeath();

            _sfxAudioSource.clip = _sfxClipExplosion;
            _sfxAudioSource.Play(0);
            
            Destroy(this.gameObject);
        }
    }

    public void TripleshotActive()
    {
        _isTripleShotActive = true;
        // start the powerdown coroutine for triple shot
        StartCoroutine(TripleshotPowerDownRoutine());
    }

    IEnumerator TripleshotPowerDownRoutine()
    {

        // wait 5 seconds
        yield return new WaitForSeconds(_powerupTimeLimit);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed = _speed * _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerupTimeLimit);
        _isSpeedBoostActive = false;
        _speed = _speed / _speedMultiplier;
    }

    public void ShieldsActive()
    {
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
        _shieldStrength = 3;
        Debug.Log("shieldStrength=" + _shieldStrength);
        _uiManagerScript.UpdateShieldsStrength(_shieldStrength);
        
    }

    // method to add 10 to the score
    public void ScoreUpdate(int points)
    {
        _score += points;
        _uiManagerScript.UpdateScore(_score); // communicate to the UI to update the score
    }
    
    public void playExplosionAnim()
    {
        Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
    }

}
