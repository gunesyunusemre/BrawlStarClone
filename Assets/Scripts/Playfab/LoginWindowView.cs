using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;

#if UNITY_FACEBOOK
using Facebook.Unity;
#endif

#if GOOGLEGAMES
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class LoginWindowView : MonoBehaviour
{

    [Header("Login Buttons")]
    public Button LoginButton;
    public Button FacebookLoginButton;
    public Button GoogleLoginButton;
    public Button RegisterOKButton;
    public Button RegisterPanelButton;
    public Button LoginPanelButton;

    [Header("Register:")]
    [Header("InputText")]
    public InputField Email;
    public InputField Username;
    public InputField Password;
    [Header("Login:")]
    public InputField EmailLogin;
    public InputField PasswordLogin;

    [Header("Panels")]
    public GameObject RegisterPanel;
    public GameObject LoginPanel;

    [Header("Text")]
    public Text LoginText;

    private AuthServices _AuthService = AuthServices.Instance;

    public GetPlayerCombinedInfoRequestParams InfoRequestParams;


    private void Awake()
    {
#if UNITY_FACEBOOK
        FB.Init(OnFBInitComplete, OnFBHideUnity);
#endif

#if GOOGLEGAMES
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .AddOauthScope("profile")
            .RequestServerAuthCode(false)
            .Build();
        PlayGamesPlatform.InitializeInstance(config);

        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesPlatform.Activate();
#endif
    }


    private void Start()
    {

        LoginButton.onClick.AddListener(OnLoginClicked);
        FacebookLoginButton.onClick.AddListener(OnFacebookLoginButtonClicked);
        GoogleLoginButton.onClick.AddListener(OnGoogleLoginButtonClicked);
        RegisterOKButton.onClick.AddListener(OnRegisterOKButtonClicked);
        RegisterPanelButton.onClick.AddListener(LogInTab); 
        LoginPanelButton.onClick.AddListener(SignUpTab);

        LoginText.text = "";

        AuthServices.OnLoginSuccess += OnLoginSuccess;
        AuthServices.OnPlayFabError += OnPlayFabError;

        _AuthService.InfoRequestParams = InfoRequestParams;

    }


    private void OnLoginClicked()
    {
        _AuthService.Email = EmailLogin.text;
        _AuthService.Password = PasswordLogin.text;
        _AuthService.Authenticate(Authtypes.EmailAndPassword);
    }

    private void OnFacebookLoginButtonClicked()
    {
#if UNITY_FACEBOOK
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, OnHandleFBResult);
#endif
    }

    private void OnGoogleLoginButtonClicked()
    {
        Social.localUser.Authenticate((success) =>
        {
            if (success)
            {
#if GOOGLEGAMES
                Debug.Log("Deneme");
                var serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                _AuthService.AuthTicket = serverAuthCode;
                _AuthService.Authenticate(Authtypes.Google);
#endif
            }
        });
    }

#if UNITY_FACEBOOK
    private void OnHandleFBResult(ILoginResult result)
    {
        if (result.Cancelled)
        {
            Debug.LogError("Facebook Login Cancelled.");
        }
        else if (result.Error != null)
        {
            Debug.LogError(result.Error);
        }
        else
        {
            _AuthService.AuthTicket = result.AccessToken.TokenString;
            _AuthService.Authenticate(Authtypes.Facebook);
        }
    }

    private void OnFBInitComplete()
    {
        if (AccessToken.CurrentAccessToken != null)
        {
            _AuthService.AuthTicket = AccessToken.CurrentAccessToken.TokenString;
            _AuthService.Authenticate(Authtypes.Facebook);
           
        }
    }

    void DisplayUserName(IResult result)
    {

        if (result.Error == null)
        {
            PhotonNetwork.NickName = "" + result.ResultDictionary["first_name"];
            Debug.Log(result.ResultDictionary["first_name"]);
            SceneManager.LoadScene("Main");
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

        private void OnFBHideUnity(bool isUnityShown)
    {
        //do nothing.
    }
#endif

    private void OnRegisterOKButtonClicked()
    {
        _AuthService.Email = Email.text;
        _AuthService.Username = Username.text;
        _AuthService.Password = Password.text;
        _AuthService.Authenticate(Authtypes.RegisterPlayFabAccount);
    }




    private void OnLoginSuccess(PlayFab.ClientModels.LoginResult result)
    {
        Debug.LogFormat("Logged In as: " + result.PlayFabId);

#if UNITY_FACEBOOK
        FB.API("/Me?fields=first_name", HttpMethod.GET, DisplayUserName);
#endif
#if !UNITY_FACEBOOK
        GetAccountInfo();
#endif
    }

    private void OnPlayFabError(PlayFabError error)
    {
        switch (error.Error)
        {
            case PlayFabErrorCode.InvalidEmailAddress:
                LoginText.text = "Email or password invalid.";
                break;
            case PlayFabErrorCode.InvalidPassword:
                LoginText.text = "Email or password invalid.";
                break;
            case PlayFabErrorCode.InvalidEmailOrPassword:
                LoginText.text = "Email or password invalid.";
                break;
            case PlayFabErrorCode.AccountNotFound:
                RegisterPanel.SetActive(true);
                LoginPanel.SetActive(false);
                break;
            case PlayFabErrorCode.InternalServerError:
                LoginText.text = "Check your internet.";
                break;
            default:
                break;

        }

        Debug.Log(error.Error);
        Debug.LogError(error.GenerateErrorReport());
    }

    private void GetAccountInfo()
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
        PhotonNetwork.NickName = result.AccountInfo.Username;
        SceneManager.LoadScene("Main");
    }

    private void SignUpTab()
    {
        RegisterPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    private void LogInTab()
    {
        RegisterPanel.SetActive(true);
        LoginPanel.SetActive(false);
    }

}
