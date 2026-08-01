using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HUD-Anzeige während des Spiels
/// </summary>
public class HUD : MonoBehaviour
{
    [SerializeField] private Text speedText;
    [SerializeField] private Text passengerText;
    [SerializeField] private Text timeText;
    [SerializeField] private BusController busController;
    
    private void Update()
    {
        if (speedText != null)
        {
            float speed = busController.GetCurrentSpeed();
            speedText.text = $"Geschwindigkeit: {speed:F1} km/h";
        }
        
        if (passengerText != null)
        {
            int passengers = busController.GetPassengerCount();
            passengerText.text = $"Fahrgäste: {passengers}";
        }
        
        if (timeText != null)
        {
            float gameTime = GameManager.Instance.GetGameTime();
            int minutes = (int)(gameTime / 60f);
            int seconds = (int)(gameTime % 60f);
            timeText.text = $"Zeit: {minutes:D2}:{seconds:D2}";
        }
    }
}
