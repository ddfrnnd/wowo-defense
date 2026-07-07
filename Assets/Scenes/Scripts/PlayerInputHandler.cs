using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string rotateObject = "RotateObject";

    private InputAction movementAction;
    private InputAction rotationAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction rotateObjectAction;

    public Vector2 MovementInput
    {
        get
        {
            Vector2 kbInput = (movementAction != null && movementAction.enabled) ? movementAction.ReadValue<Vector2>() : Vector2.zero;
            return kbInput + mobileMovementInput;
        }
    }

    public Vector2 RotationInput
    {
        get
        {
            Vector2 kbInput = (rotationAction != null && rotationAction.enabled) ? rotationAction.ReadValue<Vector2>() : Vector2.zero;
            return kbInput + mobileRotationInput;
        }
    }

    public Vector2 KeyboardMovementInput => (movementAction != null && movementAction.enabled) ? movementAction.ReadValue<Vector2>() : Vector2.zero;

    public bool JumpTriggered { get; private set; }
    public bool SprintTriggered { get; private set; }
    public bool RotateObjectTriggered { get; private set; }

    private Vector2 mobileMovementInput;
    private Vector2 mobileRotationInput;

    private void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(rotation);
        jumpAction = mapReference.FindAction(jump);
        sprintAction = mapReference.FindAction(sprint);
        rotateObjectAction = mapReference.FindAction(rotateObject);
        SubscribeActionValuesToInputEvents();
    }

    private void SubscribeActionValuesToInputEvents()
    {
        jumpAction.performed += inputInfo => JumpTriggered = true;
        jumpAction.canceled += inputInfo => JumpTriggered = false;

        rotateObjectAction.performed += _ => RotateObjectTriggered = true;
        rotateObjectAction.canceled += _ => RotateObjectTriggered = false;

        sprintAction.performed += inputInfo => SprintTriggered = true;
        sprintAction.canceled += inputInfo => SprintTriggered = false;
    }

    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
    }

    private void OnDisable()
    {
        playerControls.FindActionMap(actionMapName).Disable();
    }

    public void SetMobileMovement(Vector2 value)
    {
        mobileMovementInput = Vector2.ClampMagnitude(value, 1f);
    }

    public void SetMobileRotation(Vector2 value)
    {
        mobileRotationInput = value;
    }
}
