using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    public void PlaySound(AudioClip clip, float volume, bool repeating = false)
    {
        if (repeating)
        {
            audioSource.volume = volume;
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
            audioSource.PlayOneShot(clip, volume);
    }
}
