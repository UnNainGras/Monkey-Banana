using UnityEngine;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public static LevelTimer instance;

    [SerializeField] private TMP_Text timerText;
    private bool isTimerRunning = false;

    private float startTime;
    private float elapsedTime;
    private Dictionary<string, float> levelTimes = new Dictionary<string, float>();
    private Dictionary<string, float> currentSessionTimes = new Dictionary<string, float>();
    private string saveFilePath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, "LevelTimes.json");
        LoadTimes();
    }

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null) 
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60F);
            int seconds = Mathf.FloorToInt(elapsedTime % 60F);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 1000F) % 1000F);

            timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
        }
    }

        public void StartLevelTimer()
    {
        startTime = Time.time;
        isTimerRunning = true;
    }

    public void StopLevelTimer(string levelName)
    {
        isTimerRunning = false;
        elapsedTime = Time.time - startTime;

        currentSessionTimes[levelName] = elapsedTime;

        if (levelTimes.ContainsKey(levelName))
        {
            if (elapsedTime < levelTimes[levelName])
            {
                levelTimes[levelName] = elapsedTime;
            }
        }
        else
        {
            levelTimes.Add(levelName, elapsedTime);
        }

        SaveTimes();
    }

    public float GetLevelTime(string levelName)
    {
        if (levelTimes.ContainsKey(levelName))
        {
            return levelTimes[levelName];
        }
        return -1;
    }

    private void SaveTimes()
    {
        string json = JsonUtility.ToJson(new LevelTimeData(levelTimes), true);
        File.WriteAllText(saveFilePath, json);
    }

    private void LoadTimes()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            LevelTimeData data = JsonUtility.FromJson<LevelTimeData>(json);
            levelTimes = data.ToDictionary();
        }
    }

    public Dictionary<string, float> GetCurrentSessionTimes()
    {
        return currentSessionTimes;
    }

    public void UpdateSessionTimeTexts()
    {
        Debug.Log("Session times updated:");
        foreach (var pair in currentSessionTimes)
        {
            Debug.Log($"Level: {pair.Key}, Time: {pair.Value}");
        }
    }
}

[System.Serializable]
public class LevelTimeData
{
    public List<string> levels = new List<string>();
    public List<float> times = new List<float>();

    public LevelTimeData(Dictionary<string, float> levelTimes)
    {
        foreach (var pair in levelTimes)
        {
            levels.Add(pair.Key);
            times.Add(pair.Value);
        }
    }

    public Dictionary<string, float> ToDictionary()
    {
        Dictionary<string, float> result = new Dictionary<string, float>();
        for (int i = 0; i < levels.Count; i++)
        {
            result[levels[i]] = times[i];
        }
        return result;
    }
}
