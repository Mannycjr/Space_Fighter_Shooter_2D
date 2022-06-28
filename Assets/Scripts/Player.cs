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
    private bool _isWideShotActive = false;
    [SerializeField]
    private float _powerupTimeLimit = 5.0f;
    [SerializeField]
    private float _thrusterChargeLevelMax = 10.0f;
    [SerializeField]
    private float _thrusterChargeLevel;
    [SerializeField]
    private float _changeThrusterChargeBy = 1.0f; // per second while using up thrusters or while waiting for 

    [SerializeField]
    private bool _canUseThrusters = true; // changes to false when _thrusterChargeLevel reaches 0.0f or less. changes to true when _thrusterChargeLevel reaches _thrusterChargeLevelMax after empty.
    [SerializeField]
    private bool _thrustersInUse = false; //


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

    [SerializeField]
    private int _ammoCount = 15; //Feature: Ammo Count

    // Start is called before the first frame update
    void Start()
    {
        _speed = _defaultSpeed;
        _thrusterChargeLevel = _thrusterChargeLevelMax;
        _canUseThrusters = true;

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

        _uiManagerScript.UpdateAmmo(_ammoCount);
    }

    
    void Update()
    {
        // Feature: Thrusters
        // ● Move the player at an increased rate when the ‘Left Shift’ key is pressed down 
        //    ● Reduce thrusters charge UI by _changeThrusterChargeBy per second
        // ● Reset back to normal speed when 
        //    ● the ‘Left Shift’ key is released
        //    ● or when thrusters charge is depleted
        // ● If thrusters charge is depleted,
        //    ● Reset back to normal speed
        //    ● If _thrusterChargeLevel < _thrusterChargeLevelMax
        //    ● Add _changeThrusterChargeBy to _thrusterChargeLevel

        // Check charge level. Restrict to min, max values
        _thrusterChargeLevel = Mathf.Clamp(_thrusterChargeLevel, 0, _thrusterChargeLevelMax);

        // Set _canUseThrusters depending on _thrusterChargeLevel
        if (_thrusterChargeLevel <= 0.0f)
        {
            _canUseThrusters = false;
        }
        else if (_thrusterChargeLevel > 0.0f)
        {
            _canUseThrusters = true;
        }

        // If left shit key is pressed and thrusters have charge, increase ship speed
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canUseThrusters)
        {
            _speed = _speed + _shiftSpeedIncrease;
            _thrustersInUse = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _defaultSpeed;
            _thrustersInUse = false;
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

        
        if(_thrustersInUse)
        {
            ThrustersActive();
/*
            _thrusterChargeLevel -= Time.deltaTime * _changeThrusterChargeBy;
            _uiManagerScript.UpdateThrustersSlider(_thrusterChargeLevel); //Reduce thruster bar UI 
            Debug.Log("_thrusterChargeLevel=" + _thrusterChargeLevel);

            if (_thrusterChargeLevel <= 0)
            {
                _uiManagerScript.ThurstersSliderUsableColor(false);
                _thrustersInUse = false;
                _thrusterChargeLevel = 0;
                SpeedReset();
            }
            */
        }
        else if(!_thrustersInUse)
        {
            StartCoroutine(ThrustersPowerReplenishRoutine());
        }


    }

    // Super Speed Thrusters are ON
    void ThrustersActive()
    {

        if (_canUseThrusters = true)
        {
            _thrusterChargeLevel -= Time.deltaTime * _changeThrusterChargeBy;
            _uiManagerScript.UpdateThrustersSlider(_thrusterChargeLevel); //Reduce thruster bar UI 
            Debug.Log("_thrusterChargeLevel=" + _thrusterChargeLevel);

            if (_thrusterChargeLevel <= 0)
            {
                _uiManagerScript.ThurstersSliderUsableColor(false);
                _thrustersInUse = false;
                SpeedReset();
            }
        }

        //StartCoroutine(ThrustersPowerDownRoutine());
    }

    IEnumerator ThrustersPowerReplenishRoutine()
    {
        yield return new WaitForSeconds(_powerupTimeLimit);
    }

    //
    void ThrustersInactive()
    {

    }
    

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        // Feature: Ammo Count
        if (_ammoCount > 0 )
         {
            // if tripleshotActive is true
            if (_isTripleShotActive == true && _isWideShotActive == false)
            {
                Instantiate(_tripleShotLaserPrefab, transform.position, Quaternion.identity);
                
            } 
            else if (_isTripleShotActive == false && _isWideShotActive == true)
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.Euler(0, 0, 45f));
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.Euler(0, 0, 90f));
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.Euler(0, 0, -45f));
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.Euler(0, 0, -90f));
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
                _isTripleShotActive = false;
                _isWideShotActive = false;
            }

            _ammoCount--;
            _uiManagerScript.UpdateAmmo(_ammoCount);
            Debug.Log("_ammoCount=" + _ammoCount);
            // Play Laser SFX
            _sfxAudioSource.Play(0);

        }

    }

    public void Damage()
    {
        // Feature: Shield Strength
        // ● Allow for 3 hits on the shield to accommodate visualization
        if (_isShieldsActive)
        {
            if (_shieldStrength >= 1)
            {
                --_shieldStrength;
                Debug.Log("shieldStrength="+ _shieldStrength);
            }

            if (_shieldStrength < 1)
            {
                _isShieldsActive = false;
                _shieldVisualizer.SetActive(false);
            }

            _uiManagerScript.UpdateShieldsStrength(_shieldStrength);
            return;
        }

        if (_lives > 0)
        {
            _lives--;
            _uiManagerScript.UpdateLives(_lives);
            UpdateDamage();
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
        _isWideShotActive = false;
        // start the powerdown coroutine for triple shot
        StartCoroutine(TripleshotPowerDownRoutine());
    }

    IEnumerator TripleshotPowerDownRoutine()
    {
        // wait 5 seconds
        yield return new WaitForSeconds(_powerupTimeLimit);
        _isTripleShotActive = false;
    }

    public void WideShotActive()
    {
        _isTripleShotActive = false;
        _isWideShotActive = true;
        // start the powerdown coroutine for triple shot
        StartCoroutine(WideShotPowerDownRoutine());
    }

    IEnumerator WideShotPowerDownRoutine()
    {
        // wait 5 seconds
        yield return new WaitForSeconds(_powerupTimeLimit);
        _isWideShotActive = false;
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

    public void SpeedReset()
    {
        _speed = _defaultSpeed;
        _isSpeedBoostActive = false;
    }

    public void ShieldsActive()
    {
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
        _shieldStrength = 3;
        Debug.Log("shieldStrength=" + _shieldStrength);
        _uiManagerScript.UpdateShieldsStrength(_shieldStrength);
        
    }

    // Feature: Ammo Collectable: Create a powerup that refills the ammo count allowing the player to fire again
    public void RefillAmmo()
    {
        _ammoCount = 15;
        _uiManagerScript.UpdateAmmo(_ammoCount);
    }

    // method to add 10 to the score
    public void ScoreUpdate(int points)
    {
        _score += points;
        _uiManagerScript.UpdateScore(_score); // communicate to the UI to update the score
    }

    // Feature: Health Collectable: Create a health collectable that heals the player by 1. Update the visuals of the Player to reflect this.
    public void AddLife()
    {
        if (_lives < 3)
        {
            _lives++;
            _uiManagerScript.UpdateLives(_lives);
            UpdateDamage();
        }
    }

    private void UpdateDamage()
    {
        switch (_lives)
        {
            case 3:
                _damageEngineRight.SetActive(false);
                _damageEngineLeft.SetActive(false);
                break;
            case 2:
                _damageEngineRight.SetActive(false);
                _damageEngineLeft.SetActive(true);
                break;
            case 1:
                _damageEngineRight.SetActive(true);
                _damageEngineLeft.SetActive(true);
                break;
        }
    }

    public void playExplosionAnim()
    {
        Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
    }

}
