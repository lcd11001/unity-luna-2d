using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LanderAudio : MonoBehaviour
{
    [SerializeField] private Lander lander;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        audioSource.loop = true;
        audioSource.Pause();

        if (lander != null)
        {
            lander.OnBeforeForce += Lander_OnBeforeForce;
            lander.OnUpForce += Lander_OnUpForce;
            lander.OnLeftForce += Lander_OnLeftForce;
            lander.OnRightForce += Lander_OnRightForce;
        }
    }

    private void OnDestroy()
    {
        if (lander != null)
        {
            lander.OnBeforeForce -= Lander_OnBeforeForce;
            lander.OnUpForce -= Lander_OnUpForce;
            lander.OnLeftForce -= Lander_OnLeftForce;
            lander.OnRightForce -= Lander_OnRightForce;
        }
    }

    public void PlayThrustSound()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void PauseThrustSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    private void Lander_OnBeforeForce(object sender, System.EventArgs e)
    {
        PauseThrustSound();
    }

    private void Lander_OnUpForce(object sender, System.EventArgs e)
    {
        PlayThrustSound();
    }

    private void Lander_OnLeftForce(object sender, System.EventArgs e)
    {
        PlayThrustSound();
    }

    private void Lander_OnRightForce(object sender, System.EventArgs e)
    {
        PlayThrustSound();
    }
}
