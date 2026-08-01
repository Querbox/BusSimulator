using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Erstellt Straßen-Geometrie für realistische Fahrt-Physik
/// </summary>
public class RoadBuilder : MonoBehaviour
{
    [System.Serializable]
    public class RoadSegment
    {
        public string name;
        public Vector3[] waypoints;
        public float width = 6f;
        public bool isTwoWay = true;
    }

    private List<RoadSegment> roads = new List<RoadSegment>();

    public void BuildRoadsFromOSM(List<Vector3[]> roadPaths)
    {
        foreach (var path in roadPaths)
        {
            BuildRoad(path, 6f);
        }
    }

    public void BuildRoad(Vector3[] waypoints, float width)
    {
        if (waypoints.Length < 2) return;

        GameObject roadObj = new GameObject("Road");
        roadObj.transform.parent = transform;

        // Erstelle Mesh für Straße
        Mesh roadMesh = new Mesh();
        Vector3[] vertices = new Vector3[waypoints.Length * 2];
        
        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 forward = (i < waypoints.Length - 1) 
                ? (waypoints[i + 1] - waypoints[i]).normalized 
                : (waypoints[i] - waypoints[i - 1]).normalized;
            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

            vertices[i] = waypoints[i] + right * (width / 2);
            vertices[i + waypoints.Length] = waypoints[i] - right * (width / 2);
        }

        roadMesh.vertices = vertices;

        // Triangles
        int[] triangles = new int[(waypoints.Length - 1) * 6];
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            int baseIdx = i * 6;
            int v0 = i;
            int v1 = i + 1;
            int v2 = i + waypoints.Length;
            int v3 = i + waypoints.Length + 1;

            triangles[baseIdx] = v0;
            triangles[baseIdx + 1] = v2;
            triangles[baseIdx + 2] = v1;
            triangles[baseIdx + 3] = v1;
            triangles[baseIdx + 4] = v2;
            triangles[baseIdx + 5] = v3;
        }

        roadMesh.triangles = triangles;
        roadMesh.RecalculateNormals();

        MeshFilter meshFilter = roadObj.AddComponent<MeshFilter>();
        meshFilter.mesh = roadMesh;

        MeshRenderer renderer = roadObj.AddComponent<MeshRenderer>();
        Material roadMat = new Material(Shader.Find("Standard"));
        roadMat.color = new Color(0.5f, 0.5f, 0.5f);
        renderer.material = roadMat;

        MeshCollider collider = roadObj.AddComponent<MeshCollider>();
        collider.convex = false;

        Debug.Log($"Straße gebaut: {waypoints.Length} Waypoints, {width}m breit");
    }
}
