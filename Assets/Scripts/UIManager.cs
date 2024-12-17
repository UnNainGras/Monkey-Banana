using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject blackScreenPanel;
    public Image blackScreen;
    public bool fadeToBlack, fadeFromBlack;
    public float fadeSpeed = 2f;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        UpdateFade();
    }

    private void UpdateFade()
    {
        if (fadeToBlack)
        {
            ActivatePanel();
            FadeToBlack();
        }
        else if (fadeFromBlack)
        {
            FadeFromBlack();
            if (blackScreen.color.a <= 0f)
            {
                DeactivatePanel();
            }
        }
    }

    private void FadeToBlack()
    {
        FadeScreen(1f);
        if (blackScreen.color.a >= 1f)
        {
            fadeToBlack = false;
        }
    }

    private void FadeFromBlack()
    {
        FadeScreen(0f);
        if (blackScreen.color.a <= 0f)
        {
            fadeFromBlack = false;
        }
    }

    private void FadeScreen(float targetAlpha)
    {
        Color currentColor = blackScreen.color;
        float newAlpha = Mathf.MoveTowards(currentColor.a, targetAlpha, fadeSpeed * Time.deltaTime);
        blackScreen.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
    }

    private void ActivatePanel()
    {
        if (blackScreenPanel != null && !blackScreenPanel.activeSelf)
        {
            blackScreenPanel.SetActive(true);
        }
    }

    private void DeactivatePanel()
    {
        if (blackScreenPanel != null && blackScreenPanel.activeSelf)
        {
            blackScreenPanel.SetActive(false);
        }
    }
}
