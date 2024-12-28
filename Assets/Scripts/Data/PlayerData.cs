using System;

[System.Serializable]
public class PlayerData
{
    public string username;
    public string parentEmail;
    public string skinName;
    public PlayerAction[] PlayerResponses;
    public PlayerAction[] WritingTests;
    public PlayerAction[] PronunciationAccuracies;

    public PlayerData()
    {
        PlayerResponses = new PlayerAction[0];
        WritingTests = new PlayerAction[0];
        PronunciationAccuracies = new PlayerAction[0];
    }

    // Method to add a new entry to PlayerResponses array with the current date
    public void AddPlayerResponse(string response)
    {
        AddAction(ref PlayerResponses, response);
    }

    // Method to add a new entry to WritingTests array with the current date
    public void AddWritingTest(string test)
    {
        AddAction(ref WritingTests, test);
    }

    // Method to add a new entry to PronunciationAccuracies array with the current date
    public void AddPronunciationAccuracy(string accuracy)
    {
        AddAction(ref PronunciationAccuracies, accuracy);
    }

    // Method to add a new action with the current date to the provided array
    private void AddAction(ref PlayerAction[] actionsArray, string action)
    {
        PlayerAction newAction = new PlayerAction(action, DateTime.Now.ToString("yyyy-MM-dd"));
        Array.Resize(ref actionsArray, actionsArray.Length + 1);
        actionsArray[actionsArray.Length - 1] = newAction;
    }
}

[System.Serializable]
public class PlayerAction
{
    public string action;
    public string date;

    public PlayerAction(string action, string date)
    {
        this.action = action;
        this.date = date;
    }
}
