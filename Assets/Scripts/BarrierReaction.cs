using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierReaction : MonoBehaviour
{   
    public int count = 2;
    private GameManager gameManager;
    private GameObject powerupIndicator;
    private GameObject player;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        powerupIndicator = GameObject.Find("PowerupIndicator");
    }
     private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            other.gameObject.SetActive(false);
            gameManager.GameOver();
            Debug.Log("Lose");

  
        }

        if (other.CompareTag("Enemy") || other.CompareTag("GroundBarrier") ){
            if (powerupIndicator != null){
                powerupIndicator.transform.localScale =  powerupIndicator.transform.localScale * 1.2f; 
            }
            Debug.Log("-1 enemy");
            Destroy(other.gameObject);
            count = count -1;
        }
        
    }
}
