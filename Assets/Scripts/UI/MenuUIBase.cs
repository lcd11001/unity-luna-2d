using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class MenuUIBase : MonoBehaviour
{
    private List<Button> buttons;
    private int selectedIndex = 0;
    private int totalButtons = 0;

    protected void InitializeButtons(params Button[] buttons)
    {
        this.buttons = new List<Button>(buttons);
    }

    protected virtual void Start()
    {
        totalButtons = buttons.Count;
        selectedIndex = 0;
        UpdateButtonSelection();

        if (GameInput.Instance != null)
        {
            GameInput.Instance.OnMenuUp += GameInput_OnMenuUp;
            GameInput.Instance.OnMenuDown += GameInput_OnMenuDown;

            GameInput.Instance.OnMenuConfirmed += GameInput_OnMenuConfirmed;
        }
    }

    protected virtual void Update()
    {
        // To prevent the UI buttons from losing focus when you click on the background
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            UpdateButtonSelection();
        }
    }

    protected virtual void OnDestroy()
    {
        this.buttons.Clear();

        if (GameInput.Instance != null)
        {
            GameInput.Instance.OnMenuUp -= GameInput_OnMenuUp;
            GameInput.Instance.OnMenuDown -= GameInput_OnMenuDown;

            GameInput.Instance.OnMenuConfirmed -= GameInput_OnMenuConfirmed;
        }
    }

    private void GameInput_OnMenuUp(object sender, System.EventArgs e)
    {
        selectedIndex = (selectedIndex - 1 + totalButtons) % totalButtons; // Wrap around for 2 buttons
        // Debug.Log("Menu Up pressed, selectedIndex: " + selectedIndex);
        UpdateButtonSelection();
    }

    private void GameInput_OnMenuDown(object sender, System.EventArgs e)
    {
        selectedIndex = (selectedIndex + 1) % totalButtons; // Wrap around for 2 buttons
        // Debug.Log("Menu Down pressed, selectedIndex: " + selectedIndex);
        UpdateButtonSelection();
    }

    private void GameInput_OnMenuConfirmed(object sender, System.EventArgs e)
    {
        if (buttons != null && buttons.Count > 0)
        {
            buttons[selectedIndex].onClick.Invoke();
        }
    }

    private void UpdateButtonSelection()
    {
        if (buttons != null && buttons.Count > 0)
        {
            buttons[selectedIndex].Select();
        }
    }
}
