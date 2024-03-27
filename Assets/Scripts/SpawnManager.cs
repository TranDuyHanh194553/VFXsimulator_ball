using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{   
    public GameObject enemyPrefab;
    public GameObject blueEnemyPrefab;
    public GameObject orangeEnemyPrefab;
    public GameObject greenEnemyPrefab;
    private float spawnRange = 20;
    public int enemyCount, playerCount;
    public int count;
    public GameObject powerupPrefab;
    public GameObject speedupPrefab;
    private GameManager gameManager;
    void Start()
    {   
        SpawnEnemyWave(2);

        Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
        Instantiate(speedupPrefab, GenerateSpawnPosition(), speedupPrefab.transform.rotation);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    // Spawn function
        void SpawnEnemyWave(int enemyNumber){

            int blueEnemyNumber = 1;
            int orangeEnemyNumber = 1; 
            int greenEnemyNumber = 1;

            float choice =  Random.Range(0.1f, 2.9f);
            float choiceBox = 1.0f;
            float choiceBox2 = 2.0f;

            for(int i = 0; i < enemyNumber; i++){
                Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
            }
            // SpecialEnemy spawning
            if (choice < choiceBox ){
                for(int i = 0; i < blueEnemyNumber; i++){
                    Instantiate(blueEnemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
                }
            }
            else if (choice < choiceBox2 ){
                for(int i = 0; i < orangeEnemyNumber; i++){
                    Instantiate(orangeEnemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
                }
            }
            else{
                for(int i = 0; i < greenEnemyNumber; i++){
                    Instantiate(greenEnemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
                }
            }
        }

    private Vector3 GenerateSpawnPosition(){
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 1.5f, spawnPosZ);
        return randomPos;
    }

    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        playerCount = FindObjectsOfType<PlayerController>().Length;
        int specialEnemyNumber = Random.Range(0, 1);

        if (enemyCount == 0 && count <2 && playerCount != 0) {
            SpawnEnemyWave(2);
            Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
            Instantiate(speedupPrefab, GenerateSpawnPosition(), speedupPrefab.transform.rotation);
            count ++;
            Debug.Log("count = " + count);

        }
        else if (playerCount == 1 && enemyCount == 0 && count == 2){
            gameManager.Victory();    
            return;
        }
    }
}
