using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public static class PlayfabManager
{
    public static void Login(Action<LoginResult> onSuccess, Action<PlayFabError> onFailed)

    {
        PlayFabClientAPI.LoginWithCustomID(
            // Request
            new LoginWithCustomIDRequest
            {
                CustomId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true
            },
            // Success
            result =>
            {
                Debug.Log("Login completed.");
                IsLoggedIn = true;
                onSuccess(result);
            },
            // Failure
            error =>
            {
                Debug.LogError("Login failed.");
                Debug.LogError(error.GenerateErrorReport());
                onFailed(error);
            }
        );
    }

    // Flag set after successfull Playfab Login
    public static bool IsLoggedIn;

    public static void UpdateStatistic(string stat, int value)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate {StatisticName = stat, Value = value}
                }
            },
            response => { Debug.Log("UpdateStatistic (UpdateStatistic) completed."); },
            // Failure
            error =>
            {
                Debug.LogError("UpdateStatistic failed.");
                Debug.LogError(error.GenerateErrorReport());
            }
        );
    }

    // List of server configured leaderboards
    private static readonly string[] LeaderboardList =
    {
        "best_time",
        "high_score",
        "total_score"
    };

    private static int leaderboardSuccessCount;
    private static int leaderboardErrorCount;

    private static void OnLeaderboardResult(bool bSuccess, Action onSuccess, Action onFailed)
    {
        if (bSuccess)
        {
            ++leaderboardSuccessCount;
        }
        else
        {
            ++leaderboardErrorCount;
        }

        // Are we done?
        if (leaderboardSuccessCount + leaderboardErrorCount == LeaderboardList.Length)
        {
            if (leaderboardErrorCount == 0)
            {
                onSuccess();
            }
            else
            {
                onFailed();
            }
        }
    }

    ////////////////////////////////////////////////////////////////
    /// Load the leaderboard data
    /// 
    public static void LoadLeaderboards(Action onSuccess, Action onFailed)
    {
        leaderboardSuccessCount = 0;
        leaderboardErrorCount = 0;

        LeaderboardData = new Dictionary<string, List<PlayerLeaderboardEntry>>();

        foreach (var board in LeaderboardList)
        {
            PlayFabClientAPI.GetLeaderboard(
                // Request
                new GetLeaderboardRequest
                {
                    StatisticName = board,
                    StartPosition = 0,
                    MaxResultsCount = MaxLeaderboardEntries
                },
                // Success
                result =>
                {
                    var boardName = (result.Request as GetLeaderboardRequest)?.StatisticName;
                    if (boardName != null)
                    {
                        LeaderboardData[boardName] = result.Leaderboard;
                        Debug.Log($"GetLeaderboard completed: {boardName}");
                    }

                    OnLeaderboardResult(true, onSuccess, onFailed);
                },
                // Failure
                error =>
                {
                    Debug.LogError("GetLeaderboard failed.");
                    Debug.LogError(error.GenerateErrorReport());
                    OnLeaderboardResult(false, onSuccess, onFailed);
                }
            );
        }
    }

    // Max entries to retrieve; based on UI space
    private static int MaxLeaderboardEntries = 7;

    // Cache for leaderboard data
    private static Dictionary<string, List<PlayerLeaderboardEntry>> LeaderboardData;

    // Access for leaderboards
    public static List<PlayerLeaderboardEntry> GetLeaderboard(string board)
    {
        return LeaderboardData[board];
    }
}