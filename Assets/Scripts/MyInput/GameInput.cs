using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private MyInputActions inputActions;

    public event EventHandler OnMenuPaused;
    public event EventHandler OnMenuConfirmed;
    public event EventHandler OnMenuUp;
    public event EventHandler OnMenuDown;

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
        inputActions.Menu.Confirm.performed += OnMenuConfirm_performed;
        inputActions.Menu.Up.performed += OnMenuUp_performed;
        inputActions.Menu.Down.performed += OnMenuDown_performed;

        DontDestroyOnLoad(this);
    }

    private void OnDestroy()
    {
        if (inputActions != null)
        {
            inputActions.Menu.Pause.performed -= OnMenuPause_performed;
            inputActions.Menu.Confirm.performed -= OnMenuConfirm_performed;
            inputActions.Menu.Up.performed -= OnMenuUp_performed;
            inputActions.Menu.Down.performed -= OnMenuDown_performed;

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

    private void OnMenuPause_performed(InputAction.CallbackContext context)
    {
        OnMenuPaused?.Invoke(this, EventArgs.Empty);
    }

    private void OnMenuConfirm_performed(InputAction.CallbackContext context)
    {
        OnMenuConfirmed?.Invoke(this, EventArgs.Empty);
    }

    private void OnMenuUp_performed(InputAction.CallbackContext context)
    {
        OnMenuUp?.Invoke(this, EventArgs.Empty);
    }

    private void OnMenuDown_performed(InputAction.CallbackContext context)
    {
        OnMenuDown?.Invoke(this, EventArgs.Empty);
    }
}
