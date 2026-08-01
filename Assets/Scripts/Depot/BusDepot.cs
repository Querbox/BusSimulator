using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Verwaltet das Bus-Depot (Bushaltestelle, Parkplätze, Garagen)
/// </summary>
public class BusDepot : MonoBehaviour
{
    [System.Serializable]
    public class ParkingSpot
    {
        public int spotId;
        public Vector3 position;
        public Quaternion rotation;
        public bool isOccupied;
        public GameObject occupyingBus;
        public bool isMaintenanceSpot;
    }

    [System.Serializable]
    public class DepotConfig
    {
        public string depotName = "HVB Hechingen Depot";
        public Vector3 depotCenter = Vector3.zero;
        public int totalParkingSpots = 12;
        public int maintenanceSpots = 2;
        public float spotSpacing = 6f;
        public float rowDistance = 8f;
    }

    [SerializeField] private DepotConfig config = new DepotConfig();
    private List<ParkingSpot> parkingSpots = new List<ParkingSpot>();
    private GameObject depotContainer;
    private GameObject buildingStructure;
    private bool isInitialized;

    private void Start()
    {
        InitializeDepot();
    }

    public void InitializeDepot()
    {
        if (isInitialized)
        {
            return;
        }

        if (config == null)
        {
            config = new DepotConfig();
        }

        // Haupt-Container
        depotContainer = new GameObject(config.depotName);
        depotContainer.transform.position = config.depotCenter;

        // Depot-Gebäude erstellen
        CreateDepotBuilding();

        // Parkplätze erstellen
        CreateParkingSpots();

        // Tankstelle
        CreateFuelingStation();

        // Werkstatt
        CreateMaintenanceArea();

        isInitialized = true;
        Debug.Log($"Depot '{config.depotName}' initialisiert mit {config.totalParkingSpots} Parkplätzen");
    }

    private void CreateDepotBuilding()
    {
        buildingStructure = new GameObject("DepotBuilding");
        buildingStructure.transform.parent = depotContainer.transform;
        buildingStructure.transform.localPosition = new Vector3(0, 0, -15f);

        // Hauptgebäude (Büro/Verwaltung)
        GameObject mainBuilding = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mainBuilding.name = "MainBuilding";
        mainBuilding.transform.parent = buildingStructure.transform;
        mainBuilding.transform.localPosition = Vector3.zero;
        mainBuilding.transform.localScale = new Vector3(12f, 5f, 8f);

        Material buildingMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        buildingMat.color = new Color(0.9f, 0.85f, 0.7f); // Beige
        mainBuilding.GetComponent<Renderer>().material = buildingMat;

        // Dach
        GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roof.name = "Roof";
        roof.transform.parent = buildingStructure.transform;
        roof.transform.localPosition = new Vector3(0, 5.5f, 0);
        roof.transform.localScale = new Vector3(13f, 1f, 9f);

        Material roofMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        roofMat.color = new Color(0.3f, 0.3f, 0.3f); // Dunkelgrau
        roof.GetComponent<Renderer>().material = roofMat;

        // Fenster
        for (int i = 0; i < 3; i++)
        {
            GameObject window = GameObject.CreatePrimitive(PrimitiveType.Cube);
            window.name = $"Window_{i}";
            window.transform.parent = buildingStructure.transform;
            window.transform.localPosition = new Vector3(-4f + i * 4f, 2f, -4.5f);
            window.transform.localScale = new Vector3(1.5f, 2f, 0.2f);

            Material windowMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            windowMat.color = new Color(0.3f, 0.7f, 1f);
            window.GetComponent<Renderer>().material = windowMat;

            Destroy(window.GetComponent<Collider>());
        }

        // Eingang
        GameObject door = GameObject.CreatePrimitive(PrimitiveType.Cube);
        door.name = "Door";
        door.transform.parent = buildingStructure.transform;
        door.transform.localPosition = new Vector3(0, 2f, -4.5f);
        door.transform.localScale = new Vector3(1.5f, 3.5f, 0.2f);

        Material doorMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        doorMat.color = new Color(0.2f, 0.2f, 0.2f);
        door.GetComponent<Renderer>().material = doorMat;

        Destroy(door.GetComponent<Collider>());
    }

