using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MenuScoreDisplay : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> sessionTimeTexts; 
    [SerializeField] private List<TMP_Text> highScoreTexts;   

    private void Start()
    {
        UpdateSessionTimeTexts();
        UpdateHighScoreTexts();
    }

    public void UpdateSessionTimeTexts()
    {
        Dictionary<string, float> currentSessionTimes = LevelTimer.instance.GetCurrentSessionTimes();

        for (int i = 0; i < sessionTimeTexts.Count; i++)
        {
            string levelName = $"Level {i + 1}";
            if (currentSessionTimes.ContainsKey(levelName))
            {
                float time = currentSessionTimes[levelName];
                sessionTimeTexts[i].text = FormatTime(time);
            }
            else
            {
                sessionTimeTexts[i].text = "N/A";
            }

            Debug.Log($"SessionTimeText[{i}]: {sessionTimeTexts[i].text}");
        }
    }

    public void UpdateHighScoreTexts()
    {
        for (int i = 0; i < highScoreTexts.Count; i++)
        {
            string levelName = $"Level {i + 1}";
            float highScore = LevelTimer.instance.GetLevelTime(levelName);

            if (highScore >= 0)
            {
                highScoreTexts[i].text = FormatTime(highScore);
            }
            else
            {
                highScoreTexts[i].text = "N/A";
            }

            Debug.Log($"HighScoreText[{i}]: {highScoreTexts[i].text}");
        }

        Debug.Log("High scores updated.");
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        int milliseconds = Mathf.FloorToInt((time * 1000F) % 1000F);

        return $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }
}
