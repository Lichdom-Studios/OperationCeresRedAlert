using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public static Player instance;

    [SerializeField] int lives = 3;
    [SerializeField] float scoreTick = 1f;
    WaitForSeconds delay;

    [SerializeField] Light interiorLight;
    float minIntensity, maxIntensity;
    [SerializeField] ParticleSystem sparksEffect, electricityEffect, smokeEffect;

    [SerializeField] DOTweenAnimation cameraAnimation, lightAnimation;   

    [SerializeField] AudioSource audio1, audio2, audio3;
    [SerializeField] AudioClip idleClip, engineClip, hitClip, sirenClip, gameoverClip, gameoverSong;

    int score;
    GameUI gameUI;

    Playlist music;

    float gameDuration;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

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
                if (engineClip && audio1)
                {
                    audio1.clip = engineClip;
                    audio1.Play();
                }
                StartCoroutine(CalculateScore());
                break;
            case GameState.GAMEOVER:
                gameDuration = Time.timeSinceLevelLoad;
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

            AddToScore(1);

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
                maxIntensity = 10f;
                interiorLight.intensity = maxIntensity;

                lightAnimation.DOPlay();
                if (!audio3.clip)
                    audio3.clip = sirenClip;
                audio3.Play();
                sparksEffect.Play();
            }

            if(lives == 1)
            {              
                maxIntensity = 20f;
                interiorLight.intensity = maxIntensity;

                electricityEffect.Play();
                smokeEffect.Play();                
            }

            if (lives <= 0)
            {
                audio3.DOFade(0f, 2f);

                if (hitClip && audio2)
                {
                    audio2.clip = gameoverClip;
                    audio2.Play();                    
                }

                maxIntensity = 40f;
                interiorLight.intensity = maxIntensity;                

                music.PlaySong(gameoverSong, 0.5f);

                GameManager.instance.UpdateGameState(GameState.GAMEOVER);
            }
        }
        else
            Destroy(other.gameObject);
    }

    public void AddToScore(int value)
    {
        score += value;
        gameUI.UpdateHighscore(score);
    }
    public int GetScore()
    {
        return score;
    }

    public float GetDuration()
    {
        return gameDuration - 3;
    }

}
