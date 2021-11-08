using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    /// <summary>
    /// Object to spawn
    /// </summary>
    public GameObject Prefab;

    /// <summary>
    /// Seconds between spawn operations
    /// </summary>
    public float SpawnInterval;

    public float InitSpawnInterval = 3;


    public float Count = 0;

    /// <summary>
    /// Check if we need to spawn and if so, do so.
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void Update()
    {
        if (SpawnInterval > 0.5)
        {
            SpawnInterval = InitSpawnInterval - ScoreKeeper.getPoints() * 0.25f;
        } else 
        {
            SpawnInterval = InitSpawnInterval - ScoreKeeper.getPoints() * 0.1f;
            if (SpawnInterval < 0)
            {
                SpawnInterval = 0.1f;
            }
        } 
        
        if (Time.time > Count && !GameManager.isGameOver())
        {
            Instantiate(Prefab, transform.position, Quaternion.identity);
            Count = Time.time + SpawnInterval;
        }

    }
}
