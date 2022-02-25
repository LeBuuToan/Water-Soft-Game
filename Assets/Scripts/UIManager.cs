using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject settingPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SettingButton()
    {
        Time.timeScale = 0f;
        settingPanel.SetActive(true);
    }

    public void ResumeButton()
    {
        Time.timeScale = 1f;
        settingPanel.SetActive(false);
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }

    public void PlayGameButton()
    {
        SceneManager.LoadScene("Level 1");
        Time.timeScale = 1f;
    }

    public void Level1Active()
    {
        SceneManager.LoadScene("Level 1");
        GameController.instance.LevelNumber(1);
        GameController.instance.SeemColorNumber(0);
    }

    public void Level2Active()
    {
        SceneManager.LoadScene("Level 2");
        GameController.instance.LevelNumber(2);
        GameController.instance.SeemColorNumber(0);
    }

    public void Level3Active()
    {
        SceneManager.LoadScene("Level 3");
        GameController.instance.LevelNumber(3);
        GameController.instance.SeemColorNumber(0);
    }

    public void Level4Active()
    {
        SceneManager.LoadScene("Level 4");
        GameController.instance.LevelNumber(4);
        GameController.instance.SeemColorNumber(0);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
