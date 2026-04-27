using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Prevents decorative HUD graphics from intercepting board clicks.
/// Keeps button graphics raycastable.
/// </summary>
public class DisableNonInteractiveRaycasts : MonoBehaviour
{
    [SerializeField] private bool includeInactive = true;

    private void Awake()
    {
        Apply();
    }

    [ContextMenu("Apply Raycast Rules")]
    public void Apply()
    {
        Graphic[] graphics = GetComponentsInChildren<Graphic>(includeInactive);

        for (int i = 0; i < graphics.Length; i++)
            graphics[i].raycastTarget = false;

        Button[] buttons = GetComponentsInChildren<Button>(includeInactive);

        for (int i = 0; i < buttons.Length; i++)
        {
            Graphic targetGraphic = buttons[i].targetGraphic;
            if (targetGraphic != null)
                targetGraphic.raycastTarget = true;

            Graphic[] childGraphics = buttons[i].GetComponentsInChildren<Graphic>(includeInactive);
            for (int g = 0; g < childGraphics.Length; g++)
                childGraphics[g].raycastTarget = true;
        }
    }
}
