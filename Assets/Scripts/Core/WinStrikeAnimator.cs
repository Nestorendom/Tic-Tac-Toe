using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WinStrikeAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform boardRect;
    [SerializeField] private RectTransform strikeLine;
    [SerializeField] private Image strikeImage;

    [Header("Settings")]
    [SerializeField] private float thickness = 18f;
    [SerializeField] private float extraLength = 80f;
    [SerializeField] private float animationDuration = 0.3f;

    private Coroutine animationRoutine;

    public void Hide()
    {
        if (animationRoutine != null)
            StopCoroutine(animationRoutine);

        strikeLine.gameObject.SetActive(false);
    }

    public void Play(BoardCell startCell, BoardCell endCell)
    {
        if (startCell == null || endCell == null || boardRect == null || strikeLine == null)
            return;

        if (animationRoutine != null)
            StopCoroutine(animationRoutine);

        RectTransform startRect = startCell.GetComponent<RectTransform>();
        RectTransform endRect = endCell.GetComponent<RectTransform>();

        Vector2 startPos = GetLocalPointOnBoard(startRect);
        Vector2 endPos = GetLocalPointOnBoard(endRect);

        Vector2 center = (startPos + endPos) / 2f;
        Vector2 direction = endPos - startPos;

        float length = direction.magnitude + extraLength;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        strikeLine.gameObject.SetActive(true);
        strikeLine.SetParent(boardRect, false);
        strikeLine.anchorMin = new Vector2(0.5f, 0.5f);
        strikeLine.anchorMax = new Vector2(0.5f, 0.5f);
        strikeLine.pivot = new Vector2(0.5f, 0.5f);

        strikeLine.anchoredPosition = center;
        strikeLine.localRotation = Quaternion.Euler(0f, 0f, angle);

        animationRoutine = StartCoroutine(Animate(length));
    }

    private Vector2 GetLocalPointOnBoard(RectTransform target)
    {
        Vector3 worldCenter = target.TransformPoint(target.rect.center);
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, worldCenter);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            boardRect,
            screenPoint,
            null,
            out Vector2 localPoint
        );

        return localPoint;
    }

    private IEnumerator Animate(float targetLength)
    {
        float timer = 0f;
        strikeLine.sizeDelta = new Vector2(0f, thickness);

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;
            float t = timer / animationDuration;

            strikeLine.sizeDelta = new Vector2(
                Mathf.Lerp(0f, targetLength, t),
                thickness
            );

            yield return null;
        }

        strikeLine.sizeDelta = new Vector2(targetLength, thickness);
    }
}