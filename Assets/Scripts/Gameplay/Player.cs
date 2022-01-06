using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] int lives = 3;
    [SerializeField] float scoreTick = 1f;
    WaitForSeconds delay;

    [SerializeField] DOTweenAnimation cameraAnimation;

    int score;
    GameUI gameUI;
    
    void Start()
    {
        GameManager.OnGameStateChange += CheckGameState;
        gameUI = GameUI.instance;
        delay = new WaitForSeconds(scoreTick);

        if (!cameraAnimation)
            cameraAnimation = Camera.main.GetComponent<DOTweenAnimation>();
    }

    void CheckGameState(GameState state)
    {
        if(state == GameState.PLAY)
        {
            StartCoroutine(CalculateScore());
        }
        if (state == GameState.GAMEOVER)
        {
            SaveHighscore();
            GameManager.OnGameStateChange -= CheckGameState;
        }
    }

    IEnumerator CalculateScore()
    {
        while(GameManager.instance.GetGameState() == GameState.PLAY)
        {
            yield return delay;

            ++score;
            gameUI.UpdateHighscore(score);

            yield return null;
        }

        yield break;
    }

    void SaveHighscore()
    {
        int oldScore = PlayerPrefs.GetInt("Highscore", 0);

        if(score > oldScore)
            PlayerPrefs.SetInt("Highscore", score);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "WholeAsteroid")
        {
            cameraAnimation.DORestart();

            --lives;

            if (lives <= 0)
                GameManager.instance.UpdateGameState(GameState.GAMEOVER);
        }
    }
}
