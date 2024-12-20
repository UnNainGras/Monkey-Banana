using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Ajouté pour Button
using TMPro;

public class Events : MonoBehaviour
{
    public GameObject ExitPanel;
    public Button[] levelButtons;  
    public TextMeshProUGUI selectedLevelText; 
    private int selectedLevel = -1;

    public void SelectLevel(int levelNumber)
    {
        selectedLevel = levelNumber;
        selectedLevelText.text = "Niveau selectionne: " + levelNumber;
    }



    public void PlaySelectedLevel()
    {
        if (selectedLevel >= 0 && selectedLevel <= 4)
        {
            SceneManager.LoadScene("Level " + selectedLevel);
        }
        else
        {
            selectedLevelText.text = "selectionnez un niveau pour jouer";
        }
    }

    public void Notice()
    {
        SceneManager.LoadScene("Notice");
    }
    public void MenuPreGame()
    {
        SceneManager.LoadScene("MenuPreGame");
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Notice2()
    {
        SceneManager.LoadScene("Notice2");
    }

    public void MenuHighScore()
    {
        SceneManager.LoadScene("MenuHighScore");
    }

    public void QuitGame()
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

    // Désactive le panneau de sortie et reprend le jeu
    public void CloseLevelExitPanel()
    {
        if (ExitPanel != null)
        {
            ExitPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
