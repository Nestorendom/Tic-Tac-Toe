using UnityEngine;
using UnityEngine.UI;

public class BoardCell : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private Button button;
    [SerializeField] private Image markImage;

    public int Index => index;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (markImage == null)
            markImage = GetComponentInChildren<Image>();
    }

    public void SetIndex(int newIndex)
    {
        index = newIndex;
    }

    public void OnClick()
    {
        if (TicTacToeGameManager.Instance != null)
        {
            TicTacToeGameManager.Instance.TryPlaceMark(index);
        }
    }

    public void SetMark(Sprite sprite)
    {
        if (markImage == null)
            return;

        markImage.enabled = true;
        markImage.sprite = sprite;
    }

    public void Clear()
    {
        if (markImage != null)
        {
            markImage.sprite = null;
            markImage.enabled = false;
        }

        if (button != null)
        {
            button.interactable = true;
        }
    }

    public void Disable()
    {
        if (button != null)
        {
            button.interactable = false;
        }
    }
}