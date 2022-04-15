using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager Instance;

    string uName, pWord;

    void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    public void LoginWithGoogle()
    {

    }

    public void LoginWithApple()
    {

    }

    public void LoginWithSteam()
    {

    }

    public void LoginWithUsername(string username, string password)
    {
        var request = new LoginWithPlayFabRequest { Username = username, Password = password, InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetPlayerProfile = true } };
        PlayFabClientAPI.LoginWithPlayFab(request, LoginSuccessful, LoginFailed);
    }

    public void RegisterWithUsername(string username, string password)
    {
        uName = username;
        pWord = password;

        var request = new RegisterPlayFabUserRequest { Username = username, DisplayName = username, Password = password, RequireBothUsernameAndEmail = false };
        PlayFabClientAPI.RegisterPlayFabUser(request, RegisteredSuccessful, LoginFailed);
    }

    void RegisteredSuccessful(RegisterPlayFabUserResult result)
    {
        LoginWithUsername(uName, pWord);
    }

    void LoginSuccessful(LoginResult result)
    {
        if(result.InfoResultPayload.PlayerProfile.DisplayName == string.Empty)
        {
            MainMenu.instance.OpenUsernameWindow();
        }
        else
        {
            MainMenu.instance.CloseLoginWindow();
        }

    }

    void LoginFailed(PlayFabError error)
    {
        MainMenu.instance.DisplayErrorMessage(error.ErrorMessage);
    }

}
