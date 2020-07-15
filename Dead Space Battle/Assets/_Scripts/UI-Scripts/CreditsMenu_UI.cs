using UnityEngine;
using System.Collections;

public class CreditsMenu_UI : MonoBehaviour
{
    float _startTime;

    void OnEnable()
    {
        _startTime = Time.time;
    }

	public void OnClickBackBtn()
    {
        if ( Time.time < _startTime + 0.3f ) return;

        GameManager.Instance.CreditsToMainMenu();
    }

    public void OnClickLink()
    {
        GameManager.Instance.OpenMANA3DGames();
    }
}