    private void CreateParkingSpots()
    {
        int regularSpots = config.totalParkingSpots - config.maintenanceSpots;
        int spotIndex = 0;

        // Reihe 1: Normale Parkplätze
        for (int i = 0; i < regularSpots / 2; i++)
        {
            ParkingSpot spot = new ParkingSpot
            {
                spotId = spotIndex,
                position = config.depotCenter + new Vector3(-20f + i * config.spotSpacing, 0, 0),
                rotation = Quaternion.identity,
                isOccupied = i == 0, // Erster Platz ist besetzt (später wird hier der spieler-bus stehen)
                isMaintenanceSpot = false
            };
            parkingSpots.Add(spot);
            CreateParkingSpotMarker(spot);
            spotIndex++;
        }

        // Reihe 2: Zweite Reihe Parkplätze
        for (int i = regularSpots / 2; i < regularSpots; i++)
        {
            ParkingSpot spot = new ParkingSpot
            {
                spotId = spotIndex,
                position = config.depotCenter + new Vector3(-20f + (i - regularSpots / 2) * config.spotSpacing, 0, config.rowDistance),
                rotation = Quaternion.identity,
                isOccupied = false,
                isMaintenanceSpot = false
            };
            parkingSpots.Add(spot);
            CreateParkingSpotMarker(spot);
            spotIndex++;
        }

        // Wartungs-Plätze
        for (int i = 0; i < config.maintenanceSpots; i++)
        {
            ParkingSpot spot = new ParkingSpot
            {
                spotId = spotIndex,
                position = config.depotCenter + new Vector3(10f + i * (config.spotSpacing + 2f), 0, 0),
                rotation = Quaternion.identity,
                isOccupied = false,
                isMaintenanceSpot = true
            };
            parkingSpots.Add(spot);
            CreateParkingSpotMarker(spot, true);
            spotIndex++;
        }
    }

    private void CreateParkingSpotMarker(ParkingSpot spot, bool isMaintenance = false)
    {
        GameObject marker = new GameObject($"ParkingSpot_{spot.spotId}");
        marker.transform.parent = depotContainer.transform;
        marker.transform.position = spot.position;

        // Boden-Markierung
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.parent = marker.transform;
        ground.transform.localPosition = Vector3.zero;
        ground.transform.localScale = new Vector3(3f, 1f, 5f);

        Material groundMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        groundMat.color = isMaintenance ? new Color(1f, 1f, 0.3f) : new Color(1f, 1f, 1f); // Gelb für Wartung
        ground.GetComponent<Renderer>().material = groundMat;

        Destroy(ground.GetComponent<Collider>());

        // Nummern-Anzeige
        GameObject numberDisplay = new GameObject("NumberDisplay");
        numberDisplay.transform.parent = marker.transform;
        numberDisplay.transform.localPosition = new Vector3(0, 0.5f, -2.5f);

        // Marker speichern
        marker.AddComponent<ParkingSpotMarker>().SetSpot(spot);
    }

    private void CreateFuelingStation()
    {
        GameObject fuelingStation = new GameObject("FuelingStation");
        fuelingStation.transform.parent = depotContainer.transform;
        fuelingStation.transform.position = config.depotCenter + new Vector3(25f, 0, 5f);

        // Säule
        GameObject pump = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pump.name = "Pump";
        pump.transform.parent = fuelingStation.transform;
        pump.transform.localPosition = Vector3.zero;
        pump.transform.localScale = new Vector3(1f, 4f, 1f);

        Material pumpMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        pumpMat.color = new Color(0.2f, 0.2f, 0.2f);
        pump.GetComponent<Renderer>().material = pumpMat;

        // Tank-Reservoir
        GameObject tank = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        tank.name = "Tank";
        tank.transform.parent = fuelingStation.transform;
        tank.transform.localPosition = new Vector3(-3f, 0, 0);
        tank.transform.localScale = new Vector3(4f, 3f, 4f);

        Material tankMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        tankMat.color = new Color(1f, 0.5f, 0f); // Orange
        tank.GetComponent<Renderer>().material = tankMat;

    }

    private void CreateMaintenanceArea()
    {
        GameObject maintenanceArea = new GameObject("MaintenanceArea");
        maintenanceArea.transform.parent = depotContainer.transform;
        maintenanceArea.transform.position = config.depotCenter + new Vector3(10f, 0, -15f);

        // Wartungshalle
        GameObject garage = GameObject.CreatePrimitive(PrimitiveType.Cube);
        garage.name = "Garage";
        garage.transform.parent = maintenanceArea.transform;
        garage.transform.localPosition = Vector3.zero;
        garage.transform.localScale = new Vector3(10f, 4f, 6f);

        Material garageMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        garageMat.color = new Color(0.5f, 0.5f, 0.5f);
        garage.GetComponent<Renderer>().material = garageMat;

    }

    public ParkingSpot GetAvailableParkingSpot()
    {
        foreach (var spot in parkingSpots)
        {
            if (!spot.isOccupied && !spot.isMaintenanceSpot)
            {
                return spot;
            }
        }
        return null;
    }

    public ParkingSpot GetParkingSpot(int spotId)
    {
        return parkingSpots.Find(s => s.spotId == spotId);
    }

    public void ParkBus(GameObject busObject, int spotId)
    {
        var spot = GetParkingSpot(spotId);
        if (spot != null && !spot.isOccupied)
        {
            spot.isOccupied = true;
            spot.occupyingBus = busObject;
            busObject.transform.position = spot.position;
            busObject.transform.rotation = spot.rotation;
            Debug.Log($"Bus geparkt auf Platz {spotId}");
        }
    }

    public void UnparkBus(int spotId)
    {
        var spot = GetParkingSpot(spotId);
        if (spot != null && spot.isOccupied)
        {
            spot.isOccupied = false;
            spot.occupyingBus = null;
            Debug.Log($"Bus von Platz {spotId} entfernt");
        }
    }

    public List<ParkingSpot> GetAllParkingSpots() => parkingSpots;
}
