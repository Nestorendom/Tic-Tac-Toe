using UnityEngine;

/// <summary>
/// Enables one layout root for portrait and another for landscape.
/// Keep gameplay/scripts under both roots as shared references when possible
/// so existing button OnClick bindings remain unchanged.
/// </summary>
public class OrientationLayoutSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject portraitLayoutRoot;
    [SerializeField] private GameObject landscapeLayoutRoot;
    [SerializeField] private bool useDeviceOrientationOnMobile = true;

    private bool initialized;
    private bool isLandscape;

    private void OnEnable()
    {
        RefreshLayout(force: true);
    }

    private void Update()
    {
        RefreshLayout(force: false);
    }

    public void RefreshLayout(bool force)
    {
        bool targetLandscape = GetIsLandscape();

        if (!force && initialized && targetLandscape == isLandscape)
            return;

        initialized = true;
        isLandscape = targetLandscape;

        if (portraitLayoutRoot != null)
            portraitLayoutRoot.SetActive(!isLandscape);

        if (landscapeLayoutRoot != null)
            landscapeLayoutRoot.SetActive(isLandscape);
    }

    private bool GetIsLandscape()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (useDeviceOrientationOnMobile)
        {
            DeviceOrientation orientation = Input.deviceOrientation;

            if (orientation == DeviceOrientation.LandscapeLeft || orientation == DeviceOrientation.LandscapeRight)
                return true;

            if (orientation == DeviceOrientation.Portrait || orientation == DeviceOrientation.PortraitUpsideDown)
                return false;
        }
#endif
        return Screen.width >= Screen.height;
    }
}
