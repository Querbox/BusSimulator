using UnityEngine;

/// <summary>
/// Verwaltet die Kartendaten und Umgebung
/// </summary>
public class MapManager : MonoBehaviour
{
    [SerializeField] private string osmDataPath = "Data/map_export.osm";
    [SerializeField] private float mapScale = 1f;
    
    private GameObject mapContainer;
    
    private void Start()
    {
        InitializeMap();
    }
    
    private void InitializeMap()
    {
        mapContainer = new GameObject("Map");
        mapContainer.transform.parent = transform;
        
        LoadOSMData();
        GenerateHaltestellen();
        Debug.Log("Karte initialisiert!");
    }
    
    private void LoadOSMData()
    {
        // TODO: OSM-Datei laden und in Unity-Geometrie konvertieren
        // Für jetzt: Platzhalter-Implementierung
        Debug.Log($"Lade OSM-Daten von: {osmDataPath}");
    }
    
    private void GenerateHaltestellen()
    {
        // TODO: Haltestellen aus Kartendaten oder manuell definieren
        Debug.Log("Haltestellen generiert!");
    }
    
    public GameObject GetMapContainer()
    {
        return mapContainer;
    }
}
