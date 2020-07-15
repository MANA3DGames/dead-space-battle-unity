using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu_UI : MonoBehaviour
{
    public Button moveNormalBtn;
    public Button moveLargeBtn;
    public Button fireNormalBtn;
    public Button fireLargeBtn;

    public EasyJoystick moveJoysticks;
    public EasyJoystick fireJoysticks;


    void OnEnable()
    {
        moveNormalBtn.interactable = GameManager.Instance.joystickSettings.moveJoystickSize != 0;
        moveLargeBtn.interactable = GameManager.Instance.joystickSettings.moveJoystickSize == 0;

        fireNormalBtn.interactable = GameManager.Instance.joystickSettings.fireJoystickSize != 0;
        fireLargeBtn.interactable = GameManager.Instance.joystickSettings.fireJoystickSize == 0;

        ShowJoysticks( true );
    }


    public void OnClick_MoveNormalBtn()
    {
        moveNormalBtn.interactable = false;
        moveLargeBtn.interactable = true;

        moveJoysticks.ZoneRadius = 50;
        moveJoysticks.TouchSize = 15;
        moveJoysticks.deadZone = 10;
    }

    public void OnClick_MoveLargeBtn()
    {
        moveNormalBtn.interactable = true;
        moveLargeBtn.interactable = false;

        moveJoysticks.ZoneRadius = 75;
        moveJoysticks.TouchSize = 22.5f;
        moveJoysticks.deadZone = 15;
    }

    public void OnClick_FireNormalBtn()
    {
        fireNormalBtn.interactable = false;
        fireLargeBtn.interactable = true;

        fireJoysticks.ZoneRadius = 50;
        fireJoysticks.TouchSize = 15;
        fireJoysticks.deadZone = 0;
    }

    public void OnClick_FireLargeBtn()
    {
        fireNormalBtn.interactable = true;
        fireLargeBtn.interactable = false;

        fireJoysticks.ZoneRadius = 75;
        fireJoysticks.TouchSize = 22.5f;
        fireJoysticks.deadZone = 0;
    }


    public void OnClick_SaveBtn()
    {
        GameManager.Instance.SetMoveJoysticksSettings( moveNormalBtn.interactable ? 1 : 0 );
        GameManager.Instance.SetFireJoysticksSettings( fireNormalBtn.interactable ? 1 : 0 );

        ShowJoysticks( false );
        if ( GameManager.Instance.State == GameState.Title )
            GameManager.Instance.SettingsToMainMenu();
        else
            GameManager.Instance.SettingsToPauseMenu();
    }

    public void OnClick_CancelBtn()
    {
        if ( GameManager.Instance.joystickSettings.moveJoystickSize == 0 )
            OnClick_MoveNormalBtn();
        else
            OnClick_MoveLargeBtn();

        if ( GameManager.Instance.joystickSettings.fireJoystickSize == 0 )
            OnClick_FireNormalBtn();
        else
            OnClick_FireLargeBtn();

        ShowJoysticks( false );
        if ( GameManager.Instance.State == GameState.Title )
            GameManager.Instance.SettingsToMainMenu();
        else
            GameManager.Instance.SettingsToPauseMenu();
    }


    void ShowJoysticks( bool show )
    {
        moveJoysticks.enable = show;
        moveJoysticks.isActivated = !show;

        fireJoysticks.enable = show;
        fireJoysticks.isActivated = !show;
    }
}
