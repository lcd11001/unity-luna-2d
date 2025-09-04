using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraZoom2D : MonoBehaviour
{
    [SerializeField]
    private float zoomSpeed = 1f;

    [SerializeField]
    private float minZoom = 5f;

    [SerializeField]
    private float maxZoom = 20f;
    [SerializeField]
    private float targetZoom = 10f;

    [SerializeField]
    private CinemachineCamera virtualCamera;
    private CinemachinePositionComposer positionComposer;

    public static CinemachineCameraZoom2D Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        if (virtualCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera component not found!");
        }
        targetZoom = virtualCamera.Lens.OrthographicSize;
        positionComposer = virtualCamera.GetComponent<CinemachinePositionComposer>();
    }

    private void Update()
    {
        // HandleZoomInput();
        SmoothZoom();
    }

    private void HandleZoomInput()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            targetZoom -= scrollInput * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }
    }

    private void SmoothZoom()
    {
        if (virtualCamera != null)
        {
            virtualCamera.Lens.OrthographicSize = Mathf.Lerp(virtualCamera.Lens.OrthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
        }
    }

    public void SetTargetZoom(float newZoom)
    {
        targetZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);
    }

    public void SetVitualCamera(CinemachineCamera newCamera)
    {
        virtualCamera = newCamera;
        positionComposer = virtualCamera.GetComponent<CinemachinePositionComposer>();
    }

    public void SetDeadZoneSize(float x, float y)
    {
        if (positionComposer != null)
        {
            positionComposer.Composition.DeadZone.Size = new Vector2(x, y);
        }
    }
}
