using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Komplettes HUD-System für den Bus-Simulator
/// Zeigt Geschwindigkeit, Navigation, Fahrgäste, Uhrzeit, etc.
/// </summary>
public class HUDSystem : MonoBehaviour
{
    [SerializeField] private BusPhysicsController busController;
    [SerializeField] private NavigationSystem navigationSystem;
    [SerializeField] private Canvas hudCanvas;

    // UI Elements
    private Text speedText;
    private Text rpmText;
    private Text fuelText;
    private Text passengerText;
    private Text timeText;
    private Text nextStopText;
    private Text distanceText;
    private Image speedometer;
    private Image navCompass;
    private Image fuelBar;

    private float gameTime = 0f;
    private int currentHour = 8;
    private int currentMinute = 0;

    private void Start()
    {
        CreateHUDElements();
    }

    private void CreateHUDElements()
    {
        if (hudCanvas == null)
        {
            GameObject canvasObj = new GameObject("HUDCanvas");
            hudCanvas = canvasObj.AddComponent<Canvas>();
            hudCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        }

        // === LINKE SEITE: Geschwindigkeit & RPM ===
        GameObject speedPanelObj = CreatePanel("SpeedPanel", new Vector2(-300, -150), new Vector2(250, 200));
        speedPanelObj.transform.parent = hudCanvas.transform;
        Image speedPanelBg = speedPanelObj.GetComponent<Image>();
        speedPanelBg.color = new Color(0, 0, 0, 0.7f);

        // Speedometer Kreis
        speedometer = speedPanelObj.AddComponent<Image>();
        speedometer.color = new Color(0.2f, 0.2f, 0.2f);
        speedText = CreateText(speedPanelObj, "Geschwindigkeit: 0 km/h", new Vector2(0, 30), 28);
        rpmText = CreateText(speedPanelObj, "RPM: 0", new Vector2(0, -20), 20);

        // === RECHTE SEITE: Navi & Nächste Haltestelle ===
        GameObject naviPanelObj = CreatePanel("NaviPanel", new Vector2(300, -150), new Vector2(300, 200));
        naviPanelObj.transform.parent = hudCanvas.transform;
        Image naviPanelBg = naviPanelObj.GetComponent<Image>();
        naviPanelBg.color = new Color(0, 0, 0, 0.7f);

        nextStopText = CreateText(naviPanelObj, "Nächste Haltestelle: --", new Vector2(0, 30), 20);
        distanceText = CreateText(naviPanelObj, "Entfernung: -- m", new Vector2(0, 0), 18);
        navCompass = CreateCompass(naviPanelObj);

        // === OBEN MITTE: Uhrzeit & Fahrgäste ===
        GameObject topPanelObj = CreatePanel("TopPanel", new Vector2(0, 250), new Vector2(400, 80));
        topPanelObj.transform.parent = hudCanvas.transform;
        Image topPanelBg = topPanelObj.GetComponent<Image>();
        topPanelBg.color = new Color(0, 0, 0, 0.7f);

        timeText = CreateText(topPanelObj, "08:00", new Vector2(-100, 0), 32);
        passengerText = CreateText(topPanelObj, "Fahrgäste: 0/50", new Vector2(100, 0), 28);

        // === UNTEN LINKS: Fuel ===
        GameObject fuelPanelObj = CreatePanel("FuelPanel", new Vector2(-300, 250), new Vector2(200, 100));
        fuelPanelObj.transform.parent = hudCanvas.transform;
        Image fuelPanelBg = fuelPanelObj.GetComponent<Image>();
        fuelPanelBg.color = new Color(0, 0, 0, 0.7f);

        CreateText(fuelPanelObj, "FUEL", new Vector2(0, 20), 18);
        fuelBar = CreateProgressBar(fuelPanelObj, new Vector2(0, -10), new Vector2(150, 30));
        fuelText = CreateText(fuelPanelObj, "100%", new Vector2(0, -35), 16);

        Debug.Log("✓ HUD-System initialisiert");
    }

