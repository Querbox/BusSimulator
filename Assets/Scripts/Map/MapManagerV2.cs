using UnityEngine;

/// <summary>
/// Aktualisierte MapManager mit Unterstützung für realistische Routen
/// </summary>
public class MapManagerV2 : MonoBehaviour
{
    [SerializeField] private RealisticRouteManager routeManager;
    [SerializeField] private GameObject busStopPrefab;
    [SerializeField] private Material[] stopMaterials;
    [SerializeField] private LineRenderer routeLinePrefab;

    private GameObject mapContainer;
    private GameObject stopsContainer;
    private GameObject routesContainer;

    private void Start()
    {
        InitializeMap();
    }

    private void InitializeMap()
    {
        // Container erstellen
        mapContainer = new GameObject("Map");
        mapContainer.transform.parent = transform;

        stopsContainer = new GameObject("Stops");
        stopsContainer.transform.parent = mapContainer.transform;

        routesContainer = new GameObject("Routes");
        routesContainer.transform.parent = mapContainer.transform;

        if (routeManager != null)
        {
            SpawnBusStops();
            DrawRoutes();
        }

        Debug.Log("Karte mit realistischen Daten initialisiert!");
    }

    private void SpawnBusStops()
    {
        var stops = routeManager.GetAllStops();
        Debug.Log($"Spawne {stops.Length} Haltestellen...");

        foreach (var stop in stops)
        {
            Vector3 worldPos = routeManager.LatLonToWorldPos(stop.lat, stop.lon);
            GameObject stopObj = new GameObject(stop.name);
            stopObj.transform.parent = stopsContainer.transform;
            stopObj.transform.position = worldPos;

            // Visual Component hinzufügen
            BusStopVisual visual = stopObj.AddComponent<BusStopVisual>();
            Debug.Log($"Haltestelle platziert: {stop.name} bei {worldPos}");
        }
    }

    private void DrawRoutes()
    {
        var routes = routeManager.GetAllRoutes();
        Debug.Log($"Zeichne {routes.Length} Routen...");

        foreach (var route in routes)
        {
            GameObject routeObj = new GameObject($"Route_{route.id}");
            routeObj.transform.parent = routesContainer.transform;

            LineRenderer lineRenderer = routeObj.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = GetRouteColor(route.type);
            lineRenderer.endColor = GetRouteColor(route.type);
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;

            Vector3[] positions = new Vector3[route.stops.Length];
            for (int i = 0; i < route.stops.Length; i++)
            {
                var stop = routeManager.GetStop(route.stops[i].stop_id);
                if (stop != null)
                {
                    positions[i] = routeManager.LatLonToWorldPos(stop.lat, stop.lon);
                }
            }

            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);

            Debug.Log($"Route gezeichnet: {route.name}");
        }
    }

    private Color GetRouteColor(string routeType)
    {
        return routeType switch
        {
            "regional_bus" => Color.blue,
            "school_bus" => Color.yellow,
            "tourist_bus" => Color.magenta,
            _ => Color.white
        };
    }

    public GameObject GetMapContainer()
    {
        return mapContainer;
    }
}
