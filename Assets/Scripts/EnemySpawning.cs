using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    [SerializeField] private List<Transform> enemiesPositions;
    public static List<bool> canSpawn;
    [SerializeField] private List<EnemyStats> enemies;
    [SerializeField] private GameObject enemyTemplate;

    void Start()
    {
        if (canSpawn == null) {
            canSpawn = new();
            for (int i = 0; i < enemies.Count; i++) {
                canSpawn.Add(true);
            }
        }

        for (int index = 0; index < enemies.Count; index++) {
            if (canSpawn[index]) {
                GameObject enemyObj = Instantiate(enemyTemplate);
                enemyObj.GetComponent<Encounter>().stats = enemies[index];
                enemyObj.GetComponent<Encounter>().index = index;
                enemyObj.GetComponent<Encounter>().sprite = enemies[index].image;
                enemyObj.transform.position = enemiesPositions[index].position;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
