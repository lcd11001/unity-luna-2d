using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonQuit;

    private int selectedIndex = 0;
    private const int totalButtons = 2;

    private void Awake()
    {
        buttonPlay.onClick.AddListener(OnPlayClicked);
        buttonQuit.onClick.AddListener(OnQuitClicked);
    }

    private void Start()
    {
        selectedIndex = 0;
        UpdateButtonSelection();

        if (GameInput.Instance != null)
        {
            GameInput.Instance.OnMenuUp += GameInput_OnMenuUp;
            GameInput.Instance.OnMenuDown += GameInput_OnMenuDown;
        }
    }

    private void Update()
    {
        // To prevent the UI buttons from losing focus when you click on the background
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            UpdateButtonSelection();
        }
    }

    private void OnDestroy()
    {
        if (GameInput.Instance != null)
        {
            GameInput.Instance.OnMenuUp -= GameInput_OnMenuUp;
            GameInput.Instance.OnMenuDown -= GameInput_OnMenuDown;
        }
    }

    private void OnPlayClicked()
    {
        Debug.Log("Play button clicked");
        SceneLoader.Instance.LoadGameScene();
    }

    private void OnQuitClicked()
    {
        Debug.Log("Quit button clicked");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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

    private void UpdateButtonSelection()
    {
        if (selectedIndex == 0)
        {
            buttonPlay.Select();
        }
        else if (selectedIndex == 1)
        {
            buttonQuit.Select();
        }
    }
}
