using UnityEngine;
using System.Collections;
using MANA3D.Utilities.Coroutine;
using MANA3D.Utilities.Optimization;


public class BasicGun
{
    /********************************************************************************************************************/
    // BasicGun: Private global variables
    /********************************************************************************************************************/
    private int _bulletsNum;                    // Current number of bullets.
    private int _bulletsMax;                    // Maximum capacity of bullets for this gun.
    private float _damage;                      // The amount of damage that caused by this gun.
    private AudioClip _fireSFX;                 // Fire sound effect for this gun.
    private float _sfxVolume;
    private float _speed;
    private Renderer _muzzleFlashRenderer;      // Renderer component of the muzzle flash.
    private Transform _muzzleFlashTransform;    // Transform component of the muzzle flash.
    private Light _muzzleFlashLight;            // Reference for muzzle flash light which turn on/off while firing.
    private GameObject _bulletPrefab;           // Spark prefab which will be instantiate on the hit point.      
    private float[] _offSetMatrix;              // This will be used to adjust the offset of the muzzle flash texture.
    private Transform _spawnPoint;              // Bullet's spawn point. (usually the camera).
    private float _fireRate;
    private float _lastFireTime;

    private CoroutineTask _muzzleFlashCoroutine;
    private ObjectPool _bulletsPool;

    private bool _isEndless;

    private AudioSource _audioSource;



    /********************************************************************************************************************/
    // BasicGun : Public Constructors
    /********************************************************************************************************************/
    public BasicGun( int initAmmo, int max, float damage, float rate, float speed,
                     AudioClip sfx, Transform gunTrans, Light lit, GameObject bullet, Transform spawn,
                     string packageName, string ignoreTag, float sfxVolume = 0.1f )
    {
        // Fill basic gun instance values and compoenets.
        _bulletsNum = initAmmo;
        _bulletsMax = max;
        _damage = damage;
        _fireRate = rate;
        _fireSFX = sfx;
        _muzzleFlashLight = lit;
        _bulletPrefab = bullet;
        _spawnPoint = spawn;
        _speed = speed;
        _sfxVolume = sfxVolume;

        // Get the transfrom component of the muzzle flash.
        _muzzleFlashTransform = gunTrans.FindChild("MuzzleFlash");
        // Trun off muzzle flash renderer at the beginning.
        _muzzleFlashRenderer = _muzzleFlashTransform.GetComponent<Renderer>();
        _muzzleFlashRenderer.enabled = false;
        // Trun off muzzle flash light at the beginning.
        _muzzleFlashLight.enabled = false;

        _audioSource = _muzzleFlashTransform.GetComponent<AudioSource>();
        if ( !_audioSource )
            _audioSource = _muzzleFlashTransform.gameObject.AddComponent<AudioSource>();

        // Initilaize offset matrix.
        _offSetMatrix = new float[2];
        _offSetMatrix[0] = 0.0f;
        _offSetMatrix[1] = 0.5f;


        // Setup bullet properties.
        _bulletPrefab.GetComponent<Bullet>().ignoreTag = ignoreTag;
        _bulletPrefab.GetComponent<Bullet>().speed = _speed;
        _bulletPrefab.GetComponent<Bullet>().strength = _damage;
        _bulletsPool = new ObjectPool(_bulletPrefab, 1, packageName);


        _muzzleFlashCoroutine = new CoroutineTask(turnOffMuzzle(), false);
    }



    /********************************************************************************************************************/
    // BasicGun : Private Functions
    /********************************************************************************************************************/
    void turnOnMuzzleFlash()
    {
        // enable muzzle flash renderer and inner light.
        _muzzleFlashRenderer.enabled = true;
        _muzzleFlashLight.enabled = true;
        // Set random offset for texture of the renderer compoent of the muzzle flash.
        _muzzleFlashRenderer.material.SetTextureOffset( "_MainTex", new Vector2( _offSetMatrix[Random.Range(0, 2)], 0 ) );

        
        if ( _muzzleFlashCoroutine.IsRunning )
            _muzzleFlashCoroutine.kill();

        // Start turn off muzzle flash timer.
        _muzzleFlashCoroutine = new CoroutineTask( turnOffMuzzle() );
    }

    IEnumerator turnOffMuzzle()
    {
        yield return new WaitForSeconds( 0.03f );

        // Trun off the the renderer and the light of the muzzle flash.
        _muzzleFlashTransform.GetComponent<Renderer>().enabled = false;
        _muzzleFlashLight.enabled = false;
    }



    /********************************************************************************************************************/
    // BasicGun : Public Functions
    /********************************************************************************************************************/
    public void fire()
    {
        _lastFireTime = Time.time;

        // Play fire SFX.
        //AudioSource.PlayClipAtPoint( _fireSFX, _muzzleFlashTransform.position, _sfxVolume );
        _audioSource.PlayOneShot( _fireSFX, _sfxVolume );

        // Turn on the muzzle flash.
        turnOnMuzzleFlash();

        // Spawn a bullet.
        GameObject bullet = _bulletsPool.getNextFree();
        bullet.transform.position = _spawnPoint.position;
        bullet.transform.rotation = _spawnPoint.rotation;

        // Check if it is not an endless weapon.
        if ( !_isEndless )
            // Decrement the current amount of bullets. 
            _bulletsNum--;
    }

    public void addAmmo( int ammo )
    {
        // Add ammo to the current existing ammo.
        _bulletsNum += ammo;

        // We don't want the current number of bullets to exceed the maximum bullets number for this gun.
        if ( _bulletsNum > _bulletsMax )
            _bulletsNum = _bulletsMax;
    }

    // This should call from outside before calling fire function (just to prevent animation)
    public bool canFire()
    {
        // Return true if there is an enough number of bullets.
        return _bulletsNum > 0 && Time.time > _lastFireTime + _fireRate;
    }


    public void disableGun()
    {
        _muzzleFlashCoroutine.kill();
    }

    public void setAsEndless( bool endless )
    {
        _isEndless = endless;
    }

    public void setFireRate( float rate )
    {
        _fireRate = rate;
    }

    public int GetAmmoCount()
    {
        return _bulletsNum;
    }

    public void ResetAmmo()
    {
        _bulletsNum = 0;
    }

    public float GetFireRate()
    {
        return _fireRate;
    }
}
