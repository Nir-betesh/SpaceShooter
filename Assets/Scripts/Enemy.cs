using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemyLaserPrefab;

    private GameManager _gameManager;
    private AudioSource _audioExplosionSource;
    private Animator _anim;
    private Player _player;
    private Laser _laser;

    private float _canFire = -1;
    private float _enemySpeed = 4f;
    private float _fireRate;
    private bool _isCollided;

    void Start()
    {

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
            Debug.LogError("MY ERROR: Player is NULL!! UIManager::Start()");

        _audioExplosionSource = GetComponent<AudioSource>();
        if (_audioExplosionSource == null)
            Debug.LogError("MY ERROR: _audioExplosionSource is NULL!! Enemy::Start()");

        _anim = GetComponent<Animator>();
        if (_anim == null) 
            Debug.LogError("MY ERROR: _anim is NULL!! Enemy::Start()");

    }
    // Update is called once per frame
    void Update()
    {
        CalculateMovment();
        ShotEnemyLaser();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser" && _isCollided == false)
        {
            _laser = other.GetComponent<Laser>();
            // Only if its not other Enemy who made the shooting -> KILL ENEMY.
            if (!_laser.GetIsEnemyLaser())
            {
                _player.AddScore(10);
                OnEnemyDeath();
                Destroy(other.gameObject);
            }
        }
        else if ((other.tag == "Player" || other.tag == "Player Two") && _isCollided == false)
        {
            other.GetComponent<Player>().Damage();
            OnEnemyDeath();
        }
    }
    public void OnEnemyDeath()
    {
        _anim.SetTrigger("OnEnemyDeath");
        _enemySpeed = 0f;
        _audioExplosionSource.Play();
        _isCollided = true;
        Destroy(this.GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.6f);
    }

    void CalculateMovment()
    {
        float yBoundryUp = 6.5f;
        float yBoundryDown = -6.5f;

        transform.Translate(Vector3.down * Time.deltaTime * _enemySpeed);
        if (transform.position.y <= yBoundryDown)
        {
            float randX = Random.Range(-9.15f, 9.15f);
            transform.position = new Vector3(randX, yBoundryUp, 0);
        }
    }
    
    void ShotEnemyLaser(){
        if (Time.time > _canFire && !_isCollided)
        {
            _fireRate = Random.Range(2f, 5f);
            _canFire = Time.time + _fireRate;
            GameObject enemyObject = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            if (enemyObject == null)
                Debug.LogError("Enemy object is null!! Enemy::ShotEnemyLaser()");
            Laser[] enemyLaser = enemyObject.GetComponentsInChildren<Laser>();

            foreach (Laser laser in enemyLaser)
                laser.SetIsEnemyLaser();
        }
    }
    
}


