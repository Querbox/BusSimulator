using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Importiert und verarbeitet OpenStreetMap-Daten in Unity
/// </summary>
public class OSMImporter : MonoBehaviour
{
    public class OSMNode
    {
        public long id;
        public float lat;
        public float lon;
        public Dictionary<string, string> tags = new Dictionary<string, string>();
    }

    public class OSMWay
    {
        public long id;
        public List<long> nodes = new List<long>();
        public Dictionary<string, string> tags = new Dictionary<string, string>();
    }

    public class OSMData
    {
        public List<OSMNode> nodes = new List<OSMNode>();
        public List<OSMWay> ways = new List<OSMWay>();
    }

    private Dictionary<long, OSMNode> nodeMap = new Dictionary<long, OSMNode>();
    private List<OSMWay> ways = new List<OSMWay>();
    
    [SerializeField] private Vector2 mapCenter = new Vector2(48.3767f, 8.7544f); // Hechingen
    [SerializeField] private float mapScale = 100f; // Skalierung: 1 Unit = X Meter
    [SerializeField] private Material roadMaterial;
    [SerializeField] private Material buildingMaterial;
    [SerializeField] private Material waterMaterial;
    [SerializeField] private Material greenMaterial;

    public void GenerateMapFromOSM(OSMData osmData)
    {
        Debug.Log($"Starte OSM-Import: {osmData.nodes.Count} Nodes, {osmData.ways.Count} Ways");
        
        // Nodes in Map laden
        foreach (var node in osmData.nodes)
        {
            nodeMap[node.id] = node;
        }

        // Ways verarbeiten
        foreach (var way in osmData.ways)
        {
            ProcessWay(way);
        }

        Debug.Log("OSM-Import abgeschlossen!");
    }

    private void ProcessWay(OSMWay way)
    {
        // Bestimme Typ des Ways basierend auf Tags
        string wayType = DetermineWayType(way.tags);

        List<Vector3> pathPoints = new List<Vector3>();
        foreach (var nodeId in way.nodes)
        {
            if (nodeMap.TryGetValue(nodeId, out var node))
            {
                Vector3 pos = LatLonToWorldPos(node.lat, node.lon);
                pathPoints.Add(pos);
            }
        }

        if (pathPoints.Count < 2) return;

        // Erstelle visuelle Repräsentation
        CreateVisualWay(way.id, pathPoints, wayType);
    }

    private string DetermineWayType(Dictionary<string, string> tags)
    {
        if (tags.ContainsKey("highway"))
            return "road";
        if (tags.ContainsKey("building"))
            return "building";
        if (tags.ContainsKey("water") || tags.ContainsKey("natural") && tags["natural"] == "water")
            return "water";
        if (tags.ContainsKey("landuse") && tags["landuse"] == "forest")
            return "forest";
        if (tags.ContainsKey("landuse") && tags["landuse"] == "grass")
            return "grass";
        
        return "unknown";
    }

    private void CreateVisualWay(long wayId, List<Vector3> points, string wayType)
    {
        GameObject wayObj = new GameObject($"Way_{wayId}_{wayType}");
        wayObj.transform.parent = transform;

        if (wayType == "road")
        {
            CreateRoad(wayObj, points);
        }
        else if (wayType == "building")
        {
            CreateBuilding(wayObj, points);
        }
        else if (wayType == "water")
        {
            CreateWater(wayObj, points);
        }
    }

    private void CreateRoad(GameObject roadObj, List<Vector3> points)
    {
        LineRenderer lineRenderer = roadObj.AddComponent<LineRenderer>();
        lineRenderer.sharedMaterial = roadMaterial ?? CreateRuntimeMaterial(Color.white);
        lineRenderer.startColor = new Color(0.5f, 0.5f, 0.5f); // Grau
        lineRenderer.endColor = new Color(0.5f, 0.5f, 0.5f);
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    private void CreateBuilding(GameObject buildingObj, List<Vector3> points)
    {
        if (points.Count < 3) return;

        // OSM-Flächen wiederholen häufig den ersten Punkt am Ende.
        int pointCount = points.Count;
        if (points[0] == points[pointCount - 1])
        {
            pointCount--;
        }

        if (pointCount < 3) return;

        Mesh mesh = new Mesh { name = $"Building_{buildingObj.name}" };
        Vector3[] vertices = new Vector3[pointCount * 2];
        var triangles = new List<int>(pointCount * 12);
        
        float buildingHeight = UnityEngine.Random.Range(3f, 15f);
        
        // Boden und Dach
        for (int i = 0; i < pointCount; i++)
        {
            vertices[i] = points[i];
            vertices[i + pointCount] = points[i] + Vector3.up * buildingHeight;

            int next = (i + 1) % pointCount;
            triangles.Add(i);
            triangles.Add(i + pointCount);
            triangles.Add(next);
            triangles.Add(next);
            triangles.Add(i + pointCount);
            triangles.Add(next + pointCount);
        }

        // Vereinfachte Fächer-Triangulierung für Boden und Dach.
        for (int i = 1; i < pointCount - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i + 1);
            triangles.Add(i);
            triangles.Add(pointCount);
            triangles.Add(pointCount + i);
            triangles.Add(pointCount + i + 1);
        }

        mesh.vertices = vertices;
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        MeshFilter meshFilter = buildingObj.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = mesh;

        MeshRenderer renderer = buildingObj.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = buildingMaterial ?? CreateRuntimeMaterial(new Color(0.8f, 0.8f, 0.8f));

        MeshCollider collider = buildingObj.AddComponent<MeshCollider>();
        collider.sharedMesh = mesh;
    }

    private void CreateWater(GameObject waterObj, List<Vector3> points)
    {
        LineRenderer lineRenderer = waterObj.AddComponent<LineRenderer>();
        lineRenderer.sharedMaterial = waterMaterial ?? CreateRuntimeMaterial(Color.white);
        lineRenderer.startColor = new Color(0.2f, 0.5f, 1f); // Blau
        lineRenderer.endColor = new Color(0.2f, 0.5f, 1f);
        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.3f;
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    private Vector3 LatLonToWorldPos(float lat, float lon)
    {
        // Konvertiere Lat/Lon zu lokalen Koordinaten relativ zum Map Center
        float latOffset = (lat - mapCenter.x) * 111000f;
        float lonOffset = (lon - mapCenter.y) * 111000f * Mathf.Cos(mapCenter.x * Mathf.Deg2Rad);

        return new Vector3(lonOffset / mapScale, 0, latOffset / mapScale);
    }

    private static Material CreateRuntimeMaterial(Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            throw new InvalidOperationException("Der URP/Lit-Shader ist nicht verfügbar. Prüfe die aktive URP-Konfiguration.");
        }

        return new Material(shader) { color = color };
    }
}
