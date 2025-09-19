using Unity.VisualScripting;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private MyInputActions inputActions;

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

        DontDestroyOnLoad(this);
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
}
