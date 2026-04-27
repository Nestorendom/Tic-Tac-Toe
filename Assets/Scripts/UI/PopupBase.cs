using UnityEngine;

public class PopupBase : MonoBehaviour
{
    [SerializeField] protected GameObject root;
    [SerializeField] private PopupSpriteAnimator popupAnimator;

    public virtual void Show()
    {
        root.SetActive(true);

        AudioManager.Instance?.PlayPopup();

        if (popupAnimator != null)
            popupAnimator.PlayOpen();
    }

    public virtual void Hide()
    {
        if (popupAnimator != null)
        {
            popupAnimator.PlayClose(() =>
            {
                root.SetActive(false);
            });
        }
        else
        {
            root.SetActive(false);
        }
    }
}