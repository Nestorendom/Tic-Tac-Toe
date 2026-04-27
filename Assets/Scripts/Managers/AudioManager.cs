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

    public bool BgmEnabled { get; private set; } = true;
    public bool SfxEnabled { get; private set; } = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettings();
    }

    private void Start()
    {
        ApplyBgmState();
        ApplySfxState();
        StartBgm();
    }

    public void SetBgmEnabled(bool enabled)
    {
        if (BgmEnabled == enabled)
            return;

        BgmEnabled = enabled;
        PlayerPrefs.SetInt(SaveKeys.BgmEnabled, enabled ? 1 : 0);
        PlayerPrefs.Save();
        ApplyBgmState();
    }

    public void SetSfxEnabled(bool enabled)
    {
        if (SfxEnabled == enabled)
            return;

        SfxEnabled = enabled;
        PlayerPrefs.SetInt(SaveKeys.SfxEnabled, enabled ? 1 : 0);
        PlayerPrefs.Save();
        ApplySfxState();
    }

    private void LoadSettings()
    {
        BgmEnabled = PlayerPrefs.GetInt(SaveKeys.BgmEnabled, 1) == 1;
        SfxEnabled = PlayerPrefs.GetInt(SaveKeys.SfxEnabled, 1) == 1;
    }

    private void ApplySfxState()
    {
        if (sfxSource == null)
            return;

        sfxSource.mute = !SfxEnabled;
    }

    private void ApplyBgmState()
    {
        if (bgmSource == null)
            return;

        bgmSource.mute = !BgmEnabled;

        if (BgmEnabled && !bgmSource.isPlaying)
        {
            StartBgm();
        }
    }

    private void StartBgm()
    {
        if (bgmSource == null || bgmClip == null)
            return;

        if (bgmSource.clip != bgmClip)
            bgmSource.clip = bgmClip;

        bgmSource.loop = true;

        if (!bgmSource.isPlaying)
            bgmSource.Play();
    }

    public void PlayButtonClick()
    {
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

    private void PlaySfx(AudioClip clip)
    {
        if (!SfxEnabled || sfxSource == null || clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }
}
