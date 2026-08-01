using UnityEngine;

namespace BusSimulator
{
    public sealed class PrototypeBootstrap : MonoBehaviour
    {
        private BusController bus;
        private GUIStyle speedStyle;
        private GUIStyle hintStyle;

        private void Awake()
        {
            CreateEnvironment();
            bus = CreateBus();
            CreateCamera(bus.transform);
        }

        private static void CreateEnvironment()
        {
            RenderSettings.ambientLight = new Color(0.55f, 0.6f, 0.68f);

            var sun = new GameObject("Sun").AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.intensity = 1.2f;
            sun.transform.rotation = Quaternion.Euler(48f, -35f, 0f);

            CreateBlock("Ground", new Vector3(0f, -0.55f, 0f), new Vector3(100f, 1f, 220f), new Color(0.18f, 0.42f, 0.2f));
            CreateBlock("Road", new Vector3(0f, 0f, 30f), new Vector3(12f, 0.1f, 180f), new Color(0.12f, 0.13f, 0.15f));

            for (int z = -50; z <= 110; z += 12)
            {
                CreateBlock("Center marking", new Vector3(0f, 0.07f, z), new Vector3(0.15f, 0.02f, 6f), new Color(0.95f, 0.8f, 0.15f), false);
            }
        }

        private static BusController CreateBus()
        {
            GameObject root = new GameObject("Prototype Bus");
            root.transform.position = new Vector3(-2.5f, 1.25f, -45f);

            var body = root.AddComponent<Rigidbody>();
            body.mass = 8500f;
            body.linearDamping = 0.12f;
            body.angularDamping = 3f;
            body.centerOfMass = new Vector3(0f, -0.7f, 0f);
            body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            var collider = root.AddComponent<BoxCollider>();
            collider.center = new Vector3(0f, 0.8f, 0f);
            collider.size = new Vector3(2.5f, 2.5f, 8f);

            GameObject shell = CreateBlock("Bus body", new Vector3(0f, 0.8f, 0f), new Vector3(2.5f, 2.7f, 8f), new Color(0.1f, 0.45f, 0.85f), false);
            shell.transform.SetParent(root.transform, false);
            CreateWindow(root.transform, new Vector3(0f, 1.35f, 4.01f), new Vector3(2.15f, 1f, 0.03f));

            return root.AddComponent<BusController>();
        }

        private static void CreateWindow(Transform parent, Vector3 position, Vector3 scale)
        {
            GameObject window = CreateBlock("Windshield", position, scale, new Color(0.08f, 0.16f, 0.22f), false);
            window.transform.SetParent(parent, false);
        }

        private static GameObject CreateBlock(string objectName, Vector3 position, Vector3 scale, Color color, bool collider = true)
        {
            GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
            block.name = objectName;
            block.transform.SetPositionAndRotation(position, Quaternion.identity);
            block.transform.localScale = scale;
            block.GetComponent<Renderer>().material.color = color;
            if (!collider) Destroy(block.GetComponent<Collider>());
            return block;
        }

        private static void CreateCamera(Transform target)
        {
            Camera camera = new GameObject("Follow Camera").AddComponent<Camera>();
            camera.fieldOfView = 62f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.42f, 0.66f, 0.86f);
            camera.gameObject.AddComponent<AudioListener>();
            camera.gameObject.AddComponent<FollowCamera>().Initialize(target, new Vector3(0f, 4.8f, -11f));
        }

        private void OnGUI()
        {
            speedStyle ??= new GUIStyle(GUI.skin.label) { fontSize = 30, fontStyle = FontStyle.Bold, normal = { textColor = Color.white } };
            hintStyle ??= new GUIStyle(GUI.skin.label) { fontSize = 16, normal = { textColor = Color.white } };
            GUI.Box(new Rect(18f, 18f, 300f, 100f), GUIContent.none);
            GUI.Label(new Rect(35f, 28f, 260f, 40f), $"{Mathf.Abs(bus.SpeedKph):0} km/h", speedStyle);
            GUI.Label(new Rect(35f, 75f, 260f, 25f), "WASD · Leertaste · R", hintStyle);
        }
    }
}
