using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _backToMainMenu;
    [SerializeField] private Text _restartText;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Image _currLivesImg;
    [SerializeField] private Image _currLivesImgTwo;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private Sprite[] _liveSpritesTwo;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.gameObject.SetActive(true);
        _scoreText.text = "Score: 0";
        InitialHighScore();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gameManager == null)
            Debug.LogError("_gameManager is NULL!!! UIManager::start()");
    }

    public void UpdateScore(int Score)
    {
        _scoreText.text = "Score: " + Score.ToString();      
    }

    public void InitialHighScore()
    {
        _highScoreText.gameObject.SetActive(true);
        _highScoreText.text = "Best Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    void UpdateHighScore()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
            Debug.LogError("player is NULL!!! UIManager::UpdateHighScore()");

        if (player.GetScore() > PlayerPrefs.GetInt("HighScore", 0))
            PlayerPrefs.SetInt("HighScore", player.GetScore());
    }

    public void UpdateLives(int currLives, bool _isPlayerOne)
    {
        if (_isPlayerOne)
            _currLivesImg.sprite = _liveSprites[currLives];
        else
            _currLivesImgTwo.sprite = _liveSpritesTwo[currLives];
            
        if (currLives <= 0)
            GameOverSequence();
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.text = "Game Over";
            yield return new WaitForSeconds(0.1f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.2f);
        }
    }

    void GameOverSequence()
    {
        _gameOverPanel.SetActive(true);
        UpdateHighScore();
        StartCoroutine(GameOverFlicker());
        _gameManager.SetIsGameOver(true);
    }
}


