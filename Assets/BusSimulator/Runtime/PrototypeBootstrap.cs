using UnityEngine;

namespace BusSimulator
{
    /// <summary>
    /// Owns the first playable slice: start menu, generated test world and in-game HUD.
    /// Keeping the bootstrap self-contained makes the checked-in start scene immediately playable.
    /// </summary>
    public sealed class PrototypeBootstrap : MonoBehaviour
    {
        private BusController bus;
        private FollowCamera followCamera;
        private GUIStyle titleStyle;
        private GUIStyle subtitleStyle;
        private GUIStyle speedStyle;
        private GUIStyle labelStyle;
        private GUIStyle buttonStyle;
        private bool worldCreated;
        private bool isPlaying;
        private bool isPaused;

        private void Awake()
        {
            followCamera = CreateCamera();
            Time.timeScale = 0f;
        }

        private void Update()
        {
            if (!isPlaying || !Input.GetKeyDown(KeyCode.Escape)) return;

            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
        }

        private void StartDrive()
        {
            if (!worldCreated)
            {
                CreateEnvironment();
                bus = CreateBus();
                followCamera.Initialize(bus.transform, new Vector3(0f, 4.8f, -11f));
                worldCreated = true;
            }

            isPlaying = true;
            isPaused = false;
            Time.timeScale = 1f;
        }

        private void ReturnToMenu()
        {
            isPlaying = false;
            isPaused = false;
            Time.timeScale = 0f;
        }

        private static void CreateEnvironment()
        {
            RenderSettings.ambientLight = new Color(0.55f, 0.6f, 0.68f);

            var sun = new GameObject("Sun").AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.intensity = 1.2f;
            sun.transform.rotation = Quaternion.Euler(48f, -35f, 0f);

            CreateBlock("Ground", new Vector3(0f, -0.55f, 30f), new Vector3(100f, 1f, 220f), new Color(0.18f, 0.42f, 0.2f));
            CreateBlock("Road", new Vector3(0f, 0f, 30f), new Vector3(12f, 0.1f, 180f), new Color(0.12f, 0.13f, 0.15f));

            for (int z = -50; z <= 110; z += 12)
            {
                CreateBlock("Center marking", new Vector3(0f, 0.07f, z), new Vector3(0.15f, 0.02f, 6f), new Color(0.95f, 0.8f, 0.15f), false);
            }

            CreateBusStop(new Vector3(7.5f, 0f, 25f));
            CreateBusStop(new Vector3(-7.5f, 0f, 85f));
        }

        private static void CreateBusStop(Vector3 position)
        {
            CreateBlock("Bus stop platform", position + new Vector3(0f, 0.12f, 0f), new Vector3(3f, 0.2f, 8f), new Color(0.65f, 0.65f, 0.62f));
            CreateBlock("Bus stop pole", position + new Vector3(0f, 1.6f, 0f), new Vector3(0.12f, 3f, 0.12f), new Color(0.2f, 0.2f, 0.2f));
            CreateBlock("Bus stop sign", position + new Vector3(0f, 3.1f, 0f), new Vector3(0.75f, 0.75f, 0.12f), new Color(0.15f, 0.75f, 0.3f));
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
            CreateBusPart(root.transform, "Windshield", new Vector3(0f, 1.35f, 4.01f), new Vector3(2.15f, 1f, 0.03f), new Color(0.08f, 0.16f, 0.22f));

            for (int side = -1; side <= 1; side += 2)
            {
                CreateBusPart(root.transform, "Front wheel", new Vector3(side * 1.28f, -0.2f, 2.5f), new Vector3(0.35f, 1.2f, 1.2f), Color.black);
                CreateBusPart(root.transform, "Rear wheel", new Vector3(side * 1.28f, -0.2f, -2.5f), new Vector3(0.35f, 1.2f, 1.2f), Color.black);
            }

            return root.AddComponent<BusController>();
        }

        private static void CreateBusPart(Transform parent, string partName, Vector3 position, Vector3 scale, Color color)
        {
            GameObject part = CreateBlock(partName, position, scale, color, false);
            part.transform.SetParent(parent, false);
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

        private static FollowCamera CreateCamera()
        {
            Camera camera = new GameObject("Follow Camera").AddComponent<Camera>();
            camera.fieldOfView = 62f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.42f, 0.66f, 0.86f);
            camera.gameObject.AddComponent<AudioListener>();
            return camera.gameObject.AddComponent<FollowCamera>();
        }

        private void OnGUI()
        {
            InitializeStyles();
            float scale = Mathf.Min(Screen.width / 1280f, Screen.height / 720f);
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * Mathf.Max(0.65f, scale));

            if (!isPlaying)
            {
                DrawMainMenu();
                return;
            }

            DrawHud();
            if (isPaused) DrawPauseMenu();
        }

        private void DrawMainMenu()
        {
            GUI.Box(new Rect(0f, 0f, 1280f, 720f), GUIContent.none);
            GUI.Label(new Rect(0f, 135f, 1280f, 80f), "BUS SIMULATOR", titleStyle);
            GUI.Label(new Rect(0f, 215f, 1280f, 40f), "Fahrbarer Prototyp · Meilenstein 1", subtitleStyle);

            string action = worldCreated ? "FAHRT FORTSETZEN" : "FAHRT STARTEN";
            if (GUI.Button(new Rect(465f, 310f, 350f, 64f), action, buttonStyle)) StartDrive();
            GUI.Label(new Rect(390f, 410f, 500f, 110f), "W / S  Beschleunigen & Bremsen\nA / D  Lenken\nLeertaste  Handbremse\nR  Bus zurücksetzen · ESC  Pause", labelStyle);
            GUI.Label(new Rect(0f, 650f, 1280f, 30f), "Unity 6000.5.6f1 · macOS", subtitleStyle);
        }

        private void DrawHud()
        {
            GUI.Box(new Rect(18f, 18f, 315f, 112f), GUIContent.none);
            GUI.Label(new Rect(35f, 28f, 280f, 46f), $"{Mathf.Abs(bus.SpeedKph):0} km/h", speedStyle);
            GUI.Label(new Rect(35f, 78f, 270f, 28f), "WASD · Leertaste · R · ESC", labelStyle);
        }

        private void DrawPauseMenu()
        {
            GUI.Box(new Rect(390f, 180f, 500f, 350f), GUIContent.none);
            GUI.Label(new Rect(390f, 220f, 500f, 65f), "PAUSE", titleStyle);
            if (GUI.Button(new Rect(465f, 325f, 350f, 58f), "WEITERFAHREN", buttonStyle)) StartDrive();
            if (GUI.Button(new Rect(465f, 405f, 350f, 58f), "ZUM HAUPTMENÜ", buttonStyle)) ReturnToMenu();
        }

        private void InitializeStyles()
        {
            if (titleStyle != null) return;

            titleStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 48, fontStyle = FontStyle.Bold, normal = { textColor = Color.white } };
            subtitleStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 18, normal = { textColor = new Color(0.75f, 0.85f, 0.95f) } };
            speedStyle = new GUIStyle(GUI.skin.label) { fontSize = 34, fontStyle = FontStyle.Bold, normal = { textColor = Color.white } };
            labelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 18, normal = { textColor = Color.white } };
            buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 22, fontStyle = FontStyle.Bold };
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }
    }
}
