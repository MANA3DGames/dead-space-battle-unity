using UnityEngine;
using System.Collections;
using MANA3D.Utilities.Coroutine;
using MANA.UITweenUtil;
using MANA3D.Utilities.Security;
using MANA3D.Utilities.Optimization;


public class EnvironmentElements
{
    public GameObject ScrollingStarBG;
    public GameObject LevelCleaner;
    //public GameObject HugeSpaceship;
}

//public class HighScore
//{
//    public string name;
//    public int score;
//    public int rank;
//}

public class LevelManager : MonoBehaviour
{
    //GameObject playerPrefab;
    //GameObject enemyPrefab;
    //GameObject minePrefabs;


    bool _resourcesIsLoaded;

    EnvironmentElements _environment;
    ObjectPool _enemy1_Pool;
    ObjectPool _guiPoint_Pool;
    ObjectPool[] _spaceItems_Pools;

    

    PlayerPrefs _prefs;

    //public HighScore[] HighScores { get { return _highScores; } }
    //HighScore[] _highScores;


    public int PlayerLifes { get { return _playerLifes; } }
    int _playerLifes;

    public int PlayerScore { get { return _score; } }
    int _score;

    int _enemyCount = 0;
    int _totalEnemySpawned = 0;


    public enum QuarterArea
    {
        Unknown,
        UpperRight,
        UpperLeft,
        LowerRight,
        LowerLeft
    }

    /// <summary>
    /// Level boundary limitations.
    /// </summary>
    public float[] LevelLimitation { get { return _levelLimitation; } }
    float[] _levelLimitation = { -150.0f, 150.0f, -80.0f, 80.0f };

    CoroutineTask _levelProcedure;

    int _activeItemsCount;

    protected bool _isPaused;



    void OnEnable()
    {
        GameManager.Instance.onGamePaused += onGamePausedHandler;
    }

    void OnDisable()
    {
        GameManager.Instance.onGamePaused -= onGamePausedHandler;
    }

    void onGamePausedHandler( bool pause )
    {
        _isPaused = pause;

        if ( pause )
        {
            if ( _levelProcedure != null )
            {
                if ( _levelProcedure.IsRunning )
                    _levelProcedure.pause();
            }
        }
        else
        {
            if ( _levelProcedure != null )
            {
                if ( _levelProcedure.IsPaused )
                    _levelProcedure.unPause();
            }
        }
    }


    void Awake()
    {
        PlayerPrefsExtensions.setKeys_test();
        _prefs = new PlayerPrefs();
        _prefs.initialize();

        //if ( !_prefs.hasSecureKey( "defaultHS" ) )
        //{
        //    _prefs.save( "defaultHS", 1 );

        //    string[] names = { "Mahmoud", "Touz", "Malik", "Jone", "Ahmad", "Joe", "Yassir", "King", "Jozz", "MoonLight" };
        //    for ( int i = 0; i < 10; i++ )
        //    {
        //        _prefs.save( "HS_Name_" + i, names[i] );
        //        _prefs.save( "HS_Score_" + i, 100 - ( i * 10 ) );
        //    }
        //}

        //GetHighScore();
    }


