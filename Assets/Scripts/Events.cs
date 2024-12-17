using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Events : MonoBehaviour
{
    public GameObject ExitPanel;

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void Level()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenLevelExitPanel()
    {
        if (ExitPanel != null)
        {
            ExitPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void CloseLevelExitPanel()
    {
        if (ExitPanel != null)
        {
            ExitPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
