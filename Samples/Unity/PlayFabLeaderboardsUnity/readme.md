# PlayFab Leaderboards Unity Sample
This sample was created with Unity 2018.4

### Description

This sample demonstrates reading and writing from/to leaderboards from within a Unity title. The sample covers lowest score/best time, high score and cumulative score leaderboards.

**PlayFab Configuration**

* Create the 'best_time' leaderboard
  * __Statistic Name:__ best_time
  * __Reset Frequency:__ Manually
  * __Aggregation Method:__ Minimum
* Create the 'high_score' leaderboard
  * __Statistic Name:__ high_score
  * __Reset Frequency:__ Manually
  * __Aggregation Method:__ Maximum
* Create the 'total_score' leaderboard
  * __Statistic Name:__ total_score
  * __Reset Frequency:__ Manually
  * __Aggregation Method:__ Sum

Enable clients are allowed to post scores.

 ### Using the Sample
 
**Requirements**
* PlayFab title (created on PlayFab.com)
  * Title must be configured correctly (see PlayFab Configuration above)
* Initial Setup
  * Update PlayFabSharedSettings.asset to replace the TitleID value with your title ID.

**Using the Sample**

* Use the left and right arrows to switch leaderboards
* Enter a numeric value in the Value box and press any of the post buttons to post that value as the relevant score type
* Press Refresh to retrieve the latest leaderboard data
