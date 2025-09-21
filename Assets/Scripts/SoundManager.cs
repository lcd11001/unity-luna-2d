using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioClip fuelPickupClip;
    [SerializeField] private AudioClip coinPickupClip;
    [SerializeField] private AudioClip landingSuccessClip;
    [SerializeField] private AudioClip crashClip;

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

    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }

    public void OnFuelPickup(float amount)
    {
        PlaySFX(fuelPickupClip);
    }

    public void OnCoinPickup(int amount)
    {
        PlaySFX(coinPickupClip);
    }

    public void OnLanding(OnLandingEvent landingEvent)
    {
        if (landingEvent.type == LandingType.Success)
        {
            PlaySFX(landingSuccessClip);
        }
        else
        {
            PlaySFX(crashClip);
        }
    }
}
