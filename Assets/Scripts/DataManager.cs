using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
using Leguar.TotalJSON;

public static class DataManager
{
    static int savedHighscore = 0;
    public static int GetHighScore()
    {
        return savedHighscore;
    }
}
