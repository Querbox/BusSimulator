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

        // Bus-Modell hinzufügen
        BusModel busModel = busInstance.AddComponent<BusModel>();
        
        // Bus-Physik hinzufügen
        BusPhysicsController physicsController = busInstance.AddComponent<BusPhysicsController>();
        
        // Rigidbody
        Rigidbody rb = busInstance.AddComponent<Rigidbody>();
        rb.mass = 12000f;
        rb.drag = 0.1f;

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
        cameraController.GetComponent<CameraController>().busTransform = busInstance.transform;

        Debug.Log("Kamera eingerichtet");
    }
}
