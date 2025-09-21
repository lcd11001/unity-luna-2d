using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    private AudioSource audioSource;

    public const int MaxVolume = 10;
    public int Volume { get; private set; } = 4;

    private void Awake()
    {
        if (Instance == null)
        {
            audioSource = GetComponent<AudioSource>();
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (audioSource != null)
        {
            audioSource.volume = GetNormalizeVolume();
        }
    }

    public void ChangeVolume(int newValue)
    {
        // Debug.Log($"MusicManager ChangeVolume: old Volume={Volume} new Volume={newValue}");
        Volume = Mathf.Clamp(newValue, 0, MaxVolume);
        if (audioSource != null)
        {
            audioSource.volume = GetNormalizeVolume();
        }
    }

    public float GetNormalizeVolume()
    {
        float normalizedVolume = Mathf.Clamp01((float)Volume / MaxVolume);
        // Debug.Log($"MusicManager GetVolume: Volume={Volume}, MaxVolume={MaxVolume}, Normalized={normalizedVolume}");
        return normalizedVolume;
    }
}
