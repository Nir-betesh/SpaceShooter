using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The GameManager class manages game-related functionality such as game over state and scene loading.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] public bool _isSingleMode;

    private GameObject _pauseMenu;
    private GameObject _gameMusic;
    private SpawnManager _spawnManager;
    private Animator _pausedAnimator;
    private Animator _gameOverAnimator;

    private bool _isGameOver = false;
    private bool _isPause = false;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    /// </summary>
    void Start()
    {
        _pauseMenu = GameObject.Find("Pause Menu Panel").gameObject;
        if (_pauseMenu == null)
            Debug.LogError("_pauseMenu is NULL! GameManager::start()");

        _spawnManager = GameObject.Find("Spawn Manager")?.GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("_spawnManager is NULL! GameManager::start()");

        _gameMusic = GameObject.Find("Audio Manager");
        if (_gameMusic == null)
            Debug.LogError("_gameMusic is NULL! GameManager::start()");


        _gameOverAnimator = GameObject.Find("Game Over Panel").GetComponent<Animator>();
        if (_gameOverAnimator == null)
            Debug.LogError("_gameOverAnimator is NULL! GameManager::start()");
        _gameOverAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;

        _pausedAnimator = GameObject.Find("Pause Menu Panel").GetComponent<Animator>();
        if (_pausedAnimator == null)
            Debug.LogError("_pausedAnimator is NULL! GameManager::start()");
        _pausedAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        if (_isGameOver && Input.GetKeyDown(KeyCode.B))
            SceneManager.LoadScene("Main_Menu");
        
        // Check if "R" key press to restart the game if the game over.
        if (_isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            //GameObject.Find("Canvas").GetComponent<UIManager>().InitialBestScore();
            if (_isSingleMode)
                SceneManager.LoadScene("Single_Player"); // Possible to load by String (Scene name: "Single_Player") Or Load by int (Scene index: 1). SC
            else
                SceneManager.LoadScene("Two_Players");

            _spawnManager.StartSpawning();
            SetIsGameOver(false);
        }

        if (Input.GetKeyDown(KeyCode.P) && !_isGameOver && !_isPause)
             OnPause(0, false, true);  

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    /// <summary>
    /// Marks the game as over.
    /// </summary>
    public void SetIsGameOver(bool answer)
    {
        _isGameOver = answer;
        if (answer == true)
            _gameOverAnimator.SetBool("isGameOverPanelActive", true);
    }

    public bool IsSingleMode()
    {
        return _isSingleMode;
    }
    public void ResumeGameBtn()
    {
        OnPause(1, true, false);
    }
    public void BackToMainMenuBtn()
    {
        SceneManager.LoadScene("Main_Menu");
        Time.timeScale = 1;
    }
    public void OnPause(int gameSpeed, bool isMusicOn, bool isGamePause) {
        Time.timeScale = gameSpeed;
        _gameMusic.SetActive(isMusicOn);
        _pauseMenu.SetActive(isGamePause);
        if (isGamePause)
            _pausedAnimator.SetBool("isPaused", isGamePause);
        _isPause = isGamePause;
    }
}
