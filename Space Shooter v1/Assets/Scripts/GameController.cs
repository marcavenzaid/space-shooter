using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject[] hazards;
    public Vector3 spawnValues;
    public GameObject bossHealthBar; // used only when boss appeared.
    public Slider healthSlider;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public GUIText scoreText;
    public GUIText restartText;
    public GUIText gameOverText;

    private bool gameOver;
    private int score;

    void Start() {
        gameOver = false;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        UpdateScore();
        //StartCoroutine(SpawnWaves());
    }

    void Update() {
        if (gameOver) {
            restartText.text = "Press 'R' for Restart";
            if (Input.GetKeyDown(KeyCode.R)) {
                SceneManager.LoadScene("Main");
            }
        }
    }

    // This function is called in the DestroyByContact class
    public void AddScore(int newScoreValue) {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore() {
        scoreText.text = "Score: " + score;
    }

    // This function is called in the DestroyByContact class
    // and BeamCollision
    public void GameOver() {
        gameOverText.text = "Game Over!";
        gameOver = true;               
    }

    public void YouWin() {
        gameOverText.text = "You Win!";
        gameOver = true;
    }
}
