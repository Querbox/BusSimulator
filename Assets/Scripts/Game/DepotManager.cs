using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Verwaltet den Depot-Start-Screen und Bus-Auswahl
/// </summary>
public class DepotManager : MonoBehaviour
{
    [SerializeField] private BusDepot busDepot;
    [SerializeField] private BusPhysicsController playerBusController;
    [SerializeField] private Transform playerBusTransform;
    [SerializeField] private CameraController cameraController;

    private bool gameStarted = false;
    private List<AvailableBus> availableBuses = new List<AvailableBus>();

    [System.Serializable]
    public class AvailableBus
    {
        public string busId;
        public string routeAssignment;
        public int nextDeparture; // Minuten
        public int passengers;
        public string status; // "Ready", "Maintenance", "Fueling"
    }

    private void Start()
    {
        InitializeDepotStart();
    }

    private void InitializeDepotStart()
    {
        // Verfügbare Busse generieren
        availableBuses.Add(new AvailableBus
        {
            busId = "HEC-001",
            routeAssignment = "Linie 753: Hechingen - Boll - Burg Hohenzollern",
            nextDeparture = 15,
            passengers = 0,
            status = "Ready"
        });

        availableBuses.Add(new AvailableBus
        {
            busId = "HEC-002",
            routeAssignment = "Linie 752: Schulverkehr",
            nextDeparture = 30,
            passengers = 0,
            status = "Ready"
        });

        availableBuses.Add(new AvailableBus
        {
            busId = "HEC-003",
            routeAssignment = "Linie 754: Hohenzollern Express",
            nextDeparture = 45,
            passengers = 0,
            status = "Fueling"
        });

        Debug.Log($"Depot gestartet mit {availableBuses.Count} verfügbaren Bussen");
        ShowDepotScreen();
    }

    private void ShowDepotScreen()
    {
        Debug.Log("\n========== BUS-DEPOT HECHINGEN ==========");
        Debug.Log("Willkommen! Wähle einen Bus für deine Schicht.\n");
        Debug.Log("Verfügbare Busse:");
        Debug.Log("----------------------------------------");

        for (int i = 0; i < availableBuses.Count; i++)
        {
            var bus = availableBuses[i];
            Debug.Log($"\n[{i + 1}] {bus.busId}");
            Debug.Log($"    Route: {bus.routeAssignment}");
            Debug.Log($"    Status: {bus.status}");
            Debug.Log($"    Nächste Abfahrt: {bus.nextDeparture} Min");
            Debug.Log($"    Passagiere: {bus.passengers}");
        }

        Debug.Log("\n----------------------------------------");
        Debug.Log("Drücke 1, 2 oder 3 um einen Bus zu wählen");
        Debug.Log("Drücke ESC zum Abbrechen");
    }

    private void Update()
    {
        if (gameStarted) return;

        // Bus-Auswahl
        if (BuiltInInputControls.WasNumberPressed(1))
            SelectBus(0);
        else if (BuiltInInputControls.WasNumberPressed(2))
            SelectBus(1);
        else if (BuiltInInputControls.WasNumberPressed(3))
            SelectBus(2);
        else if (BuiltInInputControls.WasEscapePressed())
            Debug.Log("Spiel abgebrochen");
    }

    private void SelectBus(int busIndex)
    {
        if (busIndex < 0 || busIndex >= availableBuses.Count)
            return;

        var selectedBus = availableBuses[busIndex];

        if (selectedBus.status != "Ready")
        {
            Debug.LogWarning($"Bus {selectedBus.busId} ist nicht verfügbar! Status: {selectedBus.status}");
            return;
        }

        Debug.Log($"\n✓ Bus {selectedBus.busId} gewählt!");
        Debug.Log($"Route: {selectedBus.routeAssignment}");
        Debug.Log($"Abfahrt in {selectedBus.nextDeparture} Minuten\n");

        gameStarted = true;
        StartGameWithBus(selectedBus);
    }

    private void StartGameWithBus(AvailableBus selectedBus)
    {
        // Bus an Startplatz positionieren
        if (busDepot != null && playerBusTransform != null)
        {
            var startSpot = busDepot.GetParkingSpot(0); // Erster Platz
            if (startSpot != null)
            {
                playerBusTransform.position = startSpot.position + Vector3.forward * 2f; // Leicht vorgefahren
                playerBusTransform.rotation = startSpot.rotation;
                Debug.Log($"Bus {selectedBus.busId} ist startklar!");
            }
        }

        // Spiel starten
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
            Debug.Log("Fahrt begonnen! Viel Spaß!");
        }
    }

    public void Configure(BusDepot depot, BusPhysicsController busController,
        Transform busTransform, CameraController controller)
    {
        busDepot = depot;
        playerBusController = busController;
        playerBusTransform = busTransform;
        cameraController = controller;
    }

    public List<AvailableBus> GetAvailableBuses() => availableBuses;
}
