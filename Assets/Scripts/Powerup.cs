using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private AudioClip _audioPowerupClip;
    [SerializeField] private int _powerupID;
    private readonly float _powerupSpeed = 3.0f;

    void Update()
    {
        CalculateMovments();
    }

    void CalculateMovments()
    {
        float yBoundryDown = -6.5f;

        transform.Translate(Vector3.down * Time.deltaTime * _powerupSpeed);

        if (transform.position.y < yBoundryDown)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player" || other.tag == "Player Two")
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            if (audioSource == null)
                Debug.LogError("MY ERRROR: audioSource is NULL!!! OnTriggerEnter2D()::Powerup");
            audioSource.clip = _audioPowerupClip;

            Player player = other.GetComponent<Player>();
            if (player == null)
                Debug.LogError("MY ERROR: player is NULL!!! Powerup::OnTriggerEnter2D()");
            else
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActivation();
                        break;
                    case 1:
                        player.SpeedActivation();
                        break;
                    case 2:
                        player.ShieldActivation();
                        audioSource.Play();
                        break;
                    default:
                        Debug.Log("MY ERROR: Powerup ID dose not found!!! OnTriggerEnter2D()::Powerup");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }

}
