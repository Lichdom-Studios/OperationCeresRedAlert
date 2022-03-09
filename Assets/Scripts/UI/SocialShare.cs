using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class SocialShare : MonoBehaviour
{
    [SerializeField] string textMessage, advertMessage;
    [SerializeField] string androidURL = "https://bit.ly/3IONNrX", iOSURL = "";
    [SerializeField] GameObject highscoreScreen;
    [SerializeField] TextMeshProUGUI highscoreText;

    public void ShareWithScreenshot()
    {
        //ShareSheet shareSheet = ShareSheet.CreateInstance();
        //shareSheet.AddText(textMessage + PlayerPrefs.GetInt("Highscore").ToString() + "!/n" + advertMessage + androidURL + iOSURL);
        //if (highscoreScreen)
        //{
        //    highscoreText.SetText(PlayerPrefs.GetInt("Highscore", 0).ToString());
        //    highscoreScreen.SetActive(true);
        //}
        //shareSheet.AddScreenshot();
        //shareSheet.SetCompletionCallback((result, error) => {
        //    Debug.Log("Share Sheet was closed. Result code: " + result.ResultCode);
        //});
        //if (highscoreScreen)
        //    highscoreScreen.SetActive(false);
        //shareSheet.Show();
        textMessage = textMessage + " " + PlayerPrefs.GetInt("Highscore").ToString() + " " + advertMessage;

        StartCoroutine(TakeScreenshotAndShare());
    }

    private IEnumerator TakeScreenshotAndShare()
    {
        if (highscoreScreen)
        {
            highscoreText.SetText(PlayerPrefs.GetInt("Highscore", 0).ToString());
            highscoreScreen.SetActive(true);
        }

        yield return new WaitForEndOfFrame();

  

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        if (highscoreScreen)
            highscoreScreen.SetActive(false);

        new NativeShare().AddFile(filePath)
            .SetSubject("Operation Ceres: Red Alert").SetText(textMessage).SetUrl(androidURL).SetUrl(iOSURL)
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
    }
}
