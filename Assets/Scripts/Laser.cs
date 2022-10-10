using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public bool _isBigLaser = false;

    [SerializeField]
    private int _speed = 8;
    private int _position_limit = 8;
    private bool _isEnemyLaser = false;

    // Update is called once per frame
    void Update()
    {
        if (_isBigLaser == false)
        {
            if (_isEnemyLaser == false)
            {
                MoveUp();
            }
            else
            {
                MoveDown();
            }
        }

    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        //transform.Translate(Vector3.Angle);

        if (transform.position.y > _position_limit)
        {
            if (this.transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -_position_limit)
        {
            if (this.transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    public void AssignEnemyBigLaser()
    {
        _isBigLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
        }
    }


}