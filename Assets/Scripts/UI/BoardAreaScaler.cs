using UnityEngine;

[ExecuteAlways]
public class BoardAreaScaler : MonoBehaviour
{
    [SerializeField] private RectTransform boardPanel;
    [SerializeField, Range(0.6f, 0.95f)] private float portraitScale = 0.82f;
    [SerializeField, Range(0.5f, 0.9f)] private float landscapeScale = 0.65f;

    private Vector2Int lastScreenSize;

    private void OnEnable()
    {
        Refresh();
    }

    private void Update()
    {
        Vector2Int currentSize = new Vector2Int(Screen.width, Screen.height);
        if (currentSize != lastScreenSize)
        {
            Refresh();
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (boardPanel == null)
            return;

        lastScreenSize = new Vector2Int(Screen.width, Screen.height);

        bool isPortrait = Screen.height >= Screen.width;
        float targetScale = isPortrait ? portraitScale : landscapeScale;
        float size = Mathf.Min(Screen.width, Screen.height) * targetScale;

        boardPanel.anchorMin = new Vector2(0.5f, 0.5f);
        boardPanel.anchorMax = new Vector2(0.5f, 0.5f);
        boardPanel.pivot = new Vector2(0.5f, 0.5f);
        boardPanel.anchoredPosition = Vector2.zero;
        boardPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
        boardPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
    }
}
