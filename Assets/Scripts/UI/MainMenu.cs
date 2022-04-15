using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    [SerializeField] List<VolumeControl> volumeControls;
    [SerializeField] QualityControl qualityControl;
    [SerializeField] TextMeshProUGUI highscoreText;

    [SerializeField] GameObject loadingScreen, loginWindow, usernameWindow;
    [SerializeField] TextMeshProUGUI loadingText, errorText;
    [SerializeField] Slider loadingBar;
    [SerializeField] Button loginButton, registerButton;
    [SerializeField] TMP_InputField usernameInput, passwordInput;

    WaitForSeconds loadingTextAnimationTick;

    string username, password;

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

        if (qualityControl)
            qualityControl.SaveQuality();
    }

    void UpdateHighscore()
    {
        //highscoreText.SetText(PlayerPrefs.GetInt("Highscore", 0).ToString());
        highscoreText.SetText(DataManager.GetHighScore().ToString());
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

    public void OpenTwitter()
    {
        Application.OpenURL("twitter://user?screen_name=LichdomStudios");
    }

    public void OpenFacebook()
    {
        Application.OpenURL("fb://page/100214428470829");
    }

    public void OpenDiscord()
    {
        Application.OpenURL("https://discord.gg/BdzWSseuFs");
    }

    public void OpenLoginWindow()
    {
        loginWindow.SetActive(true);
    }
    public void CloseLoginWindow()
    {
        loginWindow.SetActive(false);
    }

    public void OpenUsernameWindow()
    {
        usernameWindow.SetActive(true);
    }

    public void CloseUsernameWindow()
    {
        usernameWindow.SetActive(false);
        CloseLoginWindow();
    }

    public void RegisterWithUsername()
    {
        PlayfabManager.Instance.RegisterWithUsername(usernameInput.text, passwordInput.text);
    }

    public void LoginWithUsername()
    {
        PlayfabManager.Instance.LoginWithUsername(usernameInput.text, passwordInput.text);
    }

    public void DisplayErrorMessage(string message)
    {
        errorText.SetText(message);
        errorText.GetComponent<DOTweenAnimation>().DOPlayForward();
    }

    public void VerifyInputs()
    {
        loginButton.interactable = (usernameInput.text.Length >= 4 && passwordInput.text.Length >= 6);
        registerButton.interactable = (usernameInput.text.Length >= 4 && passwordInput.text.Length >= 6);
    }
}
