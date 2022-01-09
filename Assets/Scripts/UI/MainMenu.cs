using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    [SerializeField] List<VolumeControl> volumeControls;
    [SerializeField] TextMeshProUGUI highscoreText;

    [SerializeField] GameObject loadingScreen;
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] Slider loadingBar;

    WaitForSeconds loadingTextAnimationTick;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        loadingTextAnimationTick = new WaitForSeconds(.5f);

        UpdateHighscore();
    }

    public void StartGame()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadScene());
        StartCoroutine(AnimateLoadingText());
    }

    public void SaveSettings()
    {
        if(volumeControls.Count > 0)
        {
            foreach(VolumeControl vc in volumeControls)
            {
                vc.SaveVolume();
            }
        }
    }

    void UpdateHighscore()
    {
        highscoreText.SetText(PlayerPrefs.GetInt("Highscore", 0).ToString());
    }

    IEnumerator LoadScene()
    {        
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(1);

        while(!loadingOperation.isDone)
        {
            loadingBar.value = loadingOperation.progress;
            yield return null;
        }

        yield break;
    }

    IEnumerator AnimateLoadingText()
    {
        int iteration = 0;

        while(isActiveAndEnabled)
        {
            switch(iteration)
            {
                case 0:
                    loadingText.SetText("Loading");
                    ++iteration;
                    break;
                case 1:
                    loadingText.SetText("Loading.");
                    ++iteration;
                    break;
                case 2:
                    loadingText.SetText("Loading..");
                    ++iteration;
                    break;
                case 3:
                    loadingText.SetText("Loading...");
                    iteration = 0;
                    break;
            }

            yield return loadingTextAnimationTick;
            yield return null;
        }

        yield break;
    }
}
