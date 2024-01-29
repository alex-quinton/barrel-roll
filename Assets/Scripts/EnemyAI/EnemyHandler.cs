using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] private float spawnDistance;
    [SerializeField] public float enemyCullingDistance;
    public List<SpawnWave> spawnWaves = new List<SpawnWave>();
    public List<SpawnWave> quedWaves = new List<SpawnWave>();
    private GameObject playerRef;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnDistance > enemyCullingDistance) 
        {
            throw new Exception("EnemyCullingDistance must be greater than SpawnDistance - " + gameObject.name);
        }

        SetReferences();
    }

    // Update is called once per frame
    void Update()
    {
        QueNewWaves();
        DequeOldWaves();

        CheckForNewSpawns();
    }

    /// <summary>
    /// Ques up any waves that need to start spawning.
    /// </summary>
    private void QueNewWaves()
    {
        for (int i = spawnWaves.Count - 1; i >= 0; i--)
        {
            if (Time.timeSinceLevelLoad > spawnWaves[i].spawnTimeBand.x)
            {
                quedWaves.Add(spawnWaves[i]);
                spawnWaves.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Gets rid of waves that have exceeded their maximum lifespan.
    /// </summary>
    private void DequeOldWaves()
    {
        for (int i = quedWaves.Count - 1; i >= 0; i--)
        {
            if (Time.timeSinceLevelLoad > quedWaves[i].spawnTimeBand.y)
            {
                quedWaves.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Checks the qued waves, spawning any enemies that need to be spawned, and setting the timer for the next wave.
    /// </summary>
    private void CheckForNewSpawns() 
    {
        for (int i = quedWaves.Count - 1; i >= 0; i--) 
        {
            if (quedWaves[i].spawnTimer <= 0)
            {
                for (int j = 0; j < quedWaves[i].initialCount + quedWaves[i].growthRate * quedWaves[i].currentIteration; j++)
                {
                    SpawnEnemy(quedWaves[i].enemyPrefab);
                }

                quedWaves[i].currentIteration += 1;
                quedWaves[i].spawnTimer = quedWaves[i].spawnInterval;
            }
            else
            {
                quedWaves[i].spawnTimer -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Instantiates an enemy 'SpawnDistance' away from the player
    /// </summary>
    /// <param name="enemyPrefab">The GameObject to spawn</param>
    private void SpawnEnemy(GameObject enemyPrefab) 
    {
        float spawnAngle = UnityEngine.Random.value * 2 * Mathf.PI;
        Vector2 spawnOffset = (
            new Vector2(playerRef.transform.position.x, playerRef.transform.position.y) +
            new Vector2(Mathf.Cos(spawnAngle), Mathf.Sin(spawnAngle)) * spawnDistance
            );

        Instantiate(enemyPrefab, spawnOffset, Quaternion.identity);
    }

    /// <summary>
    /// Used by enemies to relocate themselves if they're outside the cull distance.
    /// </summary>
    /// <param name="culledEnemy">The enemy to relocate</param>
    public void CheckRelocate(GameObject culledEnemy) 
    {
        if ((culledEnemy.transform.position - playerRef.transform.position).magnitude > enemyCullingDistance) 
        {
            SpawnEnemy(culledEnemy);
            Destroy(culledEnemy);
        }
    }



    /// <summary>
    /// Sets all necessary object references.
    /// </summary>
    private void SetReferences()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// A class that contains all the data for constructing a new wave of enemies.
    /// </summary>
    [System.Serializable]
    public class SpawnWave 
    {
        public GameObject enemyPrefab;
        public Vector2 spawnTimeBand;
        public float spawnInterval;
        public int initialCount;
        public int growthRate;
        [System.NonSerialized]
        public float spawnTimer;
        [System.NonSerialized]
        public float currentIteration;
    }
}
