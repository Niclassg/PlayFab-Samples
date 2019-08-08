using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
using System;
using System.Linq;

public class ScoreList : MonoBehaviour
{
	[SerializeField] private Entry entryPrefab;
	[SerializeField] private Text titlePanelText;
	[SerializeField] private Text headerPanelText;

    // Use this for initialization
    public void ShowLeaderboard (string boardName, string boardTitle, string valueName)
    {
        // Remove any existing entries
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Get the leaderboard data
        var leaderboards = PlayfabManager.GetLeaderboard(boardName);

		if (leaderboards.Any())
		{
			// Create UI entries for each
			foreach (var leaderboard in leaderboards)
			{
				var entry = Instantiate(entryPrefab, transform, false);
				entry.Set(new EntryModel(
					(leaderboard.Position + 1).ToString(),
					leaderboard.DisplayName ?? leaderboard.PlayFabId,
					leaderboard.StatValue.ToString()
					));
			}
		}
		else
		{
			var entry = Instantiate(entryPrefab, this.transform, false);
			entry.Set(new EntryModel(
				"Leaderboard is empty",
				"",
				""
				));
		}

        // Set the label text
        titlePanelText.text = boardTitle;
        headerPanelText.text = valueName;
    }
}
