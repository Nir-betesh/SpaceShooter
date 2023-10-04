using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The MainMenu class represents the initial screen when the application is launched.
/// It handles the functionality of loading into the Main Menu scene.
/// </summary>
public class MainMenu : MonoBehaviour
{
    private bool _isSingleMode;
    [SerializeField] GameObject _infoPanel;
    private bool _isPressedInfo  = false;

    /// <summary>
    /// Loads the Main Menu scene.
    /// </summary>
    public void LoadSinglePlayerGameBtn()
    {
        SetIsSingleMode(true);
        SceneManager.LoadScene("Single_Player");
    }
    
    public void LoadTwoPlayersGameBtn()
    {
        SetIsSingleMode(false);
        SceneManager.LoadScene("Two_Players");
    }
    
    public void InformationBtn()
    {
        if (!_isPressedInfo)
        {
            _infoPanel.SetActive(true);
            _isPressedInfo = true;
        }
        else
        {
            _infoPanel.SetActive(false);
            _isPressedInfo = false;
        }
    }

    public void SetIsSingleMode(bool _isSingle)
    {
        _isSingleMode = _isSingle;
    }

    public bool GetIsSingleMode()
    {
        return _isSingleMode;
    }
}
