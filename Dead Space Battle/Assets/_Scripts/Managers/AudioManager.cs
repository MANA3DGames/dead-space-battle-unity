using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
    public AudioClip[] BGMs;
    public AudioClip startSFX;
    public AudioClip clickSFX;


    AudioSource _bgmAS;
    AudioSource _AS;

    void Awake()
    {
        _bgmAS = transform.FindChild( "BGM" ).GetComponent<AudioSource>();
        _AS = transform.FindChild( "AS" ).GetComponent<AudioSource>();

        BGMs = new AudioClip[2];
        BGMs[0] = Resources.Load( "Audio/BGM/MainMenu" ) as AudioClip;
        BGMs[1] = Resources.Load( "Audio/BGM/Gameplay" ) as AudioClip;

        startSFX = Resources.Load( "Audio/Menu/start_SFX" ) as AudioClip;
        clickSFX = Resources.Load( "Audio/Menu/click_SFX" ) as AudioClip;
    }


    public void SetBGM( int id )
    {
        _bgmAS.clip = BGMs[id];
        _bgmAS.Stop();
        _bgmAS.Play();
    }

    public void PlayBGM()
    {
        _bgmAS.mute = false;
    }

    public void StopBGM()
    {
        _bgmAS.mute = true;
    }

    public void PauseBGM()
    {
        _bgmAS.Pause();
    }

    public void UnpauseBGM()
    {
        _bgmAS.UnPause();
    }


    public void PlayClickBtn()
    {
        _AS.PlayOneShot( clickSFX );
    }

    public void PlayStartSFX()
    {
        _AS.PlayOneShot( startSFX );
    }
}
