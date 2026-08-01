using UnityEngine;

/// <summary>
/// Generator für realistische OSM-Testdaten für die Region Hechingen
/// Dient als Ersatz bis echte OSM-Daten importiert sind
/// </summary>
public class OSMDataGenerator : MonoBehaviour
{
    public static OSMImporter.OSMData GenerateHechingenRegion()
    {
        var osmData = new OSMImporter.OSMData();

        // Haupt-Straßen definieren (vereinfacht)
        // Straße 1: Hauptstraße durch Hechingen (Süd-Nord)
        var road1Nodes = new[]
        {
            new OSMImporter.OSMNode { id = 1, lat = 48.3650f, lon = 8.7544f },
            new OSMImporter.OSMNode { id = 2, lat = 48.3700f, lon = 8.7544f },
            new OSMImporter.OSMNode { id = 3, lat = 48.3750f, lon = 8.7544f },
            new OSMImporter.OSMNode { id = 4, lat = 48.3800f, lon = 8.7544f },
            new OSMImporter.OSMNode { id = 5, lat = 48.3850f, lon = 8.7544f }
        };

        foreach (var node in road1Nodes)
            osmData.nodes.Add(node);

        var way1 = new OSMImporter.OSMWay { id = 101 };
        foreach (var node in road1Nodes)
            way1.nodes.Add(node.id);
        way1.tags["highway"] = "primary";
        way1.tags["name"] = "Hauptstraße";
        osmData.ways.Add(way1);

        // Straße 2: Ost-West Straße (zu Burg Hohenzollern)
        var road2Nodes = new[]
        {
            new OSMImporter.OSMNode { id = 6, lat = 48.3750f, lon = 8.7400f },
            new OSMImporter.OSMNode { id = 7, lat = 48.3750f, lon = 8.7544f },
            new OSMImporter.OSMNode { id = 8, lat = 48.3750f, lon = 8.7700f },
            new OSMImporter.OSMNode { id = 9, lat = 48.3750f, lon = 8.7850f },
            new OSMImporter.OSMNode { id = 10, lat = 48.3300f, lon = 8.8047f } // Burg Hohenzollern
        };

        foreach (var node in road2Nodes)
            osmData.nodes.Add(node);

        var way2 = new OSMImporter.OSMWay { id = 102 };
        foreach (var node in road2Nodes)
            way2.nodes.Add(node.id);
        way2.tags["highway"] = "secondary";
        way2.tags["name"] = "Burgstraße";
        osmData.ways.Add(way2);

        // Gebäude in Hechingen (Vereinfacht als Polygone)
        var buildingNodes = new[]
        {
            new OSMImporter.OSMNode { id = 20, lat = 48.3760f, lon = 8.7530f },
            new OSMImporter.OSMNode { id = 21, lat = 48.3765f, lon = 8.7530f },
            new OSMImporter.OSMNode { id = 22, lat = 48.3765f, lon = 8.7540f },
            new OSMImporter.OSMNode { id = 23, lat = 48.3760f, lon = 8.7540f }
        };

        foreach (var node in buildingNodes)
            osmData.nodes.Add(node);

        var building1 = new OSMImporter.OSMWay { id = 201 };
        foreach (var node in buildingNodes)
            building1.nodes.Add(node.id);
building1.nodes.Add(20); // Schließe Polygon
        building1.tags["building"] = "yes";
        osmData.ways.Add(building1);

        Debug.Log($"OSM-Testdaten generiert: {osmData.nodes.Count} Nodes, {osmData.ways.Count} Ways");
        return osmData;
    }
}
