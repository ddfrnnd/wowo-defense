using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using InfimaGames.LowPolyShooterPack;

/// <summary>
/// Handles mobile touch controls: MoveStick, LookArea, FireButton, FireStick, ReloadButton.
/// Attach to UI elements in the GameplayCanvas prefab.
/// All layout is done in the prefab - this script only handles input logic.
/// </summary>
public class MobileControlsUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public enum ControlType
    {
        MoveStick,
        LookArea,
        FireButton,
        ReloadButton,
        FireStick,    // Draggable fire button (analog-style but fires while held)
        RunButton,     // Mobile Sprint/Run Button
        AimButton     // Mobile Aim/Scope Button
    }

    [SerializeField] private ControlType controlType;
    [SerializeField] private CharacterBehaviour playerCharacter;
    [SerializeField] private RectTransform stickHandle;
    [SerializeField] private float stickRange = 75f;
    [SerializeField] private float lookSensitivity = 0.08f;

    private RectTransform rectTransform;
    private Camera eventCamera;
    private bool isDragging = false;
    private bool isFireHeld = false;

    private void Awake()
    {
        rectTransform = transform as RectTransform;
        var canvas = GetComponentInParent<Canvas>();
        eventCamera = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null;

        RefreshPlayerCharacter();
    }

    private void Update()
    {
        // Auto-fire while holding the fire button or fire stick
        if ((controlType == ControlType.FireButton || controlType == ControlType.FireStick) && isFireHeld)
        {
            RefreshPlayerCharacter();
            playerCharacter?.MobileFire();
        }

        if (controlType == ControlType.MoveStick && stickHandle != null && !isDragging)
        {
            RefreshPlayerCharacter();
            if (playerCharacter != null)
            {
                Vector2 moveInput = playerCharacter.GetInputMovement();
                stickHandle.anchoredPosition = moveInput * stickRange;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RefreshPlayerCharacter();

        if (controlType == ControlType.FireButton)
        {
            isFireHeld = true;
            playerCharacter?.MobileFire();
            return;
        }

        if (controlType == ControlType.FireStick)
        {
            isFireHeld = true;
            isDragging = true;
            playerCharacter?.MobileFire();
            OnDrag(eventData); // Move handle to touch position
            return;
        }

        if (controlType == ControlType.ReloadButton)
        {
            playerCharacter?.MobileReload();
            return;
        }

        if (controlType == ControlType.RunButton)
        {
            playerCharacter?.SetMobileRun(true);
            return;
        }

        if (controlType == ControlType.AimButton)
        {
            playerCharacter?.SetMobileAim(true);
            return;
        }

        isDragging = true;
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        RefreshPlayerCharacter();
        if (playerCharacter == null)
            return;

        isDragging = true;

        if (controlType == ControlType.MoveStick)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventCamera, out var localPoint))
                return;

            Vector2 offset = localPoint - rectTransform.rect.center;
            var clamped = Vector2.ClampMagnitude(offset, stickRange);
            if (stickHandle != null)
                stickHandle.anchoredPosition = clamped;

            playerCharacter.SetMobileMovement(clamped / stickRange);
        }
        else if (controlType == ControlType.FireStick)
        {
            // Handle follows finger within range (visual feedback while firing)
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventCamera, out var localPoint))
                return;

            Vector2 offset = localPoint - rectTransform.rect.center;
            var clamped = Vector2.ClampMagnitude(offset, stickRange);
            if (stickHandle != null)
                stickHandle.anchoredPosition = clamped;

            // Keep firing while dragging
            isFireHeld = true;

            // Rotate camera simultaneously
            playerCharacter.SetMobileLook(eventData.delta * lookSensitivity);
        }
        else if (controlType == ControlType.LookArea)
        {
            playerCharacter.SetMobileLook(eventData.delta * lookSensitivity);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        if (controlType == ControlType.FireButton)
        {
            isFireHeld = false;
            return;
        }

        if (controlType == ControlType.FireStick)
        {
            isFireHeld = false;
            // Snap handle back to center
            if (stickHandle != null)
                stickHandle.anchoredPosition = Vector2.zero;
            playerCharacter?.SetMobileLook(Vector2.zero);
            return;
        }

        if (controlType == ControlType.RunButton)
        {
            playerCharacter?.SetMobileRun(false);
            return;
        }

        if (controlType == ControlType.AimButton)
        {
            playerCharacter?.SetMobileAim(false);
            return;
        }

        RefreshPlayerCharacter();
        if (playerCharacter == null)
            return;

        if (controlType == ControlType.MoveStick)
        {
            if (stickHandle != null)
                stickHandle.anchoredPosition = Vector2.zero;

            playerCharacter.SetMobileMovement(Vector2.zero);
        }
        else if (controlType == ControlType.LookArea)
        {
            playerCharacter.SetMobileLook(Vector2.zero);
        }
    }

    private void RefreshPlayerCharacter()
    {
        if (playerCharacter != null && playerCharacter.gameObject.activeInHierarchy)
            return;

        playerCharacter = FindFirstObjectByType<CharacterBehaviour>();
    }
}
