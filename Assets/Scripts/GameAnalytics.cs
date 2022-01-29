using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GameAnalytics : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] PlayerController pc;
    [SerializeField] ObjectSpawner os;

    void Start()
    {
        GameManager.OnGameStateChange += CheckGamestate;
    }

    void CheckGamestate(GameState state)
    {
        if (state == GameState.GAMEOVER)
            SendAnalytics();
    }

    void SendAnalytics()
    {
        Analytics.CustomEvent("Score",
            new Dictionary<string, object>{
                { "Score",  player.GetScore() }
            });

        Analytics.CustomEvent("Play Duration",
         new Dictionary<string, object>{
                { "Duration",  player.GetDuration() }
            });

        Analytics.CustomEvent("Max Speed Achieved",
            new Dictionary<string, object>{
                { "Speed",  pc.movementSpeed}
            });

        Analytics.CustomEvent("Difficulty Level",
            new Dictionary<string, object>{
                { "Difficulty",  os.GetCurrentDifficulty()}
            });

        GameManager.OnGameStateChange -= CheckGamestate;
    }
}
