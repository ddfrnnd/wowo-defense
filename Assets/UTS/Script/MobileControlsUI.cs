using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobileControlsUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public enum ControlType
    {
        MoveStick,
        LookArea,
        FireButton,
        ReloadButton
    }

    [SerializeField] private ControlType controlType;
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private GunController gunController;
    [SerializeField] private RectTransform stickHandle;
    [SerializeField] private float stickRange = 75f;
    [SerializeField] private float lookSensitivity = 0.08f;

    private RectTransform rectTransform;
    private Camera eventCamera;
    private bool isDragging = false;

    private void Awake()
    {
        rectTransform = transform as RectTransform;
        var canvas = GetComponentInParent<Canvas>();
        eventCamera = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null;

        if (inputHandler == null)
            inputHandler = FindFirstObjectByType<PlayerInputHandler>();

        RefreshGunController();
    }

    private void Update()
    {
        if (controlType == ControlType.MoveStick && stickHandle != null && !isDragging)
        {
            if (inputHandler != null)
            {
                Vector2 kbInput = inputHandler.KeyboardMovementInput;
                stickHandle.anchoredPosition = kbInput * stickRange;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (controlType == ControlType.FireButton)
        {
            RefreshGunController();
            gunController?.MobileFire();
            return;
        }

        if (controlType == ControlType.ReloadButton)
        {
            RefreshGunController();
            gunController?.MobileReload();
            return;
        }

        isDragging = true;
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (inputHandler == null)
            return;

        isDragging = true;

        if (controlType == ControlType.MoveStick)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventCamera, out var localPoint))
                return;

            var clamped = Vector2.ClampMagnitude(localPoint, stickRange);
            if (stickHandle != null)
                stickHandle.anchoredPosition = clamped;

            inputHandler.SetMobileMovement(clamped / stickRange);
        }
        else if (controlType == ControlType.LookArea)
        {
            inputHandler.SetMobileRotation(eventData.delta * lookSensitivity);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        if (inputHandler == null)
            return;

        if (controlType == ControlType.MoveStick)
        {
            if (stickHandle != null)
                stickHandle.anchoredPosition = Vector2.zero;

            inputHandler.SetMobileMovement(Vector2.zero);
        }
        else if (controlType == ControlType.LookArea)
        {
            inputHandler.SetMobileRotation(Vector2.zero);
        }
    }

    private void RefreshGunController()
    {
        if (gunController != null && gunController.gameObject.activeInHierarchy)
            return;

        var guns = FindObjectsByType<GunController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        gunController = guns.Length > 0 ? guns[0] : null;
    }
}