    IEnumerator LevelProcedure()
    {
        _score = 0;
        _enemyCount = 0;
        _totalEnemySpawned = 0;

        // Difficulty:
        // - Behavior.
        // - Speed.
        // - Fire rate.
        // - Damage.

        // ***** Waiting time - depends on enemy type and number for each phase. ***** \\
        yield return new WaitForSeconds( 3.0f );

        while ( true )
        {
            if ( _enemyCount < 5 )
            {
                int total = 1;
                if ( _totalEnemySpawned > 50 )
                    total = 5;
                else if ( _totalEnemySpawned > 30 )
                    total = 4;
                else if ( _totalEnemySpawned > 10 )
                    total = 3;
                else if ( _totalEnemySpawned > 5 )
                    total = 2;

                for ( int i = 0; i < total; i++ )
                {
                    switch ( i )
                    {
                        case 0:
                            SpawnEnemy( GetRandomEnemy_Front() );
                            break;
                        case 1:
                            SpawnEnemy( GetRandomEnemy_Back() );
                            break;
                        case 2:
                            SpawnEnemy( GetRandomEnemy_Front() );
                            SpawnEnemy( GetRandomEnemy_RightSide() );
                            break;
                        case 3:
                            SpawnEnemy( GetRandomEnemy_RightSide() );
                            SpawnEnemy( GetRandomEnemy_LeftSide() );
                            break;
                    }


                    if ( _totalEnemySpawned > 100 )
                        yield return new WaitForSeconds( 0.05f );
                    else if ( _totalEnemySpawned > 80 )
                        yield return new WaitForSeconds( 0.1f );
                    else if ( _totalEnemySpawned > 70 )
                        yield return new WaitForSeconds( 0.15f );
                    else if ( _totalEnemySpawned > 60 )
                        yield return new WaitForSeconds( 0.2f );
                    else if ( _totalEnemySpawned > 50 )
                        yield return new WaitForSeconds( 0.25f );
                    else if ( _totalEnemySpawned > 40 )
                        yield return new WaitForSeconds( 0.3f );
                    else if ( _totalEnemySpawned > 30 )
                        yield return new WaitForSeconds( 0.35f );
                    else if ( _totalEnemySpawned > 20 )
                        yield return new WaitForSeconds( 0.4f );
                    else if ( _totalEnemySpawned > 10 )
                        yield return new WaitForSeconds( 0.45f );
                    else
                        yield return new WaitForSeconds( 0.5f );
                }  
            }

            if ( _totalEnemySpawned > 100 )
                yield return new WaitForSeconds( 0.5f );
            else if ( _totalEnemySpawned > 80 )
                yield return new WaitForSeconds( 1f );
            else if ( _totalEnemySpawned > 70 )
                yield return new WaitForSeconds( 1.5f );
            else if ( _totalEnemySpawned > 60 )
                yield return new WaitForSeconds( 2f );
            else if ( _totalEnemySpawned > 50 )
                yield return new WaitForSeconds( 2.5f );
            else if ( _totalEnemySpawned > 40 )
                yield return new WaitForSeconds( 3f );
            else if ( _totalEnemySpawned > 30 )
                yield return new WaitForSeconds( 3.5f );
            else if ( _totalEnemySpawned > 20 )
                yield return new WaitForSeconds( 4f );
            else if ( _totalEnemySpawned > 10 )
                yield return new WaitForSeconds( 4.5f );
            else
                yield return new WaitForSeconds( 5f );
        }
    }

    Vector3 GetRandomEnemy_Front()
    {
        return new Vector3( Random.Range( -180, 180 ), 0, 150 );
    }
    Vector3 GetRandomEnemy_Back()
    {
        return new Vector3( Random.Range( -180, 180 ), 0, -100 );
    }
    Vector3 GetRandomEnemy_RightSide()
    {
        return new Vector3( 190, 0, Random.Range( -180, 180 ) );
    }
    Vector3 GetRandomEnemy_LeftSide()
    {
        return new Vector3( -190, 0, Random.Range( -180, 180 ) );
    }

