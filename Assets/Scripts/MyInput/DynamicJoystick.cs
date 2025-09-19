using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public class DynamicJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    private RectTransform joystickBackground;

    [SerializeField]
    private OnScreenStick onScreenStick;

    private Camera uiCamera;
    private Canvas canvas;

    private bool canRelease = false;
    private bool processingInput = true;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        joystickBackground.gameObject.SetActive(false);

        // Disable raycast target on joystick images to allow events to pass through
        joystickBackground.gameObject.GetComponent<Image>().raycastTarget = false;
        onScreenStick.gameObject.GetComponent<Image>().raycastTarget = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!processingInput) return;


        Debug.Log("DynamicJoystick OnPointerDown");
        joystickBackground.gameObject.SetActive(true);

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground.parent as RectTransform,
            eventData.position,
            uiCamera,
            out Vector2 localPoint))
        {
            joystickBackground.localPosition = localPoint;
        }

        // Forward the PointerDown event to the OnScreenStick
        onScreenStick.OnPointerDown(eventData);

        StartCoroutine(EnableRelease());
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!processingInput) return;

        // Debug.Log("DynamicJoystick OnDrag");
        // Forward the Drag event to the OnScreenStick
        onScreenStick.OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!processingInput) return;
        if (!canRelease) return;

        Debug.LogWarning("DynamicJoystick OnPointerUp");
        // Forward the PointerUp event to the OnScreenStick
        onScreenStick.OnPointerUp(eventData);

        // Hide the joystick
        joystickBackground.gameObject.SetActive(false);
    }

    private IEnumerator EnableRelease()
    {
        canRelease = false;
        yield return new WaitForEndOfFrame();
        canRelease = true;
    }

    public void OnLandedStateChanged(Lander.State state)
    {
        Debug.Log("DynamicJoystick OnLandedStateChanged: " + state);
        // Handle the landed state response
        processingInput = state != Lander.State.Landing;
        if (!processingInput)
        {
            // If not processing input, ensure joystick is hidden and reset
            joystickBackground.gameObject.SetActive(false);
            onScreenStick.OnPointerUp(new PointerEventData(EventSystem.current));

            // Disable raycast target, so it doesn't block other UI elements
            gameObject.GetComponent<Image>().raycastTarget = false;
        }
    }
}
