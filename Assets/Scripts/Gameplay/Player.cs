using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] int lives = 3;
    [SerializeField] float scoreTick = 1f;
    WaitForSeconds delay;

    [SerializeField] Light interiorLight;
    float minIntensity, maxIntensity;

    [SerializeField] DOTweenAnimation cameraAnimation, lightAnimation;   

    [SerializeField] AudioSource audio1, audio2, audio3;
    [SerializeField] AudioClip idleClip, engineClip, hitClip, sirenClip, gameoverClip, gameoverSong;

    int score;
    GameUI gameUI;

    Playlist music;

    private void Awake()
    {
        GameManager.OnGameStateChange += CheckGameState;
    }

    void Start()
    {
        gameUI = GameUI.instance;
        music = Playlist.instance;
        delay = new WaitForSeconds(scoreTick);

        if (!cameraAnimation)
            cameraAnimation = Camera.main.GetComponent<DOTweenAnimation>();

        minIntensity = interiorLight.intensity;
    }

    void CheckGameState(GameState state)
    {
        switch(state)
        {
            case GameState.SETUP:
                break;
            case GameState.READY:
                break;
            case GameState.COUNTDOWN:
                if (idleClip && audio1)
                {
                    audio1.clip = idleClip;
                    audio1.Play();
                }
                break;
            case GameState.PLAY:
                if(engineClip && audio1)
                {
                    audio1.clip = engineClip;
                    audio1.Play();
                }
                StartCoroutine(CalculateScore());
                break;
            case GameState.GAMEOVER:
                if (idleClip && audio1)
                {
                    audio1.clip = idleClip;
                    audio1.Play();
                }
                SaveHighscore();
                GameManager.OnGameStateChange -= CheckGameState;
                break;
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

            if (hitClip && audio2)
            {
                float randomPitch = Random.Range(-0.25f, 0f);

                audio2.pitch = 1 + randomPitch;
                audio2.clip = hitClip;
                audio2.Play();
            }

            --lives;

            if (lives == 2)
            {
                maxIntensity = 3f;
                interiorLight.intensity = maxIntensity;

                lightAnimation.DOPlay();
                if (!audio3.clip)
                    audio3.clip = sirenClip;
                audio3.Play();
            }

            if (lives <= 0)
            {
                if (hitClip && audio2)
                {
                    audio2.clip = gameoverClip;
                    audio2.Play();                    
                }

                maxIntensity = 5f;
                interiorLight.intensity = maxIntensity;

                music.PlaySong(gameoverSong);

                GameManager.instance.UpdateGameState(GameState.GAMEOVER);
            }
        }
        else
            Destroy(other.gameObject);
    }
}
