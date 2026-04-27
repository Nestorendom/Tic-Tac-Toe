using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(GridLayoutGroup))]
public class BoardGridAutoSizer : MonoBehaviour
{
    [SerializeField] private RectTransform boardPanel;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private Vector2 spacing = new Vector2(10f, 10f);

    private Vector2Int lastScreenSize;

    private void Reset()
    {
        boardPanel = transform as RectTransform;
        gridLayoutGroup = GetComponent<GridLayoutGroup>();

        if (gridLayoutGroup != null)
        {
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayoutGroup.constraintCount = 3;
        }
    }

    private void Awake()
    {
        if (boardPanel == null)
            boardPanel = transform as RectTransform;

        if (gridLayoutGroup == null)
            gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

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

    private void OnValidate()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (boardPanel == null || gridLayoutGroup == null)
            return;

        lastScreenSize = new Vector2Int(Screen.width, Screen.height);

        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = 3;
        gridLayoutGroup.spacing = spacing;

        Rect rect = boardPanel.rect;
        float width = rect.width;

        float totalSpacingX = spacing.x * 2f;
        float cellSize = (width - totalSpacingX) / 3f;
        cellSize = Mathf.Max(1f, cellSize);

        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
    }
}
