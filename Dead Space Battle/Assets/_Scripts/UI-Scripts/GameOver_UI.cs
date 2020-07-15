using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOver_UI : MonoBehaviour 
{
    public void OnClick_PlayAgainBtn()
    {
        GameManager.Instance.OnClickPlayAgain();
    }

    public void OnClick_QuitGameBtn()
    {
        Application.Quit();
    }

    public void OnClick_StoreBtn()
    {

    }


    public void OnClickLeaderboardBtn()
    {
        GameManager.Instance.CheckLeaderboard();
    }

    public void OnClickAchievementBtn()
    {
        GameManager.Instance.CheckAchievements();
    }



    public void OnClickLogInBtn()
    {
        GameManager.Instance.LogInGooglePlatform();
    }
}
