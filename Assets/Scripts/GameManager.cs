using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Endings")]
    public bool end1, end2, end3, end4;
    
    [Header("Easter Eggs")]
    public bool egg1, egg2;
    
    [Header("Reference")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("AudioClips")]
    [SerializeField] private AudioClip[] bgmClips;
    [SerializeField] private AudioClip[] sfxClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        end1 = end2 = end3 = end4 = false;
        egg1 = egg2 = false;
    }

    public static void PlayBgm(int index)
    {
        Instance.bgmSource.clip = Instance.bgmClips[index];
        Instance.bgmSource.Play();
    }
    
    public static void StopBgm()
    {
        Instance.bgmSource.Stop();
    }
    
    public static void SetBgmVolume(float volume)
    {
        Instance.bgmSource.volume = volume;
    }
    
    public static void PlaySfx(int index)
    {
        Instance.sfxSource.PlayOneShot(Instance.sfxClips[index]);
    }
}
