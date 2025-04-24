using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardManager : MonoBehaviour
{
    public string gameId = "game123";
    public string userId = "user123";
    public int score;

    void Start()
    {
        score = 0;
    }

    public void SubmitScore()
    {
        StartCoroutine(SendScoreToServer());
    }

    public void GetLeaderboard()
    {
        StartCoroutine(GetLeaderboardFromServer());
    }

    private IEnumerator SendScoreToServer()
    {
        string url = "https://cgwork.ir/api/save_score";
        WWWForm form = new WWWForm();
        form.AddField("game_id", gameId);
        form.AddField("user_id", userId);
        form.AddField("score", score);

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log("Score submitted successfully!");
        }
    }

    private IEnumerator GetLeaderboardFromServer()
    {
        string url = "https://cgwork.ir/api/leaderborad";
        WWWForm form = new WWWForm();
        form.AddField("game_id", gameId);
        form.AddField("user_id", userId);

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log("Leaderboard data received!");
            // Process leaderboard data
        }
    }
}
