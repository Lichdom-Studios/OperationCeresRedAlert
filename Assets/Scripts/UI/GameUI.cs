using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    [SerializeField] TextMeshProUGUI countdown;
    [SerializeField] DOTweenAnimation countdownAnimation;

    [SerializeField] TextMeshProUGUI highscore;
    [SerializeField] GameObject gameoverScreen;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {

        GameManager.OnGameStateChange += CheckGameState;

    }
    void CheckGameState(GameState state)
    {
        switch(state)
        {
            case GameState.SETUP:
                break;
            case GameState.COUNTDOWN:
                StartCoroutine(DoCountdown());
                break;
            case GameState.READY:
                break;
            case GameState.PLAY:
                countdown.gameObject.SetActive(false);
                break;
            case GameState.GAMEOVER:
                gameoverScreen.SetActive(true);
                GameManager.OnGameStateChange -= CheckGameState;
                break;
        }
    }

    IEnumerator DoCountdown()
    {
        if (!countdownAnimation)
            countdownAnimation = countdown.GetComponent<DOTweenAnimation>();

        int second = 3;

        countdown.gameObject.SetActive(true);
        countdown.color = Color.red;

        while(GameManager.instance.GetGameState() == GameState.COUNTDOWN && second > 0)
        {
            countdownAnimation.DORestart();

            countdown.SetText(second.ToString());

            if (second == 2)
                countdown.color = Color.yellow;
            else if (second == 1)
                countdown.color = Color.green;            

            --second;

            yield return new WaitForSeconds(1f);
            yield return null;

        }

        GameManager.instance.UpdateGameState(GameState.PLAY);

        yield break;
    }

    public void UpdateHighscore(int score)
    {
        highscore.SetText(score.ToString());
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void PlayAdBeforeExit()
    {
        AdsManager.Instance.PlayAd(AdType.SKIPPABLE);
    }

}
