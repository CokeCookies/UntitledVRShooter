using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] float spawnCooldown = 5.0f;
    private GameObject player;
    private int waveNumber; //count up
    private int waveMultiplier; //perhaps percentage above previous wave in initial implementation
    [SerializeField] int maxEnemies;    //increases based on wave; increments/multiplies each time

    private List<GameObject> enemies;
    public List<GameObject> spawnZones;

    //Perhaps you can set up enemy type as a struct (taking in name, prefab). We'll only have one for this project, but it's nice to have expandability in mind

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

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
                GameObject enemy = Instantiate(enemyPrefab, spawnZones[i].transform.position, Quaternion.identity);
                enemies.Add(enemy);
            }

            spawnCooldown = 5.0f;
        }

        //Re-order spawnzones based on angle to camera.forward delta
        //Do you really need to iterate through this list again? You certainly don't need to do this reordering each update.
        for (int i = 0; i < spawnZones.Count; i++)
        {
            Vector3 angleToPlayer = player.transform.position - spawnZones[i].transform.position;   //This won'tchange at runtime! Set it in start.
            float angleDelta = Vector3.Angle(Camera.main.transform.forward, angleToPlayer);

            //Iterate through a list of angles, sort them, and reorder enemies list accordingly.
        }
    }

    private bool EnemySpawnCooldown()
    {
        spawnCooldown -= Time.deltaTime;
        return spawnCooldown <= 0;
    }

    public void RemoveDeadEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
}
