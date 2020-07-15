using UnityEngine;
using System.Collections;

public class AIDebug : MonoBehaviour 
{
    //#region !!!! Debugging !!!!
    //bool _debugMode = false;
    //bool _isApproaching;
    //bool _isDodoging;
    //bool _isAttacking;
    //bool _isFleeing;


    //void allOff()
    //{
    //    _isFleeing = false;
    //    _isApproaching = false;
    //    _isDodoging = false;
    //    _isAttacking = false;
    //}

    //void OnGUI()
    //{
    //    // Check if debugging is Enabled.
    //    if ( !_debugMode ) return;

    //    // Wrap everything in the designated GUI Area
    //    GUILayout.BeginArea( new Rect( 0, 0, 600, 300 ) );
    //    // Begin the singular Horizontal Group
    //    GUILayout.BeginHorizontal();


    //    #region *** State ***
    //    GUILayout.BeginVertical();
    //    GUILayout.Label("State");
    //        if ( GUILayout.Button( "Approach" ) )
    //        {
    //            allOff();
    //            _isApproaching = true;
    //        }
    //        if ( GUILayout.Button( "Attack" ) )
    //        {
    //            allOff();
    //            _isAttacking = true;

    //            switch ( _info.attackStyle )
    //            {
    //                case AttackStyle.DirectContact:
    //                    _coroutineTask = new CoroutineTask( directContactProcedure( 15 ), true );
    //                    break;
    //                case AttackStyle.Blade_Hurricane:
    //                    _coroutineTask = new CoroutineTask( bladeHurricaneProcedure( 3 ), true );
    //                    break;
    //            }
    //        }
    //        if ( GUILayout.Button( "Dodge" ) )
    //        {
    //            allOff();
    //            _isDodoging = true;
    //        }
    //        if ( GUILayout.Button( "Flee" ) )
    //        {
    //            allOff();
    //            _isFleeing = true;

    //            switch ( _info.fleeStyle )
    //            {
    //                case FleeStyle.BackFlee:
    //                    _targetPos = findRandomFleePoint_Back();
    //                    break;
    //                case FleeStyle.FrontFlee:
    //                    _targetPos = findRandomFleePoint_Front();
    //                    break;
    //                case FleeStyle.SuicidalAtPosition:
    //                    _coroutineTask = new CoroutineTask( suicideBomb( 15 ), true);
    //                    break;
    //                case FleeStyle.SuicidalWhereTarget:
    //                    _targetPos = Player.Instance.transform.position;
    //                    _info.appStyle = ApproachStyle.Enclosure;
    //                    _coroutineTask = new CoroutineTask( suicideBomb( 25 ), true );
    //                    break;
    //                case FleeStyle.BackFleeShooter:
    //                    _targetPos = findRandomFleePoint_Back();
    //                    break;
    //            }
    //        }
    //    GUILayout.EndVertical();
    //    #endregion


    //    #region *** Approach ***
    //    GUILayout.BeginVertical();
    //    GUILayout.Label("Approach Style");
    //        if (GUILayout.Button("Enclosure"))
    //        {
    //            _info.appStyle = ApproachStyle.Enclosure;
    //        }
    //        if (GUILayout.Button("SameQuarter"))
    //        {
    //            _info.appStyle = ApproachStyle.SameQuarter;
    //        }
    //        if (GUILayout.Button("DifferentQuarterUpperLower"))
    //        {
    //            _info.appStyle = ApproachStyle.DifferentQuarterUpperLower;
    //        }
    //        if (GUILayout.Button("DifferentQuarterLeftRight"))
    //        {
    //            _info.appStyle = ApproachStyle.DifferentQuarterLeftRight;
    //        }
    //        if (GUILayout.Button("DifferentQuarterAll"))
    //        {
    //            _info.appStyle = ApproachStyle.DifferentQuarterAll;
    //        }
    //    GUILayout.EndVertical();
    //    #endregion


    //    #region *** Dodge ***
    //    GUILayout.BeginVertical();
    //    GUILayout.Label("Dodge Style");
    //        if (GUILayout.Button("StepSide"))
    //        {
    //            _info.dodgStyle = DodgeStyle.StepSide;
    //        }
    //        if (GUILayout.Button("ChangeQuarter"))
    //        {
    //            _info.dodgStyle = DodgeStyle.ChangeQuarter;
    //        }
    //        if (GUILayout.Button("Disappear"))
    //        {
    //            _info.dodgStyle = DodgeStyle.Disappear;
    //        }
    //    GUILayout.EndVertical();
    //    #endregion


    //    #region *** Attack ***
    //    GUILayout.BeginVertical();
    //    GUILayout.Label("Attack Style");
    //        if (GUILayout.Button("DirectContact"))
    //        {
    //            _info.attackStyle = AttackStyle.DirectContact;
    //        }
    //        if (GUILayout.Button("Normal_Shooter"))
    //        {
    //            _info.attackStyle = AttackStyle.Normal_Shooter;
    //        }
    //        if (GUILayout.Button("Blade_Hurricane"))
    //        {
    //            _info.attackStyle = AttackStyle.Blade_Hurricane;
    //        }
    //        if (GUILayout.Button("Laser_Extended"))
    //        {
    //            _info.attackStyle = AttackStyle.Laser_Extended;
    //        }
    //        if (GUILayout.Button("RocketExtended"))
    //        {
    //            _info.attackStyle = AttackStyle.RocketExtended;
    //        }
    //    GUILayout.EndVertical();
    //    #endregion


    //    #region *** Flee ***
    //    GUILayout.BeginVertical();
    //    GUILayout.Label("Flee Style");
    //        if (GUILayout.Button("BackFlee"))
    //        {
    //            _info.fleeStyle = FleeStyle.BackFlee;
    //        }
    //        if (GUILayout.Button("FrontFlee"))
    //        {
    //            _info.fleeStyle = FleeStyle.FrontFlee;
    //        }
    //        if (GUILayout.Button("SuicidalAtPosition"))
    //        {
    //            _info.fleeStyle = FleeStyle.SuicidalAtPosition;
    //        }
    //        if (GUILayout.Button("SuicidalWhereTarget"))
    //        {
    //            _info.fleeStyle = FleeStyle.SuicidalWhereTarget;
    //        }
    //        if (GUILayout.Button("BackShooter"))
    //        {
    //            _info.fleeStyle = FleeStyle.BackFleeShooter;
    //        }
    //    GUILayout.EndVertical();
    //    #endregion


    //    GUILayout.EndHorizontal();
    //    GUILayout.EndArea();
    //}

    //protected virtual void Update()
    //{
    //    // Do not proceed if it is not active.
    //    if ( !_isActivated ) return;

    //    // *** Just for TESTING ***
    //    if (Input.GetKeyDown(KeyCode.F1)) _debugMode = !_debugMode;
    //    if (Input.GetKeyDown(KeyCode.F2)) reset();
    //    // *** Just for TESTING ***
    //}
    //#endregion
}
