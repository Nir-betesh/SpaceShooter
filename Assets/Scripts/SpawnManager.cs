using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject _astroid;
    [SerializeField] private GameObject[] powerups;

    private bool _isSpawning = true;
    private readonly float yBoundryUp = 6.5f;
    private readonly float xBounderyRight =  6.8f;
    private readonly float xBounderyLeft  = -6.8f;

    // Start is called before the first frame update
    void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnAstroids());
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupsRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        // Spawn enemies
        while (_isSpawning == true)
        {
           float xAxis = Random.Range(xBounderyRight, xBounderyLeft);
           Vector3 posToSpawn = new Vector3(xAxis, yBoundryUp, 0);
           GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);

           // Orgenaize the spawnded enemy under 'Spawn Manager' file in Hirearchy.
           newEnemy.transform.parent = _enemyContainer.transform;
           yield return new WaitForSeconds(Random.Range(2.0f, 6.0f));
        } 
    }
    IEnumerator SpawnPowerupsRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        // Spawn powerups
        while (_isSpawning == true)
        {
            float xAxis = Random.Range(xBounderyRight, xBounderyLeft);
            int randPowerup = Random.Range(0, 3);
            Vector3 posToSpawn = new Vector3(xAxis, yBoundryUp, 0);
            Instantiate(powerups[randPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }
    }

    IEnumerator SpawnAstroids()
    {
        yield return new WaitForSeconds(Random.Range(10.0f, 15.0f));

        while (_isSpawning)
        {
            float xAxis = Random.Range(xBounderyRight, xBounderyLeft);
            Vector3 posToSpawn = new Vector3(xAxis, yBoundryUp, 0);
            Instantiate(_astroid, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(15.0f, 30.0f));
        }
    }

    public void StopSpawning()
    {
        _isSpawning = false;
    }
}
