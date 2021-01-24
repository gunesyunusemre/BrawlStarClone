using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class FacebookConnect : MonoBehaviour
{
    public static FacebookConnect FC;

    private string aToken;
    private string facebookId;
    private string _nickName;
    public string AToken { get { return aToken; } }

    private void OnEnable()
    {
        FacebookConnect.FC = this;
    }

    public void OnClick_FacebookLogin()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback);
        }
        else
        {
            FacebookLogin();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FacebookLogin();
        }
        else
        {
            Debug.Log("Failed to initialize the Facebook SDK");
        }
    }

    private void FacebookLogin()
    {
        if (FB.IsLoggedIn)
        {
            OnFacebookLoggedIn();
        }
        else
        {
            var perms = new List<string>() { "public_profile", "email", "user_friends" };
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            OnFacebookLoggedIn();
        }
        else
        {
            Debug.LogErrorFormat("Error in Facebook login {0}", result.Error);
        }
    }

    private void OnFacebookLoggedIn()
    {
        // AccessToken class will have session details
        aToken = AccessToken.CurrentAccessToken.TokenString;
        facebookId = AccessToken.CurrentAccessToken.UserId;
        PhotonAuth();
        FB.API("/Me?fields=first_name", HttpMethod.GET, DisplayUserName);
        PlayFabControler.PFC.LoginWithFacebook();
        //SceneManager.LoadScene("Main");
        
    }

    private void PhotonAuth()
    {
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Facebook;
        PhotonNetwork.AuthValues.UserId = facebookId; // alternatively set by server
        PhotonNetwork.AuthValues.AddAuthParameter("token", aToken);
    }

    void DisplayUserName(IResult result)
    {

        if (result.Error == null)
        {
            PhotonNetwork.NickName = "" + result.ResultDictionary["first_name"];
            _nickName = "" + result.ResultDictionary["first_name"];
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

}
