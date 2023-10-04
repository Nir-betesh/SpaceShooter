using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField] private GameObject _explosinPrefab;

    private Player player;
    private SpawnManager _spawnManager;
    private readonly float yBoundryDown = -7.0f;
    private readonly int rotationSpeed = 45;
    private readonly int _astroidSpeed = 3;
    private int _hitsCounter = 5;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("MY ERROR: _spawnManager is NULL!!! Astroid::Start()");

        player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
            Debug.LogError("MY ERROR: player is NULL!!! Astroid::start()");
    }

    void Update()
    {
        CalculateMovment();
    }

    void CalculateMovment()
    {
        // Rotate the astroid
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Astroid direction
        transform.Translate(Vector3.down * Time.deltaTime * _astroidSpeed, Space.World);
        if (transform.position.y <= yBoundryDown)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            _hitsCounter--;
            if (_hitsCounter == 0)
            {
                Instantiate(_explosinPrefab, transform.position, Quaternion.identity);
                Destroy(other.gameObject);
                player.AddToScore(50);
                Destroy(this.gameObject, 0.25f);
            }
            else
                Destroy(other.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            player.SetLives(1);
            player.Damage();
        }
        else if(other.CompareTag("Player Two"))
        {
            Player playerTwo = GameObject.Find("Player Two").GetComponent<Player>();
            if (playerTwo == null)
                Debug.LogError("MY ERROR: playerTwo is NULL!!! Astroid::start()");

            playerTwo.SetLives(1);
            playerTwo.Damage();
        }
    }
}
