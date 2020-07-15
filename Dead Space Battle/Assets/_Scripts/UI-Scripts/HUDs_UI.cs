using UnityEngine;
using UnityEngine.UI;

public class HUDs_UI : MonoBehaviour 
{
    public Text lifeVal;
    public Text scoreVal;

    public void updateLife( int val )
    {
        lifeVal.text = val + "";
    }

    public void updateScore( int val )
    {
        scoreVal.text = MANA3D.Utilities.String.StringOperation.AddCommaToNumber( val );
    }

    public void OnClickPauseBtn()
    {
        GameManager.Instance.PauseGame();
    }
}
