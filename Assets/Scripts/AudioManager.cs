using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource clickSfx;
    public AudioSource swordAttackSfx;
    public AudioSource bowAttackSfx;
    public AudioSource bombChargeSfx;
    public AudioSource bombTickSfx;
    public AudioSource bombExplodeSfx;
    public AudioSource enemyHitSfx;
    public AudioSource enemyShootSfx;
    public AudioSource enemyDeathSfx;
    public AudioSource playerHitSfx;
    public AudioSource fadeInSfx;
    public AudioSource fadeOutSfx;
    public AudioSource swapWeaponSfx;
    public AudioSource pickUpSfx;

    public AudioSource bgm;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Functions that called to play audio
    public void PlayClickSfx()
    {
        clickSfx.Play();
    }

    public void PlaySwordAttackSfx()
    {
        swordAttackSfx.Play();
    }

    public void PlayBowAttackSfx()
    {
        bowAttackSfx.Play();
    }

    public void PlayBombChargeSfx()
    {
        bombChargeSfx.Play();
    }

    public void StopBombChargeSfx()
    {
        bombChargeSfx.Stop();
    }

    public void PlayBombTickSfxFast()
    {
        bombTickSfx.pitch = 1.5f;
        bombTickSfx.Play();
    }

    public void PlayBombTickSfxNormal()
    {
        bombTickSfx.pitch = 0.7f;
        bombTickSfx.Play();
    }

    public void StopBombTickSfx()
    {
        bombTickSfx.Stop();
    }

    public void PlayBombExplodeSfx()
    {
        bombExplodeSfx.Play();
    }

    public void PlayEnemyHitSfx()
    {
        enemyHitSfx.Play();
    }

    public void PlayEnemyShootSfx()
    {
        enemyShootSfx.Play();
    }

    public void PlayEnemyDeathSfx()
    {
        enemyDeathSfx.Play();
    }

    public void PlayPlayerHitSfx()
    {
        playerHitSfx.Play();
    }

    public void PlayFadeInSfx()
    {
        fadeInSfx.Play();
    }

    public void PlayFadeOutSfx()
    {
        fadeOutSfx.Play();
    }

    public void PlaySwapWeaponSfx()
    {
        swapWeaponSfx.Play();
    }

    public void PlayPickUpSfx()
    {
        pickUpSfx.Play();
    }

    public void SetBGMVolume(System.Single volume)
    {
        bgm.volume = 0.13f * volume;
    }
}
