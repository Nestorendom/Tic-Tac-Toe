using UnityEngine;
using UnityEngine.InputSystem;

public class ClickEffectSpawner : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private ClickEffect clickEffectPrefab;

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Spawn(Mouse.current.position.ReadValue());
        }

        if (Touchscreen.current != null)
        {
            foreach (var touch in Touchscreen.current.touches)
            {
                if (touch.press.wasPressedThisFrame)
                {
                    Spawn(touch.position.ReadValue());
                    break;
                }
            }
        }
    }

    private void Spawn(Vector2 screenPosition)
    {
        if (canvas == null || clickEffectPrefab == null)
            return;

        ClickEffect effect = Instantiate(clickEffectPrefab);
        effect.Play(screenPosition, canvas);
    }
}