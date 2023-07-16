using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //public Text scoreText;
    
    public bool isGameover = false;
    private int totalScore;
    private int currentScore;
    private int[] stageScore = new int[3];

    void Awake() {
        if (instance == null)
            instance = this;
        else {
            Debug.LogWarning("More than one Game Manager exists!");
            Destroy(gameObject);
        }
    }

    void Update() {
        if (isGameover && Input.anyKey)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int score) {
        if (!isGameover) {
            currentScore += score;
            // scoreText.text = "Score : " + score;
        }
    }

    public void UpdateScore(int n) {
        if (currentScore > stageScore[n]) {
            totalScore -= stageScore[n];
            stageScore[n] = currentScore;
            totalScore += stageScore[n];
        }
    }
}
