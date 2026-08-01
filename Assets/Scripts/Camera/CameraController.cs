using UnityEngine;

/// <summary>
/// Flexible Kamera-System mit mehreren Modi
/// </summary>
public class CameraController : MonoBehaviour
{
    [System.Serializable]
    public enum CameraMode
    {
        FirstPerson,      // Fahrer-Sicht
        ThirdPerson,      // Verfolgungskamera hinter dem Bus
        Orbiting,         // Umlaufbahn um den Bus
        Cinematic         // Automatische Kamera-Bewegungen
    }

    [SerializeField] private CameraMode currentMode = CameraMode.FirstPerson;
    [SerializeField] private Transform busTransform;
    [SerializeField] private Camera mainCamera;
    
    [Header("First Person Settings")]
    [SerializeField] private Vector3 firstPersonOffset = new Vector3(0, 2.5f, 3f); // Fahrer-Position
    [SerializeField] private float mouseSensitivity = 2f;
    
    [Header("Third Person Settings")]
    [SerializeField] private Vector3 thirdPersonOffset = new Vector3(0, 3f, -8f);
    [SerializeField] private float thirdPersonDistance = 8f;
    [SerializeField] private float thirdPersonHeight = 3f;
    [SerializeField] private float cameraLookAhead = 5f; // Kamera schaut voraus
    
    [Header("Orbiting Settings")]
    [SerializeField] private float orbitDistance = 10f;
    [SerializeField] private float orbitHeight = 5f;
    [SerializeField] private float orbitSpeed = 30f;
    
    private float mouseX = 0f;
    private float mouseY = 0f;
    private float orbitAngle = 0f;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        if (busTransform == null)
            busTransform = transform.parent;
    }

    private void LateUpdate()
    {
        if (busTransform == null) return;

        switch (currentMode)
        {
            case CameraMode.FirstPerson:
                UpdateFirstPersonCamera();
                break;
            case CameraMode.ThirdPerson:
                UpdateThirdPersonCamera();
                break;
            case CameraMode.Orbiting:
                UpdateOrbitingCamera();
                break;
            case CameraMode.Cinematic:
                UpdateCinematicCamera();
                break;
        }

        // Kamera-Modus wechseln mit Tasten
        HandleCameraModeInput();
    }

    private void UpdateFirstPersonCamera()
    {
        // Kamera im Bus (Fahrer-Perspektive)
        Vector3 targetPos = busTransform.position + busTransform.TransformDirection(firstPersonOffset);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, Time.deltaTime * 5f);

        // Kamera schaut in Fahrtrichtung
        mainCamera.transform.LookAt(busTransform.position + busTransform.forward * 20f + Vector3.up * 1f);

        // Optionale Mouse-Look für freie Kamerarotation
        if (InputSystemControls.IsAltPressed())
        {
            Vector2 mouseDelta = InputSystemControls.GetMouseDelta();
            mouseX += mouseDelta.x * mouseSensitivity;
            mouseY -= mouseDelta.y * mouseSensitivity;
            mouseY = Mathf.Clamp(mouseY, -30f, 30f);

            mainCamera.transform.RotateAround(busTransform.position, Vector3.up, mouseX * Time.deltaTime);
            mainCamera.transform.RotateAround(mainCamera.transform.right, Vector3.right, mouseY * Time.deltaTime);
        }
    }

    private void UpdateThirdPersonCamera()
    {
        // Kamera folgt dem Bus von hinten/oben
        Vector3 targetPos = busTransform.position - busTransform.forward * thirdPersonDistance + Vector3.up * thirdPersonHeight;
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, Time.deltaTime * 3f);

        // Schau auf einen Punkt vor dem Bus
        Vector3 lookTarget = busTransform.position + busTransform.forward * cameraLookAhead + Vector3.up * 1f;
        mainCamera.transform.LookAt(lookTarget);
    }

    private void UpdateOrbitingCamera()
    {
        // Kamera umkreist den Bus
        orbitAngle += orbitSpeed * Time.deltaTime;
        if (orbitAngle > 360f) orbitAngle -= 360f;

        float radians = orbitAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(
            Mathf.Cos(radians) * orbitDistance,
            orbitHeight,
            Mathf.Sin(radians) * orbitDistance
        );

        mainCamera.transform.position = busTransform.position + offset;
        mainCamera.transform.LookAt(busTransform.position + Vector3.up * 1.5f);
    }

    private void UpdateCinematicCamera()
    {
        // Dynamische Kamera die zwischen verschiedenen Positionen wechselt
        float cycleTime = 10f;
        float t = (Time.time % cycleTime) / cycleTime;

        Vector3 position;
        if (t < 0.5f)
        {
            // Von vorne nach hinten
            float localT = t * 2f;
            position = Vector3.Lerp(
                busTransform.position + busTransform.forward * 5f + Vector3.up * 3f,
                busTransform.position - busTransform.forward * 10f + Vector3.up * 4f,
                localT
            );
        }
        else
        {
            // Von oben
            float localT = (t - 0.5f) * 2f;
            position = Vector3.Lerp(
                busTransform.position + Vector3.up * 8f,
                busTransform.position + busTransform.right * 8f + Vector3.up * 5f,
                localT
            );
        }

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, position, Time.deltaTime * 2f);
        mainCamera.transform.LookAt(busTransform.position + Vector3.up * 2f);
    }

    private void HandleCameraModeInput()
    {
        if (InputSystemControls.WasCameraCyclePressed())
        {
            int modeCount = System.Enum.GetValues(typeof(CameraMode)).Length;
            currentMode = (CameraMode)(((int)currentMode + 1) % modeCount);
            Debug.Log($"Kamera-Modus: {currentMode}");
        }

        if (InputSystemControls.WasNumberPressed(1)) currentMode = CameraMode.FirstPerson;
        if (InputSystemControls.WasNumberPressed(2)) currentMode = CameraMode.ThirdPerson;
        if (InputSystemControls.WasNumberPressed(3)) currentMode = CameraMode.Orbiting;
        if (InputSystemControls.WasNumberPressed(4)) currentMode = CameraMode.Cinematic;
    }


    public void SetTarget(Transform target)
    {
        busTransform = target;
    }

    public void SetCameraMode(CameraMode mode)
    {
        currentMode = mode;
        Debug.Log($"Kamera-Modus gewechselt zu: {mode}");
    }
}
