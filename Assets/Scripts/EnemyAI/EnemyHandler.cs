using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public List<SpawnWave> spawnWaves;
    List<SpawnWave> quedWaves;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        QueNewWaves();
        DequeOldWaves();
    }

    private void QueNewWaves()
    {
        foreach (SpawnWave wave in spawnWaves)
        {
            if (Time.deltaTime > wave.spawnTimeBand.x)
            {
                quedWaves.Add(wave);
            }
        }
    }

    private void DequeOldWaves()
    {
        foreach (SpawnWave wave in quedWaves)
        {
            if (Time.deltaTime > wave.spawnTimeBand.y)
            {
                quedWaves.Remove(wave);
            }
        }
    }

    [System.Serializable]
    public struct SpawnWave 
    {
        public GameObject enemyPrefab;
        public Vector2 spawnTimeBand;
        public float spawnInterval;
        public int initialCount;
        public int growthRate;
        [System.NonSerialized] 
        public float spawnTimer;
    }
}
