using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text scoreText;
    public Text coinText;
    public Text timeText;
    public Text worldText;
    
    public bool isGameover = false;
    private int totalScore;
    private int currentScore = 0;
    private int[] stageScore = new int[3];
    private int coinCount = 0;
    private float time = 300.0f;

    void Awake() {
        if (instance == null)
            instance = this;
        else {
            Debug.LogWarning("More than one Game Manager exists!");
            Destroy(gameObject);
        }
    }

    void Update() {
        RemainedTime();

        if (isGameover && Input.anyKey)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int score) {
        if (!isGameover) {
            currentScore += score;
            scoreText.text = currentScore.ToString("D6");
        }
    }

    public void UpdateScore(int n) {
        if (currentScore > stageScore[n]) {
            totalScore -= stageScore[n];
            stageScore[n] = currentScore;
            totalScore += stageScore[n];
        }
    }

    public void AddCoinCount(int count) {
        coinCount += count;
        coinText.text = "x" + coinCount.ToString("D2"); 
    }

    public void RemainedTime() {
        if (time <= 0) {
            isGameover = true;
        }

        time -= Time.deltaTime;
        time = (int)time;
        timeText.text = time.ToString("D3");
    }
}
