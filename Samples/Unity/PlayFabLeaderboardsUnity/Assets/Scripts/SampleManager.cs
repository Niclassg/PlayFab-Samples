using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleManager : MonoBehaviour
{
	[SerializeField] private InputField inputScoreObject;

	private void Start()
	{
		if (PlayfabManager.IsLoggedIn == false)
		{
			PlayfabManager.Login(OnLoginSuccess, OnLoginFailed);
		}
	}

	private void OnGotLeaderboards()
	{
		GoToLeaderboards();
	}
	private void OnGotLeaderboardsFailed()
	{
		Debug.LogError("ERROR: Failed to retrieve Leaderboards. Verify configuration on PlayFab GameManager matches the supplied documentation.");
	}

	private void OnLoginSuccess(LoginResult result)
	{
		RefreshLeaderboards();
	}

	private void OnLoginFailed(PlayFabError error)
	{
		Debug.LogError("ERROR: Login Failed. Verify Title ID is set correctly.");
		Debug.LogError(error.GenerateErrorReport());
	}

	// Selected leaderboard
	private int currentLeaderboard;

	// UX strings for the leaderboards
	private readonly string[,] leaderboardInfo =
	{
		{ "best_time", "Best Time", "Seconds" },
		{ "high_score", "High Scores", "Points" },
		{ "total_score", "Total Scores", "Total Points" }
	};

	public void RefreshLeaderboards()
	{
		PlayfabManager.LoadLeaderboards(OnGotLeaderboards, OnGotLeaderboardsFailed);
	}

	private void GoToLeaderboards()
	{
		ShowLeaderboard(currentLeaderboard);
	}

	public void NextLeaderboard()
	{
		currentLeaderboard++;

		if (currentLeaderboard == leaderboardInfo.GetLength(0))
		{
			currentLeaderboard = 0;
		}

		//RefreshLeaderboards();

		ShowLeaderboard(currentLeaderboard);
	}

	public void PrevLeaderboard()
	{
		currentLeaderboard--;

		if (currentLeaderboard < 0)
		{
			currentLeaderboard = leaderboardInfo.GetLength(0) - 1;
		}

		//RefreshLeaderboards();
	}

	private void ShowLeaderboard(int boardIndex)
	{
		var list = GameObject.Find("Score List");
		var script = list.GetComponent<ScoreList>();

		script.ShowLeaderboard(
			leaderboardInfo[currentLeaderboard, 0],
			leaderboardInfo[currentLeaderboard, 1],
			leaderboardInfo[currentLeaderboard, 2]
			);
	}

	public void PostBestTime()
	{
		if (int.TryParse(inputScoreObject.text, out var value))
		{
			Debug.Log($"PostBestTime: Posted best time of {value}");
			PlayfabManager.UpdateStatistic("best_time", value);
		}
		else
		{
			Debug.Log($"Warning: Invalid Numeric Value: {inputScoreObject.text}");
		}
	}

	public void PostHighScore()
	{
		if (int.TryParse(inputScoreObject.text, out var value))
		{
			Debug.Log($"PostHighScore: Posted high score of {value}");
			PlayfabManager.UpdateStatistic("high_score", value);
		}
		else
		{
			Debug.Log($"Warning: Invalid Numeric Value: {inputScoreObject.text}");
		}
	}

	public void PostNewScore()
	{
		if (int.TryParse(inputScoreObject.text, out var value))
		{
			Debug.Log($"PostNewScore: Posted new score of {value}");
			PlayfabManager.UpdateStatistic("total_score", value);
		}
		else
		{
			Debug.Log($"Warning: Invalid Numeric Value: {inputScoreObject.text}");
		}
	}
}
