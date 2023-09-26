﻿using System.Collections;
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

    private bool _isGameOver = false;
    private bool _isPause = false;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    /// </summary>
    void Start()
    {
        _pauseMenu = GameObject.Find("Canvas").transform.Find("Pause Menu Panel")?.gameObject;
        _spawnManager = GameObject.Find("Spawn Manager")?.GetComponent<SpawnManager>();
        _pausedAnimator = GameObject.Find("Pause Menu Panel").GetComponent<Animator>();
        _pausedAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        _gameMusic = GameObject.Find("Audio Manager");
        //_spawnManager.StartSpawning();
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