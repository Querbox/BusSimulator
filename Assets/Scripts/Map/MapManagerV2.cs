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
            if (stop == null) continue;
            Vector3 worldPos = routeManager.LatLonToWorldPos(stop.lat, stop.lon);
            GameObject stopObj = new GameObject(stop.name);
            stopObj.transform.parent = stopsContainer.transform;
            stopObj.transform.position = worldPos;

            // Visual Component hinzufügen
            BusStopVisual visual = stopObj.AddComponent<BusStopVisual>();
            visual.Initialize(stop, GetStopMaterial(stop.type));
            Debug.Log($"Haltestelle platziert: {stop.name} bei {worldPos}");
        }
    }

    private void DrawRoutes()
    {
        var routes = routeManager.GetAllRoutes();
        Debug.Log($"Zeichne {routes.Length} Routen...");

        foreach (var route in routes)
        {
            if (route == null || route.stops == null || route.stops.Length == 0) continue;

            GameObject routeObj = new GameObject($"Route_{route.id}");
            routeObj.transform.parent = routesContainer.transform;

            LineRenderer lineRenderer = routeObj.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = GetRouteColor(route.type);
            lineRenderer.endColor = GetRouteColor(route.type);
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;

            var positions = new System.Collections.Generic.List<Vector3>(route.stops.Length);
            foreach (var stopReference in route.stops)
            {
                if (stopReference == null) continue;
                var stop = routeManager.GetStop(stopReference.stop_id);
                if (stop != null)
                {
                    positions.Add(routeManager.LatLonToWorldPos(stop.lat, stop.lon));
                }
            }

            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());

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

    private Material GetStopMaterial(string stopType)
    {
        if (stopMaterials == null || stopMaterials.Length == 0)
        {
            return null;
        }

        int index = stopType switch
        {
            "main_station" => 0,
            "school" => 1,
            "tourist_attraction" => 2,
            _ => 0
        };

        return stopMaterials[Mathf.Min(index, stopMaterials.Length - 1)];
    }

    public GameObject GetMapContainer()
    {
        return mapContainer;
    }
}
