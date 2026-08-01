using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Navigations-System mit Routenführung und Orientierung
/// </summary>
public class NavigationSystem : MonoBehaviour
{
    [SerializeField] private RealisticRouteManager routeManager;
    [SerializeField] private Transform busTransform;

    private BusRoute currentRoute;
    private int currentStopIndex = 0;
    private List<BusStop> routeStops = new List<BusStop>();
    private BusStop nextStop;
    private bool navigationActive = false;

    private void Start()
    {
        if (routeManager == null)
            routeManager = FindAnyObjectByType<RealisticRouteManager>();
    }

    public void StartRoute(string routeId)
    {
        currentRoute = routeManager.GetRoute(routeId);
        if (currentRoute == null)
        {
            Debug.LogError($"Route {routeId} nicht gefunden!");
            return;
        }

        routeStops.Clear();
        currentStopIndex = 0;

        // Lade alle Haltestellen der Route
        foreach (var stopRef in currentRoute.stops)
        {
            var stop = routeManager.GetStop(stopRef.stop_id);
            if (stop != null)
                routeStops.Add(stop);
        }

        navigationActive = true;
        UpdateNextStop();
        Debug.Log($"Navigation gestartet: {currentRoute.name} mit {routeStops.Count} Haltestellen");
    }

    private void UpdateNextStop()
    {
        if (currentStopIndex < routeStops.Count)
        {
            nextStop = routeStops[currentStopIndex];
            Debug.Log($"Nächste Haltestelle: {nextStop.name} ({GetDistanceToNextStop():F0}m entfernt)");
        }
        else
        {
            nextStop = null;
            navigationActive = false;
            Debug.Log("Route beendet!");
        }
    }

    public void ReachedStop()
    {
        if (navigationActive && currentStopIndex < routeStops.Count)
        {
            currentStopIndex++;
            UpdateNextStop();
        }
    }

    public BusStop GetNextStop()
    {
        return nextStop;
    }

    public float GetDistanceToNextStop()
    {
        if (nextStop == null || busTransform == null)
            return 0f;

        Vector3 nextStopPos = routeManager.LatLonToWorldPos(nextStop.lat, nextStop.lon);
        float distance = Vector3.Distance(busTransform.position, nextStopPos);
        return distance * 100f; // Konvertiere zu Metern (abhängig von Skalierung)
    }

    public float GetDirectionToNextStop()
    {
        if (nextStop == null || busTransform == null)
            return 0f;

        Vector3 nextStopPos = routeManager.LatLonToWorldPos(nextStop.lat, nextStop.lon);
        Vector3 direction = (nextStopPos - busTransform.position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        return angle - busTransform.eulerAngles.y;
    }

    public BusRoute GetCurrentRoute()
    {
        return currentRoute;
    }

    public float GetRouteProgress()
    {
        if (routeStops.Count == 0)
            return 0f;
        return (float)currentStopIndex / routeStops.Count;
    }

    public bool IsNavigationActive()
    {
        return navigationActive;
    }
}
