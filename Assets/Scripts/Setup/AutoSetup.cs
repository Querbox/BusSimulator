using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Auto-Setup Script - Richtet das komplette Spiel automatisch ein
/// Einfach an ein leeres GameObject in der Scene hängen und Play drücken!
/// </summary>
public class AutoSetup : MonoBehaviour
{
    private void Awake()
    {
        SetupGame();
    }

    private void SetupGame()
    {
        Debug.Log("\n" + new string('=', 50));
        Debug.Log("   BUS SIMULATOR - AUTO SETUP");
        Debug.Log(new string('=', 50) + "\n");

        // 1. Kamera aufsetzen
        SetupCamera();

        // 2. Licht hinzufügen
        SetupLighting();

        // 3. Game Manager erstellen
        SetupGameManager();

        // 4. OSM Importer
        SetupOSMImporter();

        // 5. Route Manager
        SetupRouteManager();

        // 6. Depot erstellen
        SetupDepot();

        // 7. Spieler-Bus spawnen
        SetupPlayerBus();

        // 8. Depot Manager
        SetupDepotManager();

        Debug.Log("\n" + new string('=', 50));
        Debug.Log("✓ SETUP KOMPLETT!");
        Debug.Log("Wähle einen Bus mit Tasten 1, 2 oder 3");
        Debug.Log(new string('=', 50) + "\n");
    }

    private void SetupCamera()
    {
        // Haupt-Kamera
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            cameraObj.AddComponent<Camera>();
            cameraObj.AddComponent<AudioListener>();
            mainCamera = cameraObj.GetComponent<Camera>();
            mainCamera.tag = "MainCamera";
        }

        mainCamera.transform.position = new Vector3(0, 5, -10);
        mainCamera.transform.LookAt(Vector3.zero);
        Debug.Log("✓ Kamera eingerichtet");
    }

    private void SetupLighting()
    {
        // Direktes Licht (Sonne)
        GameObject sunObj = new GameObject("Directional Light");
        Light sunLight = sunObj.AddComponent<Light>();
        sunLight.type = LightType.Directional;
        sunLight.intensity = 1.2f;
        sunLight.color = Color.white;
        sunObj.transform.rotation = Quaternion.Euler(45, 45, 0);

        // Umgebungslicht
        RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.5f);
        RenderSettings.ambientMode = AmbientMode.Flat;

        Debug.Log("✓ Beleuchtung eingerichtet");
    }

    private void SetupGameManager()
    {
        GameObject gmObj = new GameObject("GameManager");
        GameManager gm = gmObj.AddComponent<GameManager>();
        DontDestroyOnLoad(gmObj);
        Debug.Log("✓ GameManager erstellt");
    }

    private void SetupOSMImporter()
    {
        GameObject osmObj = new GameObject("OSMImporter");
        OSMImporter osmImporter = osmObj.AddComponent<OSMImporter>();

        // OSM-Daten generieren und importieren
        var osmData = OSMDataGenerator.GenerateHechingenRegion();
        osmImporter.GenerateMapFromOSM(osmData);

        Debug.Log("✓ OSM-Karte geladen");
    }

    private void SetupRouteManager()
    {
        GameObject routeObj = new GameObject("RealisticRouteManager");
        RealisticRouteManager routeManager = routeObj.AddComponent<RealisticRouteManager>();
        Debug.Log("✓ Route Manager erstellt");
    }

    private void SetupDepot()
    {
        GameObject depotObj = new GameObject("BusDepot");
        BusDepot depot = depotObj.AddComponent<BusDepot>();
        depot.InitializeDepot();
        Debug.Log("✓ Bus-Depot erstellt (HVB Hechingen)");
    }

    private void SetupPlayerBus()
    {
        GameObject busObj = new GameObject("PlayerBus");
        busObj.transform.position = new Vector3(-20f, 0, 0);

        // Bus-Komponenten
        BusModel busModel = busObj.AddComponent<BusModel>();
        BusPhysicsController physicsController = busObj.AddComponent<BusPhysicsController>();
        
        // Rigidbody
        Rigidbody rb = busObj.AddComponent<Rigidbody>();
        rb.mass = 12000f;
        rb.linearDamping = 0.1f;
        rb.angularDamping = 0.1f;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Kamera-Controller
        GameObject cameraControllerObj = new GameObject("CameraController");
        cameraControllerObj.transform.parent = busObj.transform;
        CameraController cameraController = cameraControllerObj.AddComponent<CameraController>();

        Debug.Log("✓ Spieler-Bus mit allen Komponenten erstellt");
    }

    private void SetupDepotManager()
    {
        GameObject depotMgrObj = new GameObject("DepotManager");
        DepotManager depotManager = depotMgrObj.AddComponent<DepotManager>();
        Debug.Log("✓ Depot Manager aktiv - Starte Spiel!");
    }
}