    private void Update()
    {
        if (busController == null) return;

        // Update Zeit
        gameTime += Time.deltaTime;
        currentMinute = (int)(gameTime / 60f) % 60;
        currentHour = 8 + (int)(gameTime / 3600f);

        // Update Speed
        float speed = busController.GetCurrentSpeed();
        speedText.text = $"Geschwindigkeit: {speed:F1} km/h";
        rpmText.text = $"RPM: {(speed / 120f * 5000):F0}";

        // Update Passengers
        int passengers = busController.GetPassengerCount();
        passengerText.text = $"Fahrgäste: {passengers}/50";

        // Update Time
        timeText.text = $"{currentHour:D2}:{currentMinute:D2}";

        // Update Fuel
        float fuelPercent = 0.8f; // TODO: Connect to real fuel system
        fuelText.text = $"{(fuelPercent * 100):F0}%";
        if (fuelBar != null)
        {
            fuelBar.fillAmount = fuelPercent;
        }

        // Update Navigation
        if (navigationSystem != null)
        {
            var nextStop = navigationSystem.GetNextStop();
            if (nextStop != null)
            {
                nextStopText.text = $"Nächste Haltestelle: {nextStop.name}";
                float distance = navigationSystem.GetDistanceToNextStop();
                distanceText.text = $"Entfernung: {distance:F0} m";

                // Update Compass
                if (navCompass != null)
                {
                    float direction = navigationSystem.GetDirectionToNextStop();
                    navCompass.transform.eulerAngles = new Vector3(0, 0, -direction);
                }
            }
        }
    }

    private GameObject CreatePanel(string name, Vector2 position, Vector2 size)
    {
        GameObject panelObj = new GameObject(name);
        RectTransform rectTransform = panelObj.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = size;
        panelObj.AddComponent<Image>();
        panelObj.AddComponent<CanvasGroup>();
        return panelObj;
    }

    private Text CreateText(GameObject parent, string content, Vector2 position, int fontSize)
    {
        GameObject textObj = new GameObject("Text");
        textObj.transform.parent = parent.transform;
        textObj.transform.localPosition = Vector3.zero;

        RectTransform rectTransform = textObj.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(400, 80);

        Text text = textObj.AddComponent<Text>();
        text.text = content;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = fontSize;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        // Outline
        Outline outline = textObj.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(2, 2);

        return text;
    }

    private Image CreateCompass(GameObject parent)
    {
        GameObject compassObj = new GameObject("Compass");
        compassObj.transform.parent = parent.transform;
        compassObj.transform.localPosition = Vector3.zero;

        RectTransform rectTransform = compassObj.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, -60);
        rectTransform.sizeDelta = new Vector2(100, 100);

        Image image = compassObj.AddComponent<Image>();
        image.color = new Color(0.3f, 0.6f, 1f);

        // Pfeil in der Mitte
        GameObject arrowObj = new GameObject("Arrow");
        arrowObj.transform.parent = compassObj.transform;
        arrowObj.transform.localPosition = Vector3.zero;
        RectTransform arrowRect = arrowObj.AddComponent<RectTransform>();
        arrowRect.sizeDelta = new Vector2(10, 50);
        Image arrowImage = arrowObj.AddComponent<Image>();
        arrowImage.color = Color.red;

        return image;
    }

    private Image CreateProgressBar(GameObject parent, Vector2 position, Vector2 size)
    {
        GameObject barBgObj = new GameObject("ProgressBarBg");
        barBgObj.transform.parent = parent.transform;
        barBgObj.transform.localPosition = Vector3.zero;

        RectTransform barBgRect = barBgObj.AddComponent<RectTransform>();
        barBgRect.anchoredPosition = position;
        barBgRect.sizeDelta = size;
        Image barBgImage = barBgObj.AddComponent<Image>();
        barBgImage.color = new Color(0.3f, 0.3f, 0.3f);

        GameObject barFillObj = new GameObject("Fill");
        barFillObj.transform.parent = barBgObj.transform;
        barFillObj.transform.localPosition = Vector3.zero;

        RectTransform barFillRect = barFillObj.AddComponent<RectTransform>();
        barFillRect.anchorMin = Vector2.zero;
        barFillRect.anchorMax = new Vector2(1, 1);
        barFillRect.offsetMin = Vector2.zero;
        barFillRect.offsetMax = Vector2.zero;
        Image barFillImage = barFillObj.AddComponent<Image>();
        barFillImage.color = new Color(0.2f, 1f, 0.2f);

        Image barImage = barBgObj.AddComponent<Image>();
        barImage.type = Image.Type.Filled;
        barImage.fillMethod = Image.FillMethod.Horizontal;
        barImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        barImage.fillAmount = 0.8f;

        return barImage;
    }
}
