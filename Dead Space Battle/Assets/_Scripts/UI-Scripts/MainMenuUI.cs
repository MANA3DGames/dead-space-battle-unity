using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MANA.UITweenUtil;

public class MainMenuUI : MonoBehaviour 
{
    Text _title_GO;
    Graphic _notification_GO;
    Text _innerText;

    bool _gameStartedAlready;
    bool _canClick;

    bool _isFirstTime = true;


    void OnEnable()
    {
        _canClick = false;
        ColorTween blend = gameObject.AddComponent<ColorTween>();
        blend.blendAlphaIn = true;
        blend.callBack = displayTitle;

        Invoke( "OnDoneAll", _isFirstTime ? 1.0f : 0.5f );
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void Start()
    {
        _title_GO = transform.FindChild( "Title" ).GetComponent<Text>();
        _notification_GO = transform.FindChild( "Notification" ).GetComponent<Graphic>();
        _innerText = _notification_GO.transform.GetChild( 0 ).GetComponent<Text>();

        _isFirstTime = false;
    }

    VoidDelegate displayTitle()
    {
        Invoke( "activateInput", 0.25f );
        //ColorTween blend = _title_GO.gameObject.AddComponent<ColorTween>();
        //blend.blendAlphaIn = true;

        DisplayButtonWithText( "Title" );
        DisplayButtonWithText( "Notification" );
        DisplayButtonWithText( "LogoImg" );
        DisplayButtonWithText( "InfoBtn" );
        DisplayButtonWithText( "SettingsBtn" );
        return null;
    }

    void OnDoneAll()
    {
        if ( !GameManager.Instance.IsGooglePlatformLoggedIn() )
            GameManager.Instance.LogInGooglePlatform( false );

        _canClick = true;
    }

    void DisplayButtonWithText( string btnName )
    {
        ColorTween btnBlend = transform.FindChild( btnName ).gameObject.AddComponent<ColorTween>();
        btnBlend.blendAlphaIn = true;
        
        //ColorTween innerTextBlend = btnBlend.transform.GetChild(0).gameObject.AddComponent<ColorTween>();
        //innerTextBlend.blendAlphaIn = true;
        for ( int i = 0; i < btnBlend.transform.childCount; i++ )
        {
            ColorTween innerTextBlend = btnBlend.transform.GetChild( i ).gameObject.AddComponent<ColorTween>();
            innerTextBlend.blendAlphaIn = true;
        }
    }


    void activateInput()
    {
        GameManager.Instance.ActivateInput( true );
    }



    public void OnClickPlay()
    {
        if ( _gameStartedAlready || !_canClick ) return;

        _gameStartedAlready = true;
        GameManager.Instance.PlayStartupSFX();
        Invoke( "StartGame", 0.6f );
    }

    void StartGame()
    {
        GameManager.Instance.OnPressAnyKey();
    }

    public void OnClickLeaderboardBtn()
    {
        if ( _gameStartedAlready || !_canClick ) return;

        GameManager.Instance.CheckLeaderboard();
    }

    public void OnClickAchievementsBtn()
    {
        if ( _gameStartedAlready || !_canClick ) return;

        GameManager.Instance.CheckAchievements();
    }

    public void OnClickCreditsBtn()
    {
        if ( _gameStartedAlready || !_canClick ) return;

        GameManager.Instance.MainMenuToCredits();
    }

    public void OnClickSettingsBtn()
    {
        if ( _gameStartedAlready || !_canClick ) return;

        GameManager.Instance.MainMenuToSettings();
    }


    public void OnClickLogInBtn()
    {
        if ( _gameStartedAlready || !_canClick ) return;

        GameManager.Instance.LogInGooglePlatform();
    }

    public void OnClickLogOutBtn()
    {
        if ( _gameStartedAlready || !_canClick ) return;

        GameManager.Instance.LogOutGooglePlatform();
    }

}
