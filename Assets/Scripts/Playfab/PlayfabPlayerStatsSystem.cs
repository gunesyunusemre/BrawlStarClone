using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class PlayfabPlayerStatsSystem : MonoBehaviour
{
    public static PlayfabPlayerStatsSystem PFSS;
    public int exp;
    private void OnEnable()
    {
        PlayfabPlayerStatsSystem.PFSS = this;
        GetStats();
    }

    public void OnClick_SetStats()
    {
        Prapare_StatsForSetting();
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<StatisticUpdate> {
        new StatisticUpdate { StatisticName = "Exp", Value = exp },
    }
        },
    result => { Debug.Log("User statistics updated"); },
    error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    public void GetStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStats,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    void OnGetStats(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
        {
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
            switch (eachStat.StatisticName)
            {
                case "Exp":
                    exp = eachStat.Value;
                    break;
            }
        }
    }

    private void Prapare_StatsForSetting()
    {
        GetStats();
        exp = exp + 10;
        //another stats
    }


    ///
    #region ForSettingCloud
    /* public void StartCloudUpdatePlayerStats()
     {
         playerScore = PlayerScripts.PlayerS.Score * 7;
         PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
         {
             FunctionName = "UpdatePlayerStats", // Arbitrary function name (must exist in your uploaded cloud.js file)
             FunctionParameter = new { Score = playerScore }, // The parameter provided to your function
             GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
         }, OnCloudUpdateStats, OnErrorShared);
     }
     // OnCloudHelloWorld defined in the next code block

     private static void OnCloudUpdateStats(ExecuteCloudScriptResult result)
     {
         // CloudScript returns arbitrary results, so you have to evaluate them one step and one parameter at a time
         //Debug.Log(PlayFab.PfEditor.Json.JsonWrapper.SerializeObject(result.FunctionResult));
         JsonObject jsonResult = (JsonObject)result.FunctionResult;
         object messageValue;
         jsonResult.TryGetValue("messageValue", out messageValue); // note how "messageValue" directly corresponds to the JSON values set in CloudScript
         Debug.Log((string)messageValue);
     }

     private static void OnErrorShared(PlayFabError error)
     {
         Debug.Log(error.GenerateErrorReport());
     }*/
    #endregion
    ///

}
