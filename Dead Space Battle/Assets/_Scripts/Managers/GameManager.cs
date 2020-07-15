using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum GameMode { Single, Multiplayer }

public enum GameState { Title, Loading, MultiplayerMenu, UncontrolledGamePlay, Gameplay, Paused, GameOver, Score, Option }


public class GameManager : MonoBehaviour 
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }


    private GameState _state;
    public GameState State { get { return _state; } }

    private GameMode _mode;
    public GameMode Mode { get { return _mode; } }


    public delegate void onGamePausedDelegate( bool pause );
    public event onGamePausedDelegate onGamePaused;

    bool _showHelp = false;


    AudioManager _audioManager;
    ImageEffectManager _imageEffectManager;
    InputManager _inputManager;
    UIManager _uiManager;
    LevelManager _levelManager;
    PlayGamesPlatformManager _leaderboardManager;

    MANA3D.Utilities.Coroutine.CoroutineTask _adMobBannerTask;



    void Awake()
    {
        if ( !_instance ) 
            _instance = this;

        bool isSmall = false;
        ConfigResoultion( ref isSmall );

        // Check build version.
        StartCoroutine( CheckBuildVersion() );

        _uiManager = gameObject.AddComponent<UIManager>();
        _inputManager = gameObject.AddComponent<InputManager>();
        _levelManager = gameObject.AddComponent<LevelManager>();
        _imageEffectManager = gameObject.AddComponent<ImageEffectManager>();
        _audioManager = gameObject.AddComponent<AudioManager>();
        _leaderboardManager = gameObject.AddComponent<PlayGamesPlatformManager>();
    }

    void Start()
    {
        _uiManager.ShowMainMenu( true );
        _audioManager.SetBGM( 0 );
        _audioManager.PlayBGM();

        _objectPools = new Dictionary<int, List<GameObject>>();
        _objectPools.Add( 0, new List<GameObject>() );
        _objectPools.Add( 1, new List<GameObject>() );
        _objectPools.Add( 2, new List<GameObject>() );
        _objectPools.Add( 3, new List<GameObject>() );
    }


    void ConfigResoultion( ref bool isSmall )
    {
        //Resolution res = Screen.resolutions[Screen.resolutions.Length - 1];

        int resolutionSum = Screen.width + Screen.height;
        if ( resolutionSum >= 2000 )
            Screen.SetResolution( 1280, 720, true );
        else if ( resolutionSum >= 1500 )
        {
            Screen.SetResolution( 960, 540, true );
            StartCoroutine( ConfigSmallResolution( 0.75f, 0.25f ) );
        }
        else if ( resolutionSum >= 800 )
        {
            Screen.SetResolution( 480, 320, true );
            StartCoroutine( ConfigSmallResolution( 0.5f, 0.5f ) );
            isSmall = true;
        }
        else
            Application.Quit();
    }

    IEnumerator ConfigSmallResolution( float resize, float offset )
    {
        yield return new WaitForEndOfFrame();
        _uiManager.ConfigSmallResolution( resize, offset );
    }


    public JoystickSettings joystickSettings { get { return _inputManager.joystickSettings; } }

    public void SetFireJoysticksSettings( int id )
    {
        _inputManager.SetFireJoysticksSettings( id );
    }
    public void SetMoveJoysticksSettings( int id )
    {
        _inputManager.SetMoveJoysticksSettings( id );
    }



    void LoadLevel()
    {
        _state = GameState.Loading;
        _uiManager.ShowLoadingScreen( true );

        StartCoroutine( _levelManager.LoadLevel( ( succeed ) => 
            {
                if ( succeed )
                    StartCoroutine( OnLevelLoaded() );
            })
        );
    }

    public void OnClickPlayAgain()
    {
        _leaderboardManager.LogEvent( PlayGamesPlatformManager.EVENT_CLICKED_PLAYAGAIN_BUTTON );
        _audioManager.PlayClickBtn();
        RestartGame();
    }
    public void RestartGame()
    {
        Player.Instance.reset();
        Player.Instance.CancelAllTasks();

        _levelManager.RestartLevel();

        _uiManager.ShowGameOver( false );
        _uiManager.ShowPauseMenu( false );

        StartCoroutine( OnLevelLoaded() );
    }

    IEnumerator OnLevelLoaded()
    {
        _uiManager.ShowLoadingScreen( false );

        // Check if this is a single player game.
        if ( _mode == GameMode.Single )
        {
            if ( _showHelp )
            {
                _uiManager.ShowHelpMenu( true );
                Player.Instance.setActive( false );
                yield return new WaitForSeconds( 5.0f );
                Player.Instance.setActive( true );
                _uiManager.ShowHelpMenu( false );
                _showHelp = false;
            }

            _audioManager.SetBGM( 1 );
            _uiManager.ShowLoadingScreen( false );
            _state = GameState.UncontrolledGamePlay;

            yield return new WaitForSeconds( 0.5f );

            _state = GameState.Gameplay;
            ShowCursor( false );
            _uiManager.ShowHUDs( true );
            //_uiManager.updateScore( 0 );
            //_uiManager.updateLifes( 3 );
            _levelManager.UpdatePlayerLifes( 3 );
            _levelManager.UpdatePlayerScore( 0 );
            _levelManager.StartLevel( 0 );

            _inputManager.EnablePlayerController( true );
        }
        // It is a multiplayer game.
        else
        {

        }
    }



    public void PauseGame()
    {
        _audioManager.PlayClickBtn();

        if ( onGamePaused != null )
            onGamePaused( true );

        _state = GameState.Paused;

        ShowCursor( true );

        _inputManager.EnablePlayerController( false );
        _uiManager.ShowHUDs( false );
        _uiManager.ShowPauseMenu( true );
        _audioManager.PauseBGM();
    }

    public void ResumeGame()
    {
        _audioManager.PlayClickBtn();

        if ( onGamePaused != null )
            onGamePaused( false );

        _state = GameState.Gameplay;

        ShowCursor( false );

        _uiManager.ShowPauseMenu( false );
        _uiManager.ShowHUDs( true );
        _inputManager.EnablePlayerController( true );
        _audioManager.UnpauseBGM();
    }


    public void EndGame()
    {
        StartCoroutine( GameOver() );
    }

    IEnumerator GameOver()
    {
        _inputManager.EnablePlayerController( false );
        _state = GameState.UncontrolledGamePlay;
        _uiManager.ShowHUDs( false );

        yield return new WaitForSeconds( 3.0f );

        ShowCursor( true );
        _state = GameState.GameOver;

        _uiManager.ShowGameOver( true );
        _uiManager.DisplayScores( _levelManager.PlayerScore );
        _leaderboardManager.SubmitScoreToLeaderBorad( _levelManager.PlayerScore );

        _levelManager.DeactivateAll();
        _inputManager.ResetPlayerControls();
    }

    void ShowCursor( bool show )
    {
#if !UNITY_EDITOR
        Cursor.visible = show;
#else
        Cursor.visible = true;
#endif
    }

    public void GoToMainMenu()
    {
        _uiManager.ShowPauseMenu( false );
        _uiManager.ShowMainMenu( true );
        _levelManager.DeactivateAll();
        _inputManager.ResetPlayerControls();
        _state = GameState.Title;
    }


