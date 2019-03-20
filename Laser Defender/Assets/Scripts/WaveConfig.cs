using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creates a menu item in the Asset windows Context menu that allows creation of this scritable object
[CreateAssetMenu(menuName = "Enemy Wave Config")]

//This is a SCRIPTABLE OBJECT and !!!NOT!!! a Monobehaviour
public class WaveConfig : ScriptableObject
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numberOfEnemies = 5;
    [SerializeField] float moveSpeed = 2f;

    //Returns Enemy Prefab of Wave
    public GameObject GetEnemyPrefab()
    {
        return enemyPrefab;
    }

    //Returns Path of Wave
    public List<Transform> GetWayPoints()
    {
        var waveWayPoints = new List<Transform>();
        
        foreach (Transform child in pathPrefab.transform)
        {
            waveWayPoints.Add(child);
        }

        return waveWayPoints;
    }

    //Returns time delay between enemies spawning
    public float GetTimeBetweenSpawns()
    {
        return timeBetweenSpawns;
    }

    //Returns the randomness factor of the spawn rate
    public float GetSpawnRandomFactor()
    {
        return spawnRandomFactor;
    }

    //Returns number of enemies in this wave
    public int GetNumberOfEnemies()
    {
        return numberOfEnemies;
    }

    //Returns move speed of enemies in this wave
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}
