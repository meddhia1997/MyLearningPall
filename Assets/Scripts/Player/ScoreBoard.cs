using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json; // Import Json.NET for JSON parsing
using Newtonsoft.Json.Linq; // Import JObject and JArray

public class ScoreboardManager : MonoBehaviour
{
    public Text scoreboardText; // Reference to the UI Text element where scoreboard will be displayed
    public Scrollbar scrollbar; // Reference to the Scrollbar component for vertical scrolling (optional)

    void Start()
    {
        StartCoroutine(FetchScoreboardData());
    }

    IEnumerator FetchScoreboardData()
    {
        string url = "http://localhost:3000/playerdata/times"; // Replace with your actual Node.js server URL

        using (WWW www = new WWW(url))
        {
            yield return www;

            if (www.error != null)
            {
                Debug.Log("Error fetching scoreboard data: " + www.error);
                yield break;
            }

            // Parse JSON data using Json.NET
            JArray jsonArray = JArray.Parse(www.text);
            ScoreboardData[] scoreboardData = ParseJSON(jsonArray);

            // Update UI
            UpdateScoreboardUI(scoreboardData);
        }
    }

    ScoreboardData[] ParseJSON(JArray jsonArray)
    {
        List<ScoreboardData> dataList = new List<ScoreboardData>();

        // Iterate over each item in the JArray
        foreach (JObject jsonObject in jsonArray)
        {
            // Check if 'time' field exists in the current object
            if (jsonObject["time"] != null)
            {
                ScoreboardData data = new ScoreboardData();
                data.username = jsonObject["username"].ToString();
                data.time = jsonObject["time"]["content"].ToString();
                dataList.Add(data);
            }
        }

        return dataList.ToArray();
    }

    void UpdateScoreboardUI(ScoreboardData[] data)
    {
        // Clear previous scoreboard entries if any
        scoreboardText.text = "";

        // Update UI with fetched data
        foreach (ScoreboardData entry in data)
        {
            scoreboardText.text += $"{entry.username}: {entry.time}\n\n"; // Add spacing between entries
        }

        // Adjust RectTransform to fit content
        RectTransform rt = scoreboardText.rectTransform;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, scoreboardText.preferredHeight);

        // Adjust scrollbar value to scroll to top (optional)
        if (scrollbar != null)
        {
            scrollbar.value = 1f;
        }
    }
}

[System.Serializable]
public class ScoreboardData
{
    public string username;
    public string time;
}
