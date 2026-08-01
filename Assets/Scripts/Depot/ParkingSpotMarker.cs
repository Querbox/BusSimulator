using UnityEngine;

/// <summary>
/// Marker für einen individuellen Parkplatz
/// </summary>
public class ParkingSpotMarker : MonoBehaviour
{
    private BusDepot.ParkingSpot spot;
    [SerializeField] private Color occupiedColor = Color.red;
    [SerializeField] private Color freeColor = Color.green;
    [SerializeField] private Color maintenanceColor = Color.yellow;

    private Renderer markerRenderer;

    private void Start()
    {
        markerRenderer = GetComponentInChildren<Renderer>();
    }

    public void SetSpot(BusDepot.ParkingSpot parkingSpot)
    {
        spot = parkingSpot;
    }

    private void Update()
    {
        if (spot == null || markerRenderer == null) return;

        // Farbe basierend auf Status aktualisieren
        if (spot.isMaintenanceSpot)
        {
            markerRenderer.material.color = maintenanceColor;
        }
        else
        {
            markerRenderer.material.color = spot.isOccupied ? occupiedColor : freeColor;
        }
    }

    public BusDepot.ParkingSpot GetSpot() => spot;
}
