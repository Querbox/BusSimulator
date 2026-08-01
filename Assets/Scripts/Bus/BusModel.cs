using UnityEngine;

/// <summary>
/// 3D-Modell und Controller für verschiedene Bustypen
/// </summary>
public class BusModel : MonoBehaviour
{
    [System.Serializable]
    public class BusConfig
    {
        public string busType; // "standard", "articulated", "minibus"
        public float length = 12f; // Meter
        public float width = 2.5f;
        public float height = 3.5f;
        public int maxPassengers = 50;
        public float maxSpeed = 120f; // km/h
    }

    [SerializeField] private BusConfig config = new BusConfig();
    private GameObject busBody;
    private GameObject[] wheels;

    private void Start()
    {
        config ??= new BusConfig();
        GenerateBusModel();
    }

    private void GenerateBusModel()
    {
        // Erstelle Bus-Body als Quader
        busBody = GameObject.CreatePrimitive(PrimitiveType.Cube);
        busBody.name = "BusBody";
        busBody.transform.SetParent(transform, false);
        busBody.transform.localPosition = new Vector3(0f, config.height * 0.5f, 0f);
        busBody.transform.localScale = new Vector3(config.width, config.height, config.length);

        // Material für Bus-Body
        Material busMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        busMaterial.color = new Color(1f, 0.2f, 0.2f); // Rot
        busBody.GetComponent<Renderer>().material = busMaterial;

        // Erstelle Räder
        wheels = new GameObject[4];
        CreateWheels();

        // Fenster hinzufügen
        AddWindows();

        // Türe hinzufügen
        AddDoor();

        Debug.Log($"Bus-Modell erstellt: {config.busType} ({config.length}m lang)");
    }

    private void CreateWheels()
    {
        float wheelRadius = 0.5f;
        float wheelWidth = 0.3f;
        
        // Rad-Positionen: 2 vorne, 2 hinten
        Vector3[] wheelPositions = new Vector3[]
        {
            new Vector3(-config.width / 2 - wheelWidth / 2, 0.5f, config.length / 3),    // Vorne links
            new Vector3(config.width / 2 + wheelWidth / 2, 0.5f, config.length / 3),     // Vorne rechts
            new Vector3(-config.width / 2 - wheelWidth / 2, 0.5f, -config.length / 3),   // Hinten links
            new Vector3(config.width / 2 + wheelWidth / 2, 0.5f, -config.length / 3)     // Hinten rechts
        };

        for (int i = 0; i < 4; i++)
        {
            wheels[i] = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            wheels[i].name = $"Wheel_{i}";
            wheels[i].transform.SetParent(transform, false);
            wheels[i].transform.localPosition = wheelPositions[i];
            wheels[i].transform.localScale = new Vector3(wheelRadius * 2, wheelWidth, wheelRadius * 2);
            wheels[i].transform.rotation = Quaternion.Euler(90, 0, 0);

            Material wheelMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            wheelMaterial.color = Color.black;
            wheels[i].GetComponent<Renderer>().material = wheelMaterial;

            Destroy(wheels[i].GetComponent<Collider>());
        }
    }

    private void AddWindows()
    {
        int windowCount = 4;
        float windowHeight = config.height * 0.6f;
        float windowWidth = config.width * 0.8f;
        
        for (int i = 0; i < windowCount; i++)
        {
            GameObject window = GameObject.CreatePrimitive(PrimitiveType.Quad);
            window.name = $"Window_{i}";
            window.transform.SetParent(transform, false);
            
            float zPos = config.length / 2 - (i + 1) * (config.length / (windowCount + 1));
            window.transform.localPosition = new Vector3(0, config.height / 2 - 0.2f, zPos);
            window.transform.localScale = new Vector3(windowWidth, windowHeight * 0.5f, 1);
            window.transform.rotation = Quaternion.identity;

            Material windowMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            windowMaterial.color = new Color(0.3f, 0.7f, 1f); // Hellblau
            window.GetComponent<Renderer>().material = windowMaterial;

            Destroy(window.GetComponent<Collider>());
        }
    }

    private void AddDoor()
    {
        GameObject door = GameObject.CreatePrimitive(PrimitiveType.Cube);
        door.name = "Door";
        door.transform.SetParent(transform, false);
        door.transform.localPosition = new Vector3(config.width / 2 + 0.05f, config.height / 4, config.length / 4);
        door.transform.localScale = new Vector3(0.1f, config.height / 2, 1f);

        Material doorMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        doorMaterial.color = new Color(0.2f, 0.2f, 0.2f); // Dunkelgrau
        door.GetComponent<Renderer>().material = doorMaterial;

        Destroy(door.GetComponent<Collider>());
    }

    public BusConfig GetConfig()
    {
        return config;
    }
}
