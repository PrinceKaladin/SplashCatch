using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("UI")]
    public Text scoreText;
    public Text timeText;

    [Header("Game")]
    public float gameTime = 60f;

    int score;
    float timeLeft;
    bool isGameOver;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        timeLeft = gameTime;
        UpdateUI();
    }

    private void Update()
    {
        if (isGameOver) { SceneManager.LoadScene(3); };

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            isGameOver = true;
            // тут можешь остановить спавн/управление
        }

        UpdateUI();
    }

    public bool IsGameOver => isGameOver;

    public void AddScore(int delta)
    {
        if (isGameOver) return;
        score += delta;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText) scoreText.text = $"{score}";
        PlayerPrefs.SetInt("score", score);
        if (timeText) timeText.text = $"Time: {Mathf.CeilToInt(timeLeft)}";
    }
}
