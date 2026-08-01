using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Automatische Straßen-Erstellung aus OSM-Daten mit realistische Physik
/// </summary>
public class AutoRoadSystem : MonoBehaviour
{
    [SerializeField] private OSMImporter osmImporter;
    [SerializeField] private float defaultRoadWidth = 6f;
    [SerializeField] private LayerMask roadLayer;

    private RoadBuilder roadBuilder;
    private List<GameObject> roads = new List<GameObject>();

    private void Start()
    {
        roadBuilder = gameObject.AddComponent<RoadBuilder>();
        GenerateRoadsFromOSM();
    }

    private void GenerateRoadsFromOSM()
    {
        // Erstelle Straßen-Layer wenn nicht vorhanden
        if (LayerMask.NameToLayer("Road") == -1)
        {
            Debug.LogWarning("Road-Layer nicht gefunden. Erstelle ihn manuell in Unity!");
        }

        // TODO: Integriere mit OSMImporter für echte Straßendaten
        Debug.Log("Straßen-System initialisiert");
    }

    public bool IsOnRoad(Vector3 position, float checkRadius = 2f)
    {
        Collider[] colliders = Physics.OverlapSphere(position, checkRadius, roadLayer);
        return colliders.Length > 0;
    }
}
