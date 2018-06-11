using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [SerializeField] private GameObject bossHealthBar; // used only when boss appeared.
    [SerializeField] private Slider bossHealthSlider;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text restartText;
    [SerializeField] private Text gameOverText;
    private bool gameOver;
    private int score;

    void Start() {
        gameOver = false;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        UpdateScore();
    }

    void Update() {
        if (gameOver && Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene("Main");
        }
    }

    public void SetBossHeathBarActive(bool active) {
        bossHealthBar.SetActive(active);
        if(active) {
            bossHealthSlider.value = bossHealthSlider.maxValue; 
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
        ShowRestartText();
        gameOver = true;               
    }

    private void ShowRestartText() {
        restartText.text = "Press 'R' for Restart";
    }

    public void YouWin() {
        gameOverText.text = "You Win!";
        gameOver = true;
    }
}
