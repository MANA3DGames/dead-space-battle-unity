using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class PlayGamesPlatformManager : MonoBehaviour
{
    struct AchievementStruct
    {
        public string ID;
        public Achievement data;
    }

    AchievementStruct[] _achievements;

    public const string EVENT_ACCESS_INFO = "CgkI9pSA_tceEAIQAg";
    public const string EVENT_ACCESS_MANA3DGAMES = "CgkI9pSA_tceEAIQAw";
    public const string EVENT_CLICKED_PLAYAGAIN_BUTTON = "CgkI9pSA_tceEAIQBA";


    /// <summary>
    /// Top Scores leaderboard.
    /// </summary>
    string _leaderboard = "CgkI9pSA_tceEAIQAA";



    /// <summary>
    /// Init Leaderboard manager.
    /// </summary>
    void Start()
    {
        // recommended for debugging.
        PlayGamesPlatform.DebugLogEnabled = true;

        // Activate the Google Play Games platform.
        PlayGamesPlatform.Activate();

        //// Login the current user.
        //LogIn();

        _achievements = new AchievementStruct[5];

        // Facebook Login
        _achievements[0] = new AchievementStruct();
        _achievements[0].ID = "CgkI9pSA_tceEAIQBQ";

        // Beat My Score
        _achievements[1] = new AchievementStruct();
        _achievements[1].ID = "CgkI9pSA_tceEAIQBg";

        // 500 Points
        _achievements[2] = new AchievementStruct();
        _achievements[2].ID = "CgkI9pSA_tceEAIQBw";

        // 2,000 Points
        _achievements[3] = new AchievementStruct();
        _achievements[3].ID = "CgkI9pSA_tceEAIQCA";

        // 5,000 Points
        _achievements[4] = new AchievementStruct();
        _achievements[4].ID = "CgkI9pSA_tceEAIQCQ";
    }


    /// <summary>
    /// Login In Into Your Google+ Account
    /// </summary>
    public void LogIn()
    {
        Social.localUser.Authenticate( ( bool success ) =>
        {
            if ( success )
            {
                //Debug.Log( "Login Sucess" );
                GameManager.Instance.OnLogInGooglePlatform();
            }
            else
            {
                //Debug.Log( "Login failed" );
                GameManager.Instance.OnLogOutGooglePlatform();
            }
        } );
    }

    /// <summary>
    /// Logout of your Google+ Account
    /// </summary>
    public void LogOut()
    {
        ( (PlayGamesPlatform)Social.Active ).SignOut();

        GameManager.Instance.OnLogOutGooglePlatform();
    }


    /// <summary>
    /// Shows All Available Leaderborad
    /// </summary>
    public void ShowAllLeaderBoards()
    {
        Social.ShowLeaderboardUI(); 
    }

    /// <summary>
    /// Show current (Active) leaderboard.
    /// </summary>
    public void ShowLeaderBoard()
    {
        if ( Social.localUser.authenticated )
            ( (PlayGamesPlatform)Social.Active ).ShowLeaderboardUI( _leaderboard );
        else
            LogIn();
    }


    public void ShowAchievements()
    {
        if ( Social.localUser.authenticated )
            ( (PlayGamesPlatform)Social.Active ).ShowAchievementsUI();
        else
            LogIn();
    }


    /// <summary>
    /// Submits Score To leader board.
    /// </summary>
    public void SubmitScoreToLeaderBorad( int newScore )
    {
        if ( Social.localUser.authenticated )
        {
            Social.ReportScore( newScore, _leaderboard, ( bool success ) =>
            {
                if ( success )
                {
                    Debug.Log( "Update Score Success" );
                }
                else
                {
                    Debug.Log( "Update Score Fail" );
                }
            } );
        }
    }

    public bool IsLoggedIn { get { return Social.localUser.authenticated; } }


    public void LogEvent( string id )
    {
        if ( PlayGamesPlatform.Instance != null &&
             PlayGamesPlatform.Instance.Events != null )
            PlayGamesPlatform.Instance.Events.IncrementEvent( id, 1 );
    }



    
    bool GetAchiIsUnlockedState( int index )
    {
        if ( _achievements[index].data == null )
            _achievements[index].data = ( (PlayGamesPlatform)Social.Active ).GetAchievement( _achievements[index].ID );

        if (  _achievements[index].data != null )
            return _achievements[index].data.IsUnlocked;
        else
            return false;
    }

    public void ReportAchiUnlocked( int index )
    {
        if ( !GetAchiIsUnlockedState( index ) )
            ( (PlayGamesPlatform)Social.Active ).ReportProgress( _achievements[index].ID, 100, ( success )=> { } );
    }
}
