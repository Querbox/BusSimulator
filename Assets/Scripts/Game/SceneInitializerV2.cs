using UnityEngine;

/// <summary>
/// Erweiterte Szenen-Initialisierung mit Depot-Support
/// </summary>
public class SceneInitializerV2 : MonoBehaviour
{
    [SerializeField] private RealisticRouteManager routeManager;
    [SerializeField] private OSMImporter osmImporter;
    [SerializeField] private BusPhysicsController playerBusController;
    [SerializeField] private CameraController cameraController;

    private GameObject busInstance;
    private BusDepot depot;

    private void Start()
    {
        InitializeScene();
    }

    private void InitializeScene()
    {
        Debug.Log("=== Initialisiere Spiel-Szene mit Depot ===");

        // 1. OSM-Daten generieren und importieren
        var osmData = OSMDataGenerator.GenerateHechingenRegion();
        if (osmImporter != null && osmData != null)
        {
            osmImporter.GenerateMapFromOSM(osmData);
            Debug.Log("✓ Karte geladen");
        }

        // 2. Bushaltestelle-Depot erstellen
        CreateDepot();

        // 3. Spieler-Bus in Depot spawnen
        SpawnBusInDepot();

        // 4. Kamera einrichten
        SetupCamera();

        // 5. Depot-Manager starten
        SetupDepotManager();

        Debug.Log("✓ Szene vollständig initialisiert!");
    }

    private void CreateDepot()
    {
        GameObject depotObj = new GameObject("BusDepot");
        depot = depotObj.AddComponent<BusDepot>();
        
        // Depot an echten Koordinaten positionieren
        // HVB Hechingen: Brunnenstraße 11, 72379 Hechingen
        // Lat: 48.37915, Lon: 8.75095
        depotObj.transform.position = new Vector3(0, 0, 0); // Lokales Koordinaten-System
        
        Debug.Log("✓ Bus-Depot erstellt (HVB Hechingen - Brunnenstraße 11)");
    }

    private void SpawnBusInDepot()
    {
        // Bus wird vom Depot-Manager an Parkplatz 0 positioniert
        busInstance = new GameObject("PlayerBus");
        busInstance.transform.position = new Vector3(-20f, 0, 0); // Wird vom Depot-Manager angepasst

        // Bus-Komponenten
        BusModel busModel = busInstance.AddComponent<BusModel>();
        BusPhysicsController physicsController = busInstance.AddComponent<BusPhysicsController>();
        
        // Rigidbody
        Rigidbody rb = busInstance.AddComponent<Rigidbody>();
        rb.mass = 12000f;
        rb.drag = 0.1f;
        rb.angularDrag = 0.1f;

        playerBusController = physicsController;
        Debug.Log("✓ Bus im Depot geparkt");
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

        Debug.Log("✓ Kamera eingerichtet");
    }

    private void SetupDepotManager()
    {
        GameObject depotManagerObj = new GameObject("DepotManager");
        DepotManager depotManager = depotManagerObj.AddComponent<DepotManager>();
        
        // Referenzen setzen
        depotManager.GetComponent<DepotManager>().enabled = true;
        
        Debug.Log("✓ Depot-Manager aktiv - Wähle einen Bus!");
    }
}
