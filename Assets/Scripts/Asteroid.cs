using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 19.13f;

    [SerializeField]
    private GameObject _explosionPrefab;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Asteroid::Start() Called. The Game Manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
             
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.transform.name + " tag: " + other.tag);

        // check for laser collision
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _gameManager.StartSpawning(); // Start the game
            DestroyAsteroid();
        }

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _gameManager.StartSpawning(); // Start the game
            DestroyAsteroid();
        }
    }

    private void DestroyAsteroid()
    {
        // instantiate explosion at the position of the asterod (us)
        Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);

        // destroy asteroid. Explosions self-destruct automatically after about 3 seconds
        Destroy(this.gameObject, 0.0f);
        //
    }


    
    

}
