using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopupSpriteAnimator : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Image targetImage;

    [Header("Animation Frames")]
    [SerializeField] private Sprite[] openFrames;
    [SerializeField] private Sprite[] closeFrames;

    [Header("Settings")]
    [SerializeField] private float frameRate = 24f;

    private Coroutine animationRoutine;

    public void PlayOpen()
    {
        Play(openFrames, false);
    }

    public void PlayClose(System.Action onComplete)
    {
        Play(closeFrames, true, onComplete);
    }

    private void Play(Sprite[] frames, bool hideAfter, System.Action onComplete = null)
    {
        if (animationRoutine != null)
            StopCoroutine(animationRoutine);

        animationRoutine = StartCoroutine(PlayRoutine(frames, hideAfter, onComplete));
    }

    private IEnumerator PlayRoutine(Sprite[] frames, bool hideAfter, System.Action onComplete)
    {
        if (targetImage == null || frames == null || frames.Length == 0)
        {
            onComplete?.Invoke();
            yield break;
        }

        float delay = 1f / frameRate;

        for (int i = 0; i < frames.Length; i++)
        {
            targetImage.sprite = frames[i];
            yield return new WaitForSecondsRealtime(delay);
        }

        if (hideAfter)
            gameObject.SetActive(false);

        onComplete?.Invoke();
    }
}