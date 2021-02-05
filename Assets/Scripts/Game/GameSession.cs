using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    int score = 0;
    public bool gameOver = false;
    public bool gameStarted = false;

    //Start Screen Panel

    GameObject startScreenPanel;

    //Game Over Panel
    Text curScoreText;
    GameObject gameOverPanel;
    GameObject currentScore;
    GameObject slider;
    GameObject playerHealthIcon;
    GameObject player;
    Text finalScoreText;
    Text bestScoreText;

    private void Awake()
    {
        SetUpSingleton();
        SetUpScreenResolution();
    }

    private static void SetUpScreenResolution()
    {
        int width = 500; // or something else
        int height = 900; // or something else
        bool isFullScreen = false;
        Screen.SetResolution(width, height, isFullScreen);
    }

    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        FindObjectOfType<GameSession>().gameStarted = true;
    }

    void Start()
    {
        curScoreText = GameObject.Find("CurScoreText").GetComponent<Text>();
        gameOverPanel = GameObject.Find("GameOverPanel");
        finalScoreText = gameOverPanel.transform.Find("FinalScoreText").gameObject.GetComponent<Text>();
        bestScoreText = gameOverPanel.transform.Find("BestScoreText").gameObject.GetComponent<Text>();
        startScreenPanel = GameObject.Find("StartScreenPanel");
        currentScore = GameObject.Find("CurScoreText");
        slider = GameObject.Find("Slider");
        playerHealthIcon = GameObject.Find("PlayerHealthIcon");
        player = GameObject.Find("Player");
    }

    void Update()
    {
        startScreenPanel.gameObject.SetActive(gameStarted);
        if (gameStarted)
        {
            currentScore.SetActive(false);
            slider.SetActive(false);
            playerHealthIcon.SetActive(false);
            player.SetActive(false);
        }
        //update current score text value
        curScoreText.text = "SCORE: " + score;

        //when the game is over, some objects have to change their activity state
        curScoreText.enabled = !gameOver;
        gameOverPanel.gameObject.SetActive(gameOver);

        if (gameOver)
        {
            //update score values on the game over screen and make sure
            finalScoreText.text = curScoreText.text;
            bestScoreText.text = "BEST: " + PlayerPrefs.GetInt("BestScore").ToString();
        }
    }

    public void StartGame()
    {
            gameStarted = !gameStarted;
            currentScore.SetActive(true);
            slider.SetActive(true);
            playerHealthIcon.SetActive(true);
            player.SetActive(true);
    }

    private void SetUpSingleton()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int scoreValue)
    {
        score += scoreValue;
        if (score > PlayerPrefs.GetInt("BestScore"))
            PlayerPrefs.SetInt("BestScore", score);
    }

    public void Restart()
    {
        gameOver = false;
        gameStarted = !gameStarted;
        score = 0;
        SceneManager.LoadScene("1");
        Destroy(gameObject);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
