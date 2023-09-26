using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _playerShield;
    [SerializeField] private GameObject _playerDamageLeft;
    [SerializeField] private GameObject _playerDamageRight;
    [SerializeField] private AudioClip _audioLaserClip;
    [SerializeField] private bool _isPlayerOne;

    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private UIManager _UImanager;
    private AudioSource _audioLaserSource;

    private int _playerLives = 3;
    private int _playerScore;
    private float _increasePlayerSpeed = 4.0f;
    private float _playerSpeed = 5.0f;
    private float _fireRate = 0.2f;
    private float _canFire = -1.0f;
    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;
    private bool _alreadyActive;
    private bool _isFirstEnemyShot;
    private bool _isSingleMode;

    

    // Start is called before the first frame update
    void Start()
    {

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("MY ERROR: _spawnManager is NULL! (Player Start())");
        
        _UImanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_UImanager == null)
            Debug.LogError("MY ERROR: _UImanager is NULL! (Player Start())");

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gameManager == null)
            Debug.LogError("MY ERROR: _gameManager is NULL! (Player Start())");

        _audioLaserSource = GetComponent<AudioSource>();
        if (_audioLaserSource == null)
            Debug.LogError("MY ERROR: _audioLaseSource is NULL!(Player Start()");
        else // Set clip to variable
            _audioLaserSource.clip = _audioLaserClip;
    }

    void Update()
    {
        calculateMovment();
        if (_isPlayerOne)
        {
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
                FireLaser();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) && Time.time > _canFire)
                FireLaser();
        }
    }

    // Player Movment and bounderies
    void calculateMovment()
    {
        // ------------- Set Players boundries -------------
        float currX = transform.position.x;
        float currY = transform.position.y;
        float xBounderyLeft  = -9.3f;
        float xBounderyRight = 9.3f;
        float yBounderyDown  = -4.6f;
        float yBounderyUp    = 2f;

        // Set boundries - Y axis - Clamp means range. SC
        transform.position = new Vector3(currX, Mathf.Clamp(currY, yBounderyDown, yBounderyUp), 0);

        // Set boundries - X axis - Teleport from side to side.
        if (transform.position.x >= xBounderyRight)
            transform.position = new Vector3(xBounderyLeft, currY, 0);
        else if (transform.position.x <= xBounderyLeft)
            transform.position = new Vector3(xBounderyRight, currY, 0);

        // ------------- Control players movements -------------
        Vector3[] directions = { Vector3.left, Vector3.down, Vector3.right, Vector3.up};
        if (_isPlayerOne)
        {
            KeyCode[] PlayerKeys = {KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.W};
            for (int i = 0; i < directions.Length; i++)
                OnPressKeyMoveTo(PlayerKeys[i], directions[i]);
        }
        else
        {
            KeyCode[] PlayerTwoKeys = {KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad8};
            for (int i = 0; i < directions.Length; i++)
                OnPressKeyMoveTo(PlayerTwoKeys[i], directions[i]);  
        }
    }

    void OnPressKeyMoveTo(KeyCode keyCode, Vector3 direction)
    {
        if (Input.GetKey(keyCode))
            transform.Translate(direction * _playerSpeed * Time.deltaTime);
    }

    // Shoot laser with space key
    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        _audioLaserSource.Play();

        // If triple key is active -> Fire 3 laseres
        if (_isTripleShotActive)
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        else
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
    }

    public void Damage()
    {
        if (!_isShieldActive)
        {
            _playerLives--;
            if (_playerLives == 2)
                _playerDamageRight.SetActive(true);
            else if (_playerLives == 1)
                _playerDamageLeft.SetActive(true);

            if (_isPlayerOne)
                AddScore(-10);
            else
                GameObject.Find("Player").GetComponent<Player>().AddScore(-10);

            if (_playerLives > -1)
                _UImanager.UpdateLives(_playerLives, _isPlayerOne);

            if (_playerLives <= 0)
                GameOver();
        }
        else {
            _isShieldActive = false;
            _playerShield.SetActive(false);
        }    
    }
    public void GameOver()
    {
        _spawnManager.StopSpawning();
        if (!_isSingleMode)
        {
            if (this.tag == "Player")
                Destroy(GameObject.Find("Player Two"));
            else
                Destroy(GameObject.Find("Player"));
        }
        Destroy(this.gameObject);
    }

    public void SetLives(int lives)
    {
        _playerLives = lives;
    }

    public int GetScore()
    {
        return _playerScore;
    }
    public void AddScore(int ScoreToAdd)
    {
        _playerScore += ScoreToAdd;
        _UImanager.UpdateScore(_playerScore);

    }
    public void TripleShotActivation()
    {
        if (_isTripleShotActive)
            _alreadyActive = true;
        else
            _isTripleShotActive = true;

        StartCoroutine(ActivateTripleShot());
    }
    IEnumerator ActivateTripleShot()
    {
        yield return new WaitForSeconds(5.0f);
        // If got one more do not puase the triple shoot.
        if (!_alreadyActive)
            _isTripleShotActive = false;
        else
            _alreadyActive = false;
    }
    public void SpeedActivation()
    {
        _playerSpeed += _increasePlayerSpeed;
        StartCoroutine(ActivateSpeedPlayer());
    }
    IEnumerator ActivateSpeedPlayer()
    {
        yield return new WaitForSeconds(5.0f);
        _playerSpeed -= _increasePlayerSpeed;
    }
    public void ShieldActivation()
    {
        _isShieldActive = true;
        _playerShield.SetActive(true);
    }
    public void SetIsFirstEnemyShot(bool isFirst)
    {
        _isFirstEnemyShot = isFirst;
    }
    public bool GetIsFirstEnemyShot()
    {
        return _isFirstEnemyShot;
    }

    public bool GetIsPlayerOne()
    {
        return _isPlayerOne;
    }
}