    void SpawnEnemy( Vector3 point )
    {
        //_enemy1_Pool.getNextFree().transform.position = point;
        GameObject go = _enemy1_Pool.getNextFree();
        go.transform.position = point;
        Enemy enemy = go.GetComponent<Enemy>();

        if ( _totalEnemySpawned > 100 )
        {
            enemy.CreateWeapon( 10000000, 10000000, 10, 0.3f, 280 );
        }
        else if ( _totalEnemySpawned > 80 )
        {
            enemy.CreateWeapon( 10000000, 10000000, 10, 0.3f, 260 );
        }
        else if ( _totalEnemySpawned > 70 )
        {
            enemy.CreateWeapon( 10000000, 10000000, 10, 0.35f, 260 );
        }
        else if ( _totalEnemySpawned > 60 )
        {
            enemy.CreateWeapon( 10000000, 10000000, 10, 0.35f, 240 );
        }
        else if ( _totalEnemySpawned > 50 )
        {
            enemy.CreateWeapon( 10000000, 10000000, 10, 0.4f, 240 );
        }
        else if ( _totalEnemySpawned > 40 )
        {
            enemy.CreateWeapon( 10000000, 10000000, 10, 0.4f, 230 );
        }
        else if ( _totalEnemySpawned > 30 )
        {
            enemy.CreateWeapon( 10000000, 10000000, 10, 0.45f, 200 );
        }
        else if ( _totalEnemySpawned > 20 )
        {
            enemy.CreateWeapon( 10000000, 10000000, 10, 0.45f, 200 );
        }
        else if ( _totalEnemySpawned > 10 )
        {
            enemy.CreateWeapon( 10000000, 10000000, 10, 0.5f, 200 );
        }
        else
        {
            enemy.CreateWeapon( 10000000, 10000000, 10, 0.5f, 200 );
        }

        _enemyCount++;
        _totalEnemySpawned++;
    }


    public void OnRecycleEnemy( Vector3 position )
    {
        _enemyCount--;

        int giveItem = Random.Range( 0, 11 );

        if ( giveItem % 2 == 0 )
        {
            // Check total active count
            if ( _activeItemsCount > 4 ) return;

            int max = Random.Range( 0, 10 ) > 5 ? _spaceItems_Pools.Length : _spaceItems_Pools.Length - 1;
            int id = Random.Range( 0, max );

            if ( _playerLifes >= 10 && id == 3 )
                id = 0;

            _spaceItems_Pools[id].getNextFree().transform.position = position;
            _activeItemsCount++;
        }
    }

    public void OnItemClean()
    {
        _activeItemsCount--;
    }

    public void StartLevel(int levelID)
    {
        // Load level assets.


        // Start level procedure.
        _levelProcedure = new CoroutineTask( LevelProcedure(), true );
    }

    public void PauseLevelProcedure()
    {
        _levelProcedure.pause();
    }

    public void ResumeLevelProcedure()
    {
        _levelProcedure.unPause();
    }

    public void RestartLevel()
    {
        DeactivateAll();
        ResetPlayerLifes();
        ResetPlayerScore();
        ActivateAll();
    }



    public IEnumerator LoadLevel( System.Action<bool> onComplete )
    {
        if ( !_resourcesIsLoaded )
           LoadResources();

        yield return new WaitForSeconds( 0.1f );

        if ( onComplete != null )
            onComplete( true );
    }

    void LoadResources()
    {
        _environment = new EnvironmentElements();

        GameObject playerPrefab = Resources.Load( "Player/Player" ) as GameObject;

        GameObject aim = Instantiate( Resources.Load( "Player/Aiming" ) as GameObject );
        ColorTweenSprite colorTween = aim.transform.FindChild("AimPointer").gameObject.AddComponent<ColorTweenSprite>();
        colorTween.blendAlphaIn = true;
        colorTween.destroyWhenDone = false;
        GameManager.Instance.SetAimPointer( aim.transform.FindChild( "AimPointer" ).transform );

        Instantiate( playerPrefab );

        // Only once.
        _environment.ScrollingStarBG = Instantiate(Resources.Load("Level/ScrollingStarBG") as GameObject);
        _environment.LevelCleaner = Instantiate(Resources.Load("Level/LevelCleaner") as GameObject);
        //_environment.HugeSpaceship = Instantiate(Resources.Load("Level/HugeSpaceship") as GameObject);


        // Object Pool
        GameObject enemyPrefab = Resources.Load( "Enemies/Enemy_2" ) as GameObject;
        _enemy1_Pool = new ObjectPool( enemyPrefab, 3, "Enemy1Pool" );

        _guiPoint_Pool = new ObjectPool( Resources.Load( "GUIPoints/+10Point" ) as GameObject, 3, "+10PointGUI_Pool" );

        _spaceItems_Pools = new ObjectPool[4];
        for ( int i = 0; i < _spaceItems_Pools.Length; i++ )
            _spaceItems_Pools[i] = new ObjectPool( Resources.Load( "Items/" + i ) as GameObject, 1, "SpaceItem" + i + "_Pool" );


        _resourcesIsLoaded = true;
    }

