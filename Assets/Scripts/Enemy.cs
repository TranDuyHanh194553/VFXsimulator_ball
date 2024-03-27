using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    public float speed;
    private Rigidbody enemyRb, playerRb;
    private GameObject player;
    // private GameObject powerupIndicator;
    private GameManager gameManager;
    private int pointValue = 5;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();

        player = GameObject.FindWithTag("Player");
        playerRb = player.GetComponent<Rigidbody>();;
        // powerupIndicator = GameObject.Find("PowerupIndicator");
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {   
        if (transform.position == null){
            return;
        }
        else {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            enemyRb.AddForce(1.5f * lookDirection * speed);
        }
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy")){
            Rigidbody enemyRigidbody =  collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromEnemy = (collision.gameObject.transform.position - transform.position);

            enemyRigidbody.AddForce(awayFromEnemy * 7 , ForceMode.Impulse);
        }
    }
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Barrier")){
            player.transform.localScale = player.transform.localScale * 1.05f; 
            playerRb.mass = playerRb.mass * 1.05f;
            // powerupIndicator.transform.localScale =  powerupIndicator.transform.localScale * 1.2f; 

            gameObject.SetActive(false);
            Debug.Log("-1 enemy");
            gameManager.UpdateScore(pointValue);
        }
    }

    
}
