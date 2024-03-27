using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
//    private float spawnRate = 1.0f;
//    public List<GameObject> targets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI victoryText;
    private int score;
    public bool isGameActive;
    public Button restartButton;
    public FloatingJoystick joystick;
//    public GameObject titleScreen;
//    public GameObject[] targets2;

    void Start()
    {

    }
    public void StartGame(/*int difficulty*/)
    {   
        isGameActive = true;
//        StartCoroutine(SpawnTarget());
        score = 0;
//        scoreText.text = "Score: " + score;
        UpdateScore(0);
//        spawnRate /= difficulty;
//        titleScreen.gameObject.SetActive(false);
    }
        public void UpdateScore(int scoreToAdd){
            score += scoreToAdd;
            scoreText.text = "Score: " + score; 
        }
        public void GameOver(){
            gameOverText.gameObject.SetActive(true);
            joystick.gameObject.SetActive(false);
            victoryText.gameObject.SetActive(false);
            isGameActive = false;
            restartButton.gameObject.SetActive(true);
            return;
        }

        public void Victory(){
            victoryText.gameObject.SetActive(true);
            joystick.gameObject.SetActive(false);
            gameOverText.gameObject.SetActive(false);
            isGameActive = false;
            restartButton.gameObject.SetActive(true);
            return;
        }

        public void RestartGame(){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    
        void Update()
    {
    }
}
