using UnityEngine;
using UnityEngine.UI;
using MANA3D.UI;
using MANA3D.UI.Tween;


public class UIManager : MonoBehaviour 
{
    GameObject _mainMenu_GO;
    GameObject _loading_GO;
    GameObject _huds_GO;
    GameObject _pauseMenu_GO;
    GameObject _gameOver_GO;
    //GameObject _helpMenu_GO;
    GameObject _creditsMenu_GO;
    GameObject _settingsMenu_GO;

    HUDs_UI _hudScript;
    

    UITweenManager _tweenManager;
    UIMenu _rootMenu;
    //UIMenu _titleMenu;
    UIMenu _mainMenu;

    float _resizePrecentage;
    float _offsetPrecentage;


    void Awake()
    {
        Transform _uiRoot = GameObject.Find( "UI_Root" ).transform;
        _mainMenu_GO = _uiRoot.FindChild( "MainMenu" ).gameObject;
        _loading_GO = _uiRoot.FindChild( "Loading" ).gameObject;
        _pauseMenu_GO = _uiRoot.FindChild("PauseMenu").gameObject;
        _huds_GO = _uiRoot.FindChild("HUD").gameObject;
        _hudScript = _huds_GO.GetComponent<HUDs_UI>();
        _gameOver_GO = _uiRoot.FindChild("GameOverMenu").gameObject;
        //_helpMenu_GO = _uiRoot.FindChild("HelpMenu").gameObject;
        _creditsMenu_GO = _uiRoot.FindChild("CreditsMenu").gameObject;
        _settingsMenu_GO = _uiRoot.FindChild("SettingsMenu").gameObject;

        _tweenManager = new UITweenManager();
        _rootMenu = new UIMenu( _uiRoot.gameObject );
        //_titleMenu = new UIMenu( _uiRoot.FindChild("TitleMenu").gameObject );

        _mainMenu = new UIMenu( _uiRoot.FindChild( "MainMenu" ).gameObject );
        _mainMenu.AddListenerTo( "SinglePlayerBtn", ()=> { GameManager.Instance.OnClickSinglePlayerBtn(); } );
        //_multiplayerMenu = new UIMenu( _uiRoot.FindChild("MultiplayerMenu").gameObject );
    }

    void Update()
    {
        _tweenManager.Update();
    }

    //public void ShowTitleMenu( bool show )
    //{
    //    _titleMenu.ShowRoot( show );

    //    if ( show )
    //        _tweenManager.AddUITweenAlpha( _rootMenu, "TitleMenu", 
    //                                       0.0f, 1.0f, 1.0f, 0.0f,
    //                                       ()=> { ShowTitleUI(); }, true );
    //}
    void ShowTitleUI()
    {
        _tweenManager.AddUITweenAlphaToChildren( _rootMenu, "TitleMenu", 
                                                 0.0f, 1.0f, 1.0f, 0.0f,
                                                 ()=> { GameManager.Instance.ActivateInput( true ); }, true );
    }


