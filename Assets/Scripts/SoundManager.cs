using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioClip fuelPickupClip;
    [SerializeField] private AudioClip coinPickupClip;
    [SerializeField] private AudioClip landingSuccessClip;
    [SerializeField] private AudioClip crashClip;

    public const int MaxVolume = 10;
    public int Volume { get; private set; } = 5;

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

    public void ChangeVolume(int newValue)
    {
        Debug.Log($"SoundManager ChangeVolume: old Volume={Volume} new Volume={newValue}");
        Volume = Mathf.Clamp(newValue, 0, MaxVolume);
    }

    public float GetNormalizeVolume()
    {
        float normalizedVolume = Mathf.Clamp01((float)Volume / MaxVolume);
        Debug.Log($"SoundManager GetVolume: Volume={Volume}, MaxVolume={MaxVolume}, Normalized={normalizedVolume}");
        return normalizedVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, GetNormalizeVolume());
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
