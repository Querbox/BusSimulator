using UnityEngine;

/// <summary>
/// Initialisiert und verwaltet die komplette Spiel-Szene
/// </summary>
public class SceneInitializer : MonoBehaviour
{
    [SerializeField] private Transform busSpawnPoint;
    [SerializeField] private RealisticRouteManager routeManager;
    [SerializeField] private OSMImporter osmImporter;

    private GameObject busInstance;
    private CameraController cameraController;

    private void Start()
    {
        InitializeScene();
    }

    private void InitializeScene()
    {
        Debug.Log("Initialisiere Spiel-Szene...");

        // 1. OSM-Daten generieren und importieren
        var osmData = OSMDataGenerator.GenerateHechingenRegion();
        if (osmImporter != null && osmData != null)
        {
            osmImporter.GenerateMapFromOSM(osmData);
        }

        // 2. Bus spawnen
        SpawnBus();

        // 3. Kamera einrichten
        SetupCamera();

        Debug.Log("Szene initialisiert!");
    }

    private void SpawnBus()
    {
        Vector3 spawnPos = busSpawnPoint != null ? busSpawnPoint.position : Vector3.zero;
        busInstance = new GameObject("Bus");
        busInstance.transform.position = spawnPos;

        // Rigidbody vor Komponenten konfigurieren, deren Awake darauf zugreift.
        Rigidbody rb = busInstance.AddComponent<Rigidbody>();
        rb.mass = 12000f;
        rb.linearDamping = 0.1f;

        busInstance.AddComponent<BusModel>();
        busInstance.AddComponent<BusPhysicsController>();

        Debug.Log($"Bus gespawnt bei {spawnPos}");
    }

    private void SetupCamera()
    {
        if (busInstance == null)
        {
            Debug.LogError("Bus konnte nicht vor Kamera-Setup erstellt werden!");
            return;
        }

        GameObject cameraObj = new GameObject("CameraController");
        cameraObj.transform.parent = busInstance.transform;
        cameraController = cameraObj.AddComponent<CameraController>();
        cameraController.SetTarget(busInstance.transform);

        Debug.Log("Kamera eingerichtet");
    }
}