#region [Player]
    public void OnPlayerDestroyed()
    {
        _state = GameState.UncontrolledGamePlay;
        ShakeCamera( 1.5f, 1.0f );
        UpdatePlayerLifes( -1 );
    }

    public void OnPlayerRespawn()
    {
        _state = GameState.Gameplay;
    }
#endregion

#region [ImageEffect Manager]
    public void ShakeCamera( float shakeAmount, float duration )
    {
        _imageEffectManager.ShakeCamera( shakeAmount, duration );
    }
#endregion

#region [Input Manager]
    public void ActivateInput( bool active )
    {
        _inputManager.Activate( active );
    }
    public float GetInputVerticalVal()
    {
        return _inputManager.RawVerticalVal;
    }
    public float GetInputHorizontalVal()
    {
        return _inputManager.RawHorizontalVal;
    }
    public bool GetInputIsFiring()
    {
        return _inputManager.IsFiring;
    }
    public bool GetInputIsAiming()
    {
        return _inputManager.IsAiming;
    }
    public Vector2 GetInputLookAt()
    {
        return _inputManager.LookAt;
    }
    public void SetAimPointer( Transform aim )
    {
        _inputManager.SetAimPointer( aim );
    }
#endregion

#region [UI Manager]
    public void OnPressAnyKey()
    {
        _uiManager.ShowMainMenu( false );
        OnClickSinglePlayerBtn();
    }
    public void OnClickSinglePlayerBtn()
    {
        _mode = GameMode.Single;
        _uiManager.ShowMainMenu( false );
        LoadLevel();
    }
    public void PlayStartupSFX()
    {
        _audioManager.PlayStartSFX();
    }


    public void UpdateHUDLifes( int life )
    {
        _uiManager.UpdateHUDLifes( life );
    }
    public void UpdateScore( int score )
    {
        _uiManager.UpdateScore( score );
    }

    public void MainMenuToCredits()
    {
        _audioManager.PlayClickBtn();
        _uiManager.ShowMainMenu( false );
        _uiManager.ShowCredits( true );
        _leaderboardManager.LogEvent( PlayGamesPlatformManager.EVENT_ACCESS_INFO );
    }
    public void CreditsToMainMenu()
    {
        _audioManager.PlayClickBtn();
        _uiManager.ShowMainMenu( true );
        _uiManager.ShowCredits( false );
    }
    public void OpenMANA3DGames()
    {
        _leaderboardManager.LogEvent( PlayGamesPlatformManager.EVENT_ACCESS_MANA3DGAMES );
        Application.OpenURL( "http://mana3dgames.com" );
    }


    public void MainMenuToSettings()
    {
        _audioManager.PlayClickBtn();
        _uiManager.ShowMainMenu( false );
        _uiManager.ShowSettings( true );
    }
    public void SettingsToMainMenu()
    {
        _audioManager.PlayClickBtn();
        _uiManager.ShowMainMenu( true );
        _uiManager.ShowSettings( false );
    }

    public void PauseMenuToSettings()
    {
        _audioManager.PlayClickBtn();
        _uiManager.ShowPauseMenu( false );
        _uiManager.ShowSettings( true );
    }
    public void SettingsToPauseMenu()
    {
        _audioManager.PlayClickBtn();
        _uiManager.ShowPauseMenu( true );
        _uiManager.ShowSettings( false );
    }

    public void CheckLeaderboard()
    {
        _audioManager.PlayClickBtn();
        _leaderboardManager.ShowLeaderBoard();
    }

    public void CheckAchievements()
    {
        _audioManager.PlayClickBtn();
        _leaderboardManager.ShowAchievements();
    }
