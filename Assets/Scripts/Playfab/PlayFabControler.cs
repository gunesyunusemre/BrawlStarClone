using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;
using Photon.Pun;

public class PlayFabControler : MonoBehaviour
{
    public static PlayFabControler PFC;

    string aToken;
    private string userName, userEmail, userPassword;
    [SerializeField]
    GameObject signUpPanel, LoginPanel;

    
    //string encrytpedPassword;

    private void OnEnable()
    {
        if (PlayFabControler.PFC==null)
        {
            PlayFabControler.PFC = this;
        }
        else
        {
            if (PlayFabControler.PFC != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }


    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "B56E5"; //// Please change this value to your own titleId from PlayFab Game Manager
        }
    }


    public void SignUpTab()
    {
        signUpPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    public void LogInTab()
    {
        signUpPanel.SetActive(true);
        LoginPanel.SetActive(false);
    }

    string Encrypt(string pw)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] epw = System.Text.Encoding.UTF8.GetBytes(pw);
        epw = x.ComputeHash(epw);
        System.Text.StringBuilder s = new System.Text.StringBuilder();
        foreach (byte b in epw)
        {
            s.Append(b.ToString("x2").ToLower());
        }
        return s.ToString();
    }



    #region ------------------- Register ------------------------------

    public void OnClick_Register()
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = userName };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }
 
    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("User Registered");
    }

    void OnRegisterFailure(PlayFabError error)
    {
        Debug.Log("Registration failed" + error);
    }

    #endregion ----------------------------------------------------------


    #region ------------------- Login ------------------------------

    public void OnClick_Login()
    {        
        var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };//Encrypt(userPasswordLogin.text) };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, LoginFailure);
    }

    public void LoginSuccess(LoginResult result)
    {
        Debug.Log("Sign in!");
        GetAccountInfo();
        //PlayerScripts.PlayerS.SetLoginType("Playfab");
    }

    public void LoginFailure(PlayFabError error)
    {
        //Debug.LogError(error.GenerateErrorReport());
    }

    public void LoginWithFacebook()
    {

        aToken = FacebookConnect.FC.AToken; //Facebook.Unity.AccessToken.CurrentAccessToken;

        LoginWithFacebookRequest request = new LoginWithFacebookRequest();
        request.AccessToken = aToken;//.TokenString;
        request.TitleId = PlayFabSettings.TitleId;
        request.CreateAccount = true;

        PlayFabClientAPI.LoginWithFacebook(request, OnLoginCallback, OnApiCallError);
       ///
        #region Google
        /*  Login Google
         *  LoginWithGoogleAccountRequest request = new LoginWithGoogleAccountRequest();
            request.ServerAuthCode = googleSignInUser.AuthCode; // GoogleSignInUser is the result of a successful google login
            request.TitleId = PlayFabSettings.TitleId;
            request.CreateAccount = true;
 
            PlayFabClientAPI.LoginWithGoogleAccount(request, OnLoginCallback, OnApiCallError); */
        #endregion Google
        ///
    }

    void OnLoginCallback(LoginResult result)
    {
        Debug.Log("Sign in! " +result.PlayFabId);
        SceneManager.LoadScene("Main");
    }

    void OnApiCallError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    #endregion Login

    #region Set_InputField

    public void GetUserEmail(string emailIn)
    {
        userEmail = emailIn;
    }

    public void GetUserPassword(string passwordIn)
    {
        userPassword = passwordIn;
    }

    public void GetUserName(string userNameIn)
    {
        userName = userNameIn;
    }

    #endregion Set_InputField

    #region Login_as_Quest
    public void OnClick_SingUpAsQuest()
    {
        SceneManager.LoadScene("Main");
    }
    #endregion Login_as_Quest

    #region GetUserName

    void GetAccountInfo()
    {
        GetAccountInfoRequest request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, GetAccountInfoSuccesss, GetAccountInfoFail);
    }

    private void GetAccountInfoFail(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    private void GetAccountInfoSuccesss(GetAccountInfoResult result)
    {
       PhotonNetwork.NickName= result.AccountInfo.Username;
        SceneManager.LoadScene("Main");
    }

    #endregion GetUserName

}
