using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public enum Authtypes
{
    None,
    EmailAndPassword,
    RegisterPlayFabAccount,
    Facebook,
    Google
}


public class AuthServices
{

    public delegate void LoginSuccessEvent(LoginResult success);
    public static event LoginSuccessEvent OnLoginSuccess;

    public delegate void PlayFabErrorEvent(PlayFabError error);
    public static event PlayFabErrorEvent OnPlayFabError;


    public string Email;
    public string Username;
    public string Password;
    public string AuthTicket;
    public GetPlayerCombinedInfoRequestParams InfoRequestParams;

    public static string PlayFabId { get { return _playFabId; } }
    private static string _playFabId;
    public static string SessionTicket { get { return _sessionTicket; } }
    private static string _sessionTicket;


    private const string _PlayFabAuthTypeKey = "PlayFabAuthType";

    public static AuthServices Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AuthServices();
            }
            return _instance;
        }
    }
    private static AuthServices _instance;



    public Authtypes AuthType
    {
        get
        {
            return (Authtypes)PlayerPrefs.GetInt(_PlayFabAuthTypeKey, 0);
        }
        set
        {

            PlayerPrefs.SetInt(_PlayFabAuthTypeKey, (int)value);
        }
    }

    public void Authenticate(Authtypes authType)
    {
        AuthType = authType;
        Authenticate();
    }

    public void Authenticate()
    {
        var authType = AuthType;
        switch (authType)
        {
            case Authtypes.None:
                //Empty
                break;
            case Authtypes.EmailAndPassword:
                AuthenticateEmailAndPassword();
                break;
            case Authtypes.RegisterPlayFabAccount:
                AuthenticateRegisterPlayFabAccount();
                break;
            case Authtypes.Facebook:
                AuthenticateLoginWithFacebook();
                break;
            case Authtypes.Google:
                AuthenticateGooglePlayGames();
                break;
        }
    }

    private void AuthenticateEmailAndPassword()
    {
        var request = new LoginWithEmailAddressRequest {
            TitleId = PlayFabSettings.TitleId,
            Email = Email, 
            Password = Password,
            InfoRequestParameters = InfoRequestParams
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, StatusFail);
    }

    private void AuthenticateRegisterPlayFabAccount()
    {
        var registerRequest = new RegisterPlayFabUserRequest
        {
            Email = Email,
            Password = Password,
            Username = Username
        };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, StatusFail);
    }

    private void AuthenticateLoginWithFacebook()
    {
#if UNITY_FACEBOOK
        LoginWithFacebookRequest request = new LoginWithFacebookRequest();
        request.AccessToken = AuthTicket;
        request.TitleId = PlayFabSettings.TitleId;
        request.CreateAccount = true;

        PlayFabClientAPI.LoginWithFacebook(request, LoginSuccess, StatusFail);
#endif
    }

    private void AuthenticateGooglePlayGames()
    {
#if GOOGLEGAMES
        PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            ServerAuthCode = AuthTicket,
            InfoRequestParameters = InfoRequestParams,
            CreateAccount = true
        }, (result) =>
        {
            //Store Identity and session
            _playFabId = result.PlayFabId;
            _sessionTicket = result.SessionTicket;

            //check if we want to get this callback directly or send to event subscribers.
            if (OnLoginSuccess != null)
            {
                //report login result back to the subscriber
                OnLoginSuccess.Invoke(result);
            }
        }, (error) =>
        {

            //report errro back to the subscriber
            if (OnPlayFabError != null)
            {
                OnPlayFabError.Invoke(error);
            }
        });
#endif
    }


    private void LoginSuccess(LoginResult result)
    {
        _playFabId = result.PlayFabId;
        _sessionTicket = result.SessionTicket;
        if (OnLoginSuccess != null)
        {
            OnLoginSuccess.Invoke(result);
        }
    }

    private void StatusFail(PlayFabError error)
    {
        if (OnPlayFabError != null)
        {
            OnPlayFabError.Invoke(error);
        }
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
       Authenticate(Authtypes.EmailAndPassword);
    }


   


}
