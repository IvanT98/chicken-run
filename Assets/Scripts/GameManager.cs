using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public GameObject gameScreen;

    private bool gameOver = true;

    public bool isGameOver() {
        return gameOver;
    }

    public void StartGame() {
        titleScreen.SetActive(false);
        gameScreen.SetActive(true);
        gameOver = false;
    }

    public void GameOver() {
        gameOver = true;
        gameOverScreen.SetActive(true);
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
