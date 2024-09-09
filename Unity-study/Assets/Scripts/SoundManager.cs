using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogWarning("SoundManager가 강제로 생성됨");
                GameObject emptyGameManager = new GameObject("Generated Sound Manager");
                instance = emptyGameManager.AddComponent<SoundManager>();
            }

            return instance;
        }
    }

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    public void PlaySFX(AudioClip clip, float volumeScale) => sfxSource.PlayOneShot(clip, volumeScale);
}
