using UnityEngine;

public class PauseMenu_UI : MonoBehaviour
{
    public void OnClickResumeBtn()
    {
        GameManager.Instance.ResumeGame();
    }

    public void OnClickSettingsBtn()
    {
        GameManager.Instance.PauseMenuToSettings();
    }

    public void OnClickStoreBtn()
    {

    }

    public void OnClickRestartGameBtn()
    {
        GameManager.Instance.ResumeGame();
        GameManager.Instance.RestartGame();
    }

    public void OnClickQuitGameBtn()
    {
        Application.Quit();
    }


    public void OnClickLogInBtn()
    {
        GameManager.Instance.LogInGooglePlatform();
    }

    public void OnClickLogOutBtn()
    {
        GameManager.Instance.LogOutGooglePlatform();
    }
}
