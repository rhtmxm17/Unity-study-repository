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

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volumeScale = 1f) => sfxSource.PlayOneShot(clip, volumeScale);
}
