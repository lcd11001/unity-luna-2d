using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private MyInputActions inputActions;

    public event EventHandler OnMenuPaused;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        inputActions = new MyInputActions();
        inputActions.Enable();

        inputActions.Menu.Pause.performed += OnMenuPause_performed;

        DontDestroyOnLoad(this);
    }

    private void OnDestroy()
    {
        if (inputActions != null)
        {
            inputActions.Disable();
        }
    }

    public bool IsUpActionPressed()
    {
        return inputActions.Player.LanderUp.IsPressed();
    }

    public bool IsLeftActionPressed()
    {
        return inputActions.Player.LanderLeft.IsPressed();
    }

    public bool IsRightActionPressed()
    {
        return inputActions.Player.LanderRight.IsPressed();
    }

    public Vector2 GetMovementVector()
    {
        return inputActions.Player.Movement.ReadValue<Vector2>();
    }

    public bool IsMenuConfirmPressed()
    {
        return inputActions.Menu.Confirm.IsPressed();
    }

    private void OnMenuPause_performed(InputAction.CallbackContext context)
    {
        OnMenuPaused?.Invoke(this, EventArgs.Empty);
    }
}
