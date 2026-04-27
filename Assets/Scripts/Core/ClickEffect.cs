using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClickEffect : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float frameRate = 24f;

    private void Awake()
    {
        if (image == null)
            image = GetComponent<Image>();
    }

    public void Play(Vector2 screenPosition, Canvas canvas)
    {
        transform.SetParent(canvas.transform, false);

        RectTransform rect = GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPoint
        );

        rect.anchoredPosition = localPoint;

        gameObject.SetActive(true);
        StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        float delay = 1f / frameRate;

        for (int i = 0; i < frames.Length; i++)
        {
            image.sprite = frames[i];
            yield return new WaitForSecondsRealtime(delay);
        }

        Destroy(gameObject);
    }
}