#endregion

#region [Level Manager]
    public float GetLevelLimitation( int index )
    {
        return _levelManager.LevelLimitation[index];
    }
    public void SpawnPlus10Point( Vector3 position )
    {
        _levelManager.SpawnPlus10Point( position );
    }
    public void UpdatePlayerScore( int val )
    {
        _levelManager.UpdatePlayerScore( val );

        if ( _levelManager.PlayerScore >= 5000 )
            ReportAchiUnlocked( 4 );
        else if ( _levelManager.PlayerScore >= 2000 )
            ReportAchiUnlocked( 3 );
        else if ( _levelManager.PlayerScore >= 500 )
            ReportAchiUnlocked( 2 );
        else if ( _levelManager.PlayerScore >= 200 )
            ReportAchiUnlocked( 1 );
    }
    public void OnRecycleEnemy( Vector3 pos )
    {
        _levelManager.OnRecycleEnemy( pos );
    }
    public void UpdatePlayerLifes( int val )
    {
        _levelManager.UpdatePlayerLifes( val );
    }
    public void OnItemClean()
    {
        _levelManager.OnItemClean();
    }
    #endregion

#region [Leaderboard]

    public bool IsGooglePlatformLoggedIn()
    {
        return _leaderboardManager.IsLoggedIn;
    }

    public void LogInGooglePlatform( bool playSFX = true )
    {
        if ( playSFX )
            _audioManager.PlayClickBtn();
        _leaderboardManager.LogIn();
    }

    public void OnLogInGooglePlatform()
    {
        if ( State == GameState.GameOver )
            _leaderboardManager.SubmitScoreToLeaderBorad( _levelManager.PlayerScore );

        _uiManager.ShowLogIn( false );
        ReportAchiUnlocked( 0 );
    }


    public void LogOutGooglePlatform()
    {
        _audioManager.PlayClickBtn();
        _leaderboardManager.LogOut();
    }

    public void OnLogOutGooglePlatform()
    {
        _uiManager.ShowLogIn( true );
    }

    public void ReportAchiUnlocked( int index )
    {
        _leaderboardManager.ReportAchiUnlocked( index );
    }

    #endregion



    IEnumerator CheckBuildVersion()
    {
        //WWW www = new WWW( "http://www.mana3dgames.com/uploads/4/4/7/2/44722523/dsbv.txt" );
        WWW www = new WWW( "http://www.mana3dgames.com/uploads/4/4/7/2/44722523/deadspacebattle_version.txt" );
        yield return www;

        if ( www.error != null )
        {
            Debug.Log( "Error: " + www.error );
            // for example, often 'Error .. 404 Not Found'
        }
        else if ( www.text != Application.version )
        {
            OpenGooglePlayStore();
        }

        /* example code to separate all that text in to lines:
        longStringFromFile = w.text
        List<string> lines = new List<string>(
            longStringFromFile
            .Split(new string[] { "\r","\n" },
            StringSplitOptions.RemoveEmptyEntries) );
        // remove comment lines...
        lines = lines
            .Where(line => !(line.StartsWith("//")
                            || line.StartsWith("#")))
            .ToList();
        */
    }

    //public void OpenGooglePlayStore()
    //{
    //    AndroidJavaClass uriClass = new AndroidJavaClass( "android.net.Uri" );
    //    AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>( "parse", "market://details?id=..." );

    //    AndroidJavaClass intentClass = new AndroidJavaClass( "android.content.Intent" );
    //    AndroidJavaObject intentObject = new AndroidJavaObject( "android.content.Intent",
    //                                                            intentClass.GetStatic<string>( "ACTION_VIEW" ),
    //                                                            uriObject );

    //    AndroidJavaClass unity = new AndroidJavaClass( "com.unity3d.player.UnityPlayer" );
    //    AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>( "currentActivity" );

    //    currentActivity.Call( "startActivity", intentObject );
    //}

    public void OpenGooglePlayStore()
    {
        Application.OpenURL( "market://details?id=" + Application.bundleIdentifier ); 
    }


    public void MinimizeApp()
    {
        AndroidJavaObject activity = new AndroidJavaClass( "com.unity3d.player.UnityPlayer" ).GetStatic<AndroidJavaObject>( "currentActivity" );
        activity.Call<bool>( "moveTaskToBack", true );
    }



    #region [Optimization]
    Dictionary<int, List<GameObject>> _objectPools;

    public void OnCreateObjectID( int ID, GameObject go )
    {
        _objectPools[ID].Add( go );

        if ( _objectPools[ID].Count > 2 )
        {
            GameObject temp = _objectPools[ID][0];
            _objectPools[ID].RemoveAt( 0 );
            Destroy( temp );
        }
    }

    public void OnDestroyObjectID( int ID, GameObject go )
    {
        _objectPools[ID].Remove( go );
    }
    #endregion
}