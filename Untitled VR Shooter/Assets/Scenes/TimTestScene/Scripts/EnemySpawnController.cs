using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] float spawnCooldown = 5.0f;

    public List<GameObject> spawnZones;

    void Start()
    {
        for (int i = 0; i < spawnZones.Count; i++)
        {
            Instantiate(enemyPrefab, spawnZones[i].transform.position, Quaternion.identity);        
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemySpawnCooldown())
        {
            for (int i = 0; i < spawnZones.Count; i++)
            {
                Instantiate(enemyPrefab, spawnZones[i].transform.position, Quaternion.identity);
            }

            spawnCooldown = 5.0f;
        }
    }

    private bool EnemySpawnCooldown()
    {
        spawnCooldown -= Time.deltaTime;
        return spawnCooldown <= 0;
    }
}
