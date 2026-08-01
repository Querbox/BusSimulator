using UnityEngine;

/// <summary>
/// Stellt visuell Bushaltestellen als 3D-Objekte dar
/// </summary>
public class BusStopVisual : MonoBehaviour
{
    [SerializeField] private BusStop busStop;
    [SerializeField] private Material stopMaterial;
    [SerializeField] private float markerHeight = 2f;
    [SerializeField] private float markerRadius = 1f;

    private GameObject marker;

    private void Start()
    {
        if (busStop != null)
        {
            CreateStopMarker();
        }
    }

    public void Initialize(BusStop stop, Material material = null)
    {
        busStop = stop;
        stopMaterial = material;

        if (marker == null && busStop != null)
        {
            CreateStopMarker();
        }
    }

    private void CreateStopMarker()
    {
        // Erstelle einen zylinder als Haltestellen-Marker
        marker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        marker.name = $"Stop: {busStop.name}";
        marker.transform.parent = transform;
        marker.transform.localPosition = Vector3.zero;
        marker.transform.localScale = new Vector3(markerRadius * 2, markerHeight, markerRadius * 2);

        // Material setzen
        if (stopMaterial != null)
        {
            marker.GetComponent<Renderer>().sharedMaterial = stopMaterial;
        }
        else
        {
            // Standard-Farbe basierend auf Typ
            Color color = GetColorByStopType(busStop.type);
            marker.GetComponent<Renderer>().material.color = color;
        }

        // Kollider entfernen (nicht nötig für visuelle Marker)
        Destroy(marker.GetComponent<Collider>());

        // Text-Anzeige erstellen
        GameObject textObj = new GameObject("StopName");
        textObj.transform.parent = marker.transform;
        textObj.transform.localPosition = Vector3.up * (markerHeight / 2 + 1);
        textObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        // Debug.Log($"Haltestelle erstellt: {busStop.name}");
    }

    private Color GetColorByStopType(string type)
    {
        return type switch
        {
            "main_station" => Color.red,
            "city_center" => Color.yellow,
            "school" => Color.blue,
            "residential" => Color.green,
            "industrial" => Color.gray,
            "village_center" => new Color(1f, 0.5f, 0f), // Orange
            "landmark" => Color.magenta,
            "tourist_attraction" => new Color(1f, 0.84f, 0f), // Gold
            "parking" => Color.cyan,
            _ => Color.white
        };
    }

    public BusStop GetBusStop()
    {
        return busStop;
    }
}
