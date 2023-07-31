using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip backgroundClip;
    public AudioClip endingClip;
    public static GameManager instance;
    public Text scoreText;
    public Text coinText;
    public Text timeText;
    public bool isGameover = false;
    public bool isClear = false;

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
        if(isGameover)
        {
            StartCoroutine(newGame(5f));
        }

        RemainedTime();
        timeText.text = time.ToString("F0");
        scoreText.text = currentScore.ToString("D6");

        if (isGameover && Input.anyKey)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int score) {
        if (!isGameover) {
            currentScore += score;
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
        if (isClear)
            return;

        if (time <= 0){
            isGameover = true;
            return;
        }

        time -= Time.deltaTime;
    }

    public void ClearStage() {
        isClear = true;
        StartCoroutine(ShowScoreReplacement());
    }

    private System.Collections.IEnumerator ShowScoreReplacement() {
        int bonusScore = Mathf.FloorToInt(time) * 100;

        float timeToShow = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < timeToShow) {
            // 시간을 빠르게 감소시키고 점수를 빠르게 증가시킴
            float timeStep = Mathf.Lerp(time, 0f, elapsedTime / timeToShow);
            int scoreStep = (int)Mathf.Lerp(0, bonusScore, elapsedTime / timeToShow);
            timeText.text = timeStep.ToString("F0");
            scoreText.text = (currentScore + scoreStep).ToString("D6");

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        time = 0.0f;
        currentScore += bonusScore;

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private System.Collections.IEnumerator newGame(float d) {
        yield return new WaitForSecondsRealtime(d);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