    //void CreateStage()
    //{
    //    GameObject stagePrefab = Resources.Load( "Leve/Stage" ) as GameObject;
    //    GameObject stage1 = Instantiate( stagePrefab, new Vector3( 0, 0, 466 ), Quaternion.Euler( 0, 180 ,0 ) ) as GameObject;
    //    GameObject stage2 = Instantiate( stagePrefab, new Vector3( 0, 0, 804 ), Quaternion.Euler( 0, 180 ,0 ) ) as GameObject;

    //    stage1.GetComponent<Stage>().otherStage = stage2.transform;
    //    stage2.GetComponent<Stage>().otherStage = stage1.transform;
    //}

    public void UpdatePlayerLifes( int val )
    {
        _playerLifes += val;
        if ( _playerLifes >= 10 )
            _playerLifes = 10;

        GameManager.Instance.UpdateHUDLifes( _playerLifes );

        if ( val < 0 )
        {
            if ( _playerLifes <= 0 )
                GameManager.Instance.EndGame();
            else
                Player.Instance.respawn();
        }
    }

    public void ResetPlayerLifes()
    {
        _playerLifes = 0;
        GameManager.Instance.UpdateHUDLifes( _playerLifes );
    }


    public void DeactivateAll()
    {
        Player.Instance.Activate( false );

        if ( _levelProcedure != null )
        {
            _levelProcedure.kill();
            _levelProcedure = null;
        }

        _environment.ScrollingStarBG.SetActive( false );
        _environment.LevelCleaner.SetActive( false );
        //_environment.HugeSpaceship.SetActive( false );

        //Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        //foreach ( var enemy in enemies )
        //    enemy.sendToRecycle();

        ObjectInfo[] infos = GameObject.FindObjectsOfType<ObjectInfo>();
        foreach (var i in infos)
            i.recycle();
    }

    public void ActivateAll()
    {
        Player.Instance.Activate( true );

        _environment.ScrollingStarBG.SetActive( true );
        _environment.LevelCleaner.SetActive( true );
        //_environment.HugeSpaceship.SetActive( true );
        //_environment.HugeSpaceship.GetComponent<MovingItem>().reset();
    }


    public void UpdatePlayerScore( int val )
    {
        _score += val;
        GameManager.Instance.UpdateScore( _score );
    }

    public void ResetPlayerScore()
    {
        _score = 0;
        GameManager.Instance.UpdateScore( _score );
    }


    public void SpawnPlus10Point( Vector3 position )
    {
        _guiPoint_Pool.getNextFree().transform.position = position;
    }



    //void GetHighScore()
    //{
    //    _highScores = new HighScore[10];
    //    for ( int i = 0; i < _highScores.Length; i++ )
    //    {
    //        _highScores[i] = new HighScore();
    //        _highScores[i].name = _prefs.loadString( "HS_Name_" + i  );
    //        _highScores[i].score = _prefs.loadInt( "HS_Score_" + i  );
    //        _highScores[i].rank = i;
    //    }
    //}

    //public void UpdateHighScores( string strName, int score )
    //{
    //    for ( int i = 0; i < _highScores.Length; i++ )
    //    {
    //        if ( score > _highScores[i].score )
    //        {
    //            for ( int j = _highScores.Length - 1; j > i; j-- )
    //            {
    //                _prefs.save( "HS_Name_" + j, _highScores[j-1].name );
    //                _prefs.save( "HS_Score_" + j, _highScores[j-1].score );
    //            }

    //            _prefs.save( "HS_Name_" + i, strName );
    //            _prefs.save( "HS_Score_" + i, score );
    //            break;
    //        }
    //    }

    //    GetHighScore();
    //}
}
