using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 19.13f;

    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
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
            _spawnManager.StartSpawning();
            DestroyAsteroid();
        }

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _spawnManager.StartSpawning();
            DestroyAsteroid();
        }
    }

    private void DestroyAsteroid()
    {
        // instantiate explosion at the position of the asterod (us)
        //GameObject _newExplosion = 
        Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);

        // destroy asteroid and then explosion after 3 seconds
        Destroy(this.gameObject, 0.1f);
        //Destroy(_newExplosion, 3.0f);
    }


    
    

}
