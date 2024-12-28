using UnityEngine;
using System.IO;

public class JsonDataService : IDataService
{
    private string dataFilePath;

    public JsonDataService(string fileName)
    {
        dataFilePath = Path.Combine(Application.persistentDataPath, fileName);
        Debug.Log(dataFilePath); // Use Debug.Log instead of print
    }

    public void SavePlayerData(PlayerData playerData)
    {
        string jsonData = JsonUtility.ToJson(playerData);
        File.WriteAllText(dataFilePath, jsonData);
    }

    public PlayerData LoadPlayerData()
    {
        if (File.Exists(dataFilePath))
        {
            string jsonData = File.ReadAllText(dataFilePath);
            return JsonUtility.FromJson<PlayerData>(jsonData);
        }
        else
        {
            Debug.LogWarning("Player data file not found.");
            return null;
        }
    }
}
