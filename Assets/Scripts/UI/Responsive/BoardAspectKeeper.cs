using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Keeps the board square while fitting it inside its parent with orientation-specific occupancy.
/// Works with AspectRatioFitter + LayoutElement best-practice setup.
/// </summary>
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(LayoutElement))]
public class BoardAspectKeeper : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField, Range(0.4f, 1f)] private float portraitOccupancy = 0.9f;
    [SerializeField, Range(0.4f, 1f)] private float landscapeOccupancy = 0.75f;
    [SerializeField] private float extraPadding = 0f;

    private RectTransform rectTransform;
    private LayoutElement layoutElement;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        layoutElement = GetComponent<LayoutElement>();

        if (container == null)
            container = rectTransform.parent as RectTransform;

        ApplySize();
    }

    private void OnEnable()
    {
        ApplySize();
    }

    private void Update()
    {
        ApplySize();
    }

    private void OnRectTransformDimensionsChange()
    {
        ApplySize();
    }

    private void ApplySize()
    {
        if (container == null)
            return;

        bool isLandscape = Screen.width >= Screen.height;
        float occupancy = isLandscape ? landscapeOccupancy : portraitOccupancy;

        Rect area = container.rect;
        float maxWidth = area.width * occupancy - extraPadding;
        float maxHeight = area.height * occupancy - extraPadding;
        float side = Mathf.Max(0f, Mathf.Min(maxWidth, maxHeight));

        layoutElement.preferredWidth = side;
        layoutElement.preferredHeight = side;
    }
}