    public void ConfigSmallResolution( float resize, float offset )
    {
        _resizePrecentage = resize;
        _offsetPrecentage = offset;

        ConfigItem( _mainMenu_GO, "Title" );
        ConfigItem( _mainMenu_GO, "InfoBtn" );
        ConfigItem( _mainMenu_GO, "LogoImg" );
        ConfigItem( _mainMenu_GO, "SettingsBtn" );
        ConfigItem( _mainMenu_GO, "Notification" );
        ConfigItem( _mainMenu_GO, "LeaderboardBtn" );
        ConfigItem( _mainMenu_GO, "AchievementBtn" );
        ConfigItem( _mainMenu_GO, "LogInBtn" );
        ConfigItem( _mainMenu_GO, "LogOutBtn" );

        ConfigItem( _creditsMenu_GO, "MANA" );
        ConfigItem( _creditsMenu_GO, "Publish" );
        ConfigItem( _creditsMenu_GO, "Development" );
        ConfigItem( _creditsMenu_GO, "BGMusic" );
        ConfigItem( _creditsMenu_GO, "link" );
        ConfigItem( _creditsMenu_GO, "Back" );

        ConfigItem( _settingsMenu_GO, "Window" );
        //ConfigItem( _settingsMenu_GO, "GameSettingsTitle" );
        ConfigItem( _settingsMenu_GO, "MoveTitle" );
        ConfigItem( _settingsMenu_GO, "move_NormalBtn" );
        ConfigItem( _settingsMenu_GO, "move_LargeBtn" );
        ConfigItem( _settingsMenu_GO, "FireTitle" );
        ConfigItem( _settingsMenu_GO, "Fire_LargeBtn" );
        ConfigItem( _settingsMenu_GO, "Fire_NormalBtn" );
        //ConfigItem( _settingsMenu_GO, "Save" );
        //ConfigItem( _settingsMenu_GO, "Cancel" );

        ConfigItem( _loading_GO, "Title" );

        //var pauseWind = _pauseMenu_GO.transform.FindChild( "Window" ).gameObject;
        //ConfigItem( pauseWind, "Title" );
        //ConfigItem( pauseWind, "ResumeGameBtn" );
        //ConfigItem( pauseWind, "GameSettingsBtn" );
        //ConfigItem( pauseWind, "LogInBtn" );
        //ConfigItem( pauseWind, "LogOutBtn" );
        //ConfigItem( pauseWind, "RestartGameBtn" );
        //ConfigItem( pauseWind, "StoreBtn" );
        //ConfigItem( pauseWind, "QuitGameBtn" );
        ConfigItem( _pauseMenu_GO, "Window" );

        ConfigItem( _huds_GO, "ScoreValBG" );
        ConfigItem( _huds_GO, "ScoreIcon" );
        ConfigItem( _huds_GO, "ScoreVal" );
        ConfigItem( _huds_GO, "LifeValBG" );
        ConfigItem( _huds_GO, "LifeIcon" );
        ConfigItem( _huds_GO, "LifeVal" );
        ConfigItem( _huds_GO, "PauseBtn" );

        ConfigItem( _gameOver_GO, "Title" );
        ConfigItem( _gameOver_GO, "Notification" );
        ConfigItem( _gameOver_GO, "ScoreTitle" );
        ConfigItem( _gameOver_GO, "ScoreValBG" );
        ConfigItem( _gameOver_GO, "ScoreIcon" );
        ConfigItem( _gameOver_GO, "scoreVal" );
        ConfigItem( _gameOver_GO, "PlayAgainBtn" );
        ConfigItem( _gameOver_GO, "TopScoresBtn" );
        ConfigItem( _gameOver_GO, "AchievementsBtn" );
        ConfigItem( _gameOver_GO, "LogInBtn" );
    }

    void ConfigItem( GameObject root, string uiName )
    {
        root.transform.FindChild( uiName ).GetComponent<RectTransform>().localScale *= _resizePrecentage;
        root.transform.FindChild( uiName ).GetComponent<RectTransform>().anchoredPosition -= ( root.transform.FindChild( uiName ).GetComponent<RectTransform>().anchoredPosition * _offsetPrecentage );
    }


    public void ShowMainMenu( bool show )
    {
        //_mainMenu_GO.SetActive( show );
        _mainMenu.ShowRoot( show );


        if ( show )
            _tweenManager.AddUITweenAlpha( _rootMenu, "MainMenu", 
                                           0.0f, 1.0f, 1.0f, 0.0f,
                                           ()=> { ShowTitleUI(); }, true );
    }

    public void ShowLoadingScreen( bool show )
    {
        _loading_GO.SetActive( show );
    }

    public void ShowHUDs( bool show )
    {
        _huds_GO.SetActive( show );
    }
    public void UpdateHUDLifes( int life )
    {
        _hudScript.updateLife( life );
    }
    public void UpdateScore( int score )
    {
        _hudScript.updateScore( score );
    }


    public void ShowPauseMenu( bool show )
    {
        _pauseMenu_GO.SetActive( show );
    }

    public void ShowGameOver( bool show )
    {
        _gameOver_GO.SetActive( show );
    }

    public void DisplayScores( int score )
    {
        _gameOver_GO.transform.FindChild( "scoreVal" ).GetComponent<Text>().text = score.ToString();
    }

    public void ShowHelpMenu( bool show )
    {
        //_helpMenu_GO.SetActive( show );
    }


    public void ShowCredits( bool show )
    {
        _creditsMenu_GO.SetActive( show );
    }

    public void ShowSettings( bool show )
    {
        _settingsMenu_GO.SetActive( show );
    }


    public void ShowLogIn( bool show )
    {
        _mainMenu_GO.transform.FindChild( "LogInBtn" ).gameObject.SetActive( show );

        _mainMenu_GO.transform.FindChild( "LeaderboardBtn" ).gameObject.SetActive( !show );
        _mainMenu_GO.transform.FindChild( "AchievementBtn" ).gameObject.SetActive( !show );
        _mainMenu_GO.transform.FindChild( "LogOutBtn" ).gameObject.SetActive( !show );

        _pauseMenu_GO.transform.FindChild( "Window" ).FindChild( "LogInBtn" ).gameObject.SetActive( show );
        _pauseMenu_GO.transform.FindChild( "Window" ).FindChild( "LogOutBtn" ).gameObject.SetActive( !show );


        _gameOver_GO.transform.FindChild( "LogInBtn" ).gameObject.SetActive( show );
        _gameOver_GO.transform.FindChild( "TopScoresBtn" ).gameObject.SetActive( !show );
        _gameOver_GO.transform.FindChild( "AchievementsBtn" ).gameObject.SetActive( !show );
    }
}
