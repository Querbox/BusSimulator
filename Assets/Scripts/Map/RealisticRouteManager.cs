using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

[System.Serializable]
public class BusStop
{
    public string id;
    public string name;
    public float lat;
    public float lon;
    public string description;
    public string type;
}

[System.Serializable]
public class StopReference
{
    public int order;
    public string stop_id;
    public int arrival_offset_minutes;
    public int departure_offset_minutes;
    public string type;
}

[System.Serializable]
public class TimeTableEntry
{
    public string day;
    public string[] departures;
}

[System.Serializable]
public class BusRoute
{
    public string id;
    public string name;
    public string @operator;
    public string description;
    public string type;
    public StopReference[] stops;
    public TimeTableEntry[] timetable;
}

[System.Serializable]
public class BusNetworkData
{
    public BusStop[] stops;
    public BusRoute[] routes;
    public MetaData metadata;
}

[System.Serializable]
public class MetaData
{
    public string region;
    public string country;
    public string data_source;
    public string last_updated;
    public string note;
}

/// <summary>
/// Lädt und verwaltet realistische Buslinien- und Haltestellen-Daten
/// </summary>
public class RealisticRouteManager : MonoBehaviour
{
    [SerializeField] private string routeDataPath = "Data/realistic_routes_data";
    private BusNetworkData busNetworkData;
    private Dictionary<string, BusStop> stopsDict = new Dictionary<string, BusStop>();
    private Dictionary<string, BusRoute> routesDict = new Dictionary<string, BusRoute>();

    private void Start()
    {
        LoadRealisticRouteData();
    }

    private void LoadRealisticRouteData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(routeDataPath);
        if (jsonFile == null)
        {
            Debug.LogError($"Konnte Routendatei nicht laden: {routeDataPath}");
            return;
        }

        try
        {
            busNetworkData = UnityEngine.JsonUtility.FromJson<BusNetworkData>(jsonFile.text);
            
            // Stopps in Dictionary laden
            foreach (var stop in busNetworkData.stops)
            {
                stopsDict[stop.id] = stop;
                Debug.Log($"Haltestelle geladen: {stop.name} ({stop.id})");
            }

            // Routen in Dictionary laden
            foreach (var route in busNetworkData.routes)
            {
                routesDict[route.id] = route;
                Debug.Log($"Route geladen: {route.name} mit {route.stops.Length} Haltestellen");
            }

            Debug.Log($"Busnetzwerk erfolgreich geladen! {busNetworkData.stops.Length} Haltestellen, {busNetworkData.routes.Length} Routen.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Fehler beim Laden der Routendaten: {ex.Message}");
        }
    }

    /// <summary>
    /// Gibt eine Haltestelle anhand ihrer ID zurück
    /// </summary>
    public BusStop GetStop(string stopId)
    {
        if (stopsDict.TryGetValue(stopId, out var stop))
        {
            return stop;
        }
        Debug.LogWarning($"Haltestelle nicht gefunden: {stopId}");
        return null;
    }

    /// <summary>
    /// Gibt alle Haltestellen zurück
    /// </summary>
    public BusStop[] GetAllStops()
    {
        return busNetworkData.stops;
    }

    /// <summary>
    /// Gibt eine Route anhand ihrer ID zurück
    /// </summary>
    public BusRoute GetRoute(string routeId)
    {
        if (routesDict.TryGetValue(routeId, out var route))
        {
            return route;
        }
        Debug.LogWarning($"Route nicht gefunden: {routeId}");
        return null;
    }

    /// <summary>
    /// Gibt alle verfügbaren Routen zurück
    /// </summary>
    public BusRoute[] GetAllRoutes()
    {
        return busNetworkData.routes;
    }

    /// <summary>
    /// Konvertiert Koordinaten (Lat/Lon) in lokale Unity-Positionen
    /// </summary>
    public Vector3 LatLonToWorldPos(float lat, float lon)
    {
        // Vereinfachte Konvertierung: 1 Grad ≈ 111 km
        // Für Hechingen als Mittelpunkt
        float baseLat = 48.3767f;
        float baseLon = 8.7544f;

        float latOffset = (lat - baseLat) * 111000f;
        float lonOffset = (lon - baseLon) * 111000f * Mathf.Cos(baseLat * Mathf.Deg2Rad);

        // Skalierung: 1 Meter = 1 Unit (könnte angepasst werden)
        return new Vector3(lonOffset / 100f, 0, latOffset / 100f);
    }

    /// <summary>
    /// Gibt alle Abfahrtszeiten einer Route für einen bestimmten Tag zurück
    /// </summary>
    public string[] GetDepartureTimes(string routeId, string dayType)
    {
        var route = GetRoute(routeId);
        if (route == null) return new string[0];

        foreach (var timeTable in route.timetable)
        {
            if (timeTable.day.Contains(dayType))
            {
                return timeTable.departures;
            }
        }
        return new string[0];
    }
}
