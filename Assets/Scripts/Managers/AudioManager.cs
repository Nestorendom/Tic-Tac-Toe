using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private AudioClip placementClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip popupClip;

    private bool bgmEnabled = true;
    private bool sfxEnabled = true;
    public bool BgmEnabled => bgmEnabled;
    public bool SfxEnabled => sfxEnabled;

    private const string BGM_KEY = "BGM_ENABLED";
    private const string SFX_KEY = "SFX_ENABLED";

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load saved settings
        bgmEnabled = PlayerPrefs.GetInt(BGM_KEY, 1) == 1;
        sfxEnabled = PlayerPrefs.GetInt(SFX_KEY, 1) == 1;
    }

    private void Start()
    {
        ApplyBgmState();
    }



    public void StartBgm()
    {
        if (!bgmEnabled || bgmClip == null || bgmSource == null)
            return;

        if (bgmSource.isPlaying)
            return;

        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBgm()
    {
        if (bgmSource != null)
            bgmSource.Stop();
    }

    public void SetBgmEnabled(bool enabled)
    {
        bgmEnabled = enabled;
        PlayerPrefs.SetInt(BGM_KEY, enabled ? 1 : 0);

        ApplyBgmState();
    }

    private void ApplyBgmState()
    {
        if (bgmEnabled)
        {
            // Will start only after first interaction
        }
        else
        {
            StopBgm();
        }
    }


    public void SetSfxEnabled(bool enabled)
    {
        sfxEnabled = enabled;
        PlayerPrefs.SetInt(SFX_KEY, enabled ? 1 : 0);
    }

    private void PlaySfx(AudioClip clip)
    {
        if (!sfxEnabled || clip == null || sfxSource == null)
            return;

        // Small pitch variation = nicer feel
        sfxSource.pitch = Random.Range(0.95f, 1.05f);
        sfxSource.PlayOneShot(clip);
    }

    public void PlayButtonClick()
    {
        // ? WebGL fix: start BGM on first interaction
        if (bgmEnabled && bgmSource != null && !bgmSource.isPlaying)
        {
            StartBgm();
        }

        PlaySfx(buttonClickClip);
    }

    public void PlayPlacement()
    {
        PlaySfx(placementClip);
    }

    public void PlayWin()
    {
        PlaySfx(winClip);
    }

    public void PlayPopup()
    {
        PlaySfx(popupClip);
    }
}