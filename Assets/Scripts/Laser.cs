using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Laser class represents a laser projectile in the game.
/// It handles the movement, collision, and interaction logic of lasers.
/// </summary>
public class Laser : MonoBehaviour
{
    private readonly float _laserSpeed = 8.0f;
    private bool _isEnemy = false;
    private Player _player;

    /// <summary>
    /// Update is called once per frame.
    /// Handles the movement and collision behavior of the laser.
    /// </summary>
    void Update()
    {
        if (_isEnemy)
            LaserDirection(Vector3.down);
        else
            LaserDirection(Vector3.up);
    }

    /// <summary>
    /// Moves the laser in the specified direction.
    /// Handles destroying the laser if it goes out of bounds.
    /// </summary>
    /// <param name="direction">The direction in which the laser should move.</param>
    private void LaserDirection(Vector3 direction)
    {
        transform.Translate(direction * Time.deltaTime * _laserSpeed);
        CheckLaserBounds();
    }

    /// <summary>
    /// Checks if the laser is out of the screen bounds and destroys it.
    /// </summary>
    private void CheckLaserBounds()
    {
        if (transform.position.y < -6.0f || transform.position.y > 6.0f)
        {
            Destroy(transform.parent?.gameObject);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Assigns the laser as an enemy laser.
    /// </summary>
    public void SetIsEnemyLaser()
    {
        _isEnemy = true;
    }
    public bool GetIsEnemyLaser()
    {
        return _isEnemy;
    }

    /// <summary>
    /// Called when the laser's collider triggers with another collider.
    /// Handles damaging the player and destroying the laser if conditions are met.
    /// </summary>
    /// <param name="other">The collider that the laser collided with.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If enemy laser hit the player.
        if ((other.tag == "Player" || other.tag == "Player Two") && _isEnemy)
        {
            _player = other.GetComponent<Player>();
            onPlayerCollision(_player);
        }
    }

    void onPlayerCollision(Player player)
    {
        if (player.GetIsFirstEnemyShot())
        {
            player.Damage();
            player.SetIsFirstEnemyShot(false);
        }
        else
            player.SetIsFirstEnemyShot(true);

        Destroy(this.gameObject);
    }
}
