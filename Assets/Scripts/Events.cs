using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Ajouté pour Button
using TMPro;

public class Events : MonoBehaviour
{
    public GameObject ExitPanel;
    public Button[] levelButtons;  // Boutons des niveaux
    public TextMeshProUGUI selectedLevelText; // Affichage texte du niveau sélectionné
    private int selectedLevel = -1;

    // Sélectionne le niveau choisi par le joueur
    public void SelectLevel(int levelNumber)
    {
        selectedLevel = levelNumber;
        selectedLevelText.text = "Niveau selectionne: " + levelNumber;
    }



    // Charge la scène correspondant au niveau choisi
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

    // Affiche un message de notice (à personnaliser selon le besoin)
    public void ShowNotice()
    {
        SceneManager.LoadScene("Notice");
        // Ajouter la logique pour afficher une notice UI si nécessaire
    }
    public void MenuPreGame()
    {
        SceneManager.LoadScene("MenuPreGame");
        // Ajouter la logique pour afficher une notice UI si nécessaire
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        // Ajouter la logique pour afficher une notice UI si nécessaire
    }

    // Quitte le jeu
    public void QuitGame()
    {
        Application.Quit();
    }

    // Active le panneau de sortie et met le jeu en pause
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
