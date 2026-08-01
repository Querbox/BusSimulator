using UnityEngine;

/// <summary>
/// Zentrale Spielverwaltung und Spielzustände
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private BusController busController;
    [SerializeField] private MapManager mapManager;
    
    private bool isGameRunning = false;
    private float gameTime = 0f;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        StartGame();
    }
    
    private void Update()
    {
        if (isGameRunning)
        {
            gameTime += Time.deltaTime;
            UpdateGameState();
        }
        
        if (InputSystemControls.WasEscapePressed())
        {
            TogglePauseGame();
        }
    }
    
    private void UpdateGameState()
    {
        // Hier können Spiellogik-Updates erfolgen
        // z.B. Haltestellen-Erkennung, Passagier-Management, etc.
    }
    
    public void StartGame()
    {
        isGameRunning = true;
        gameTime = 0f;
        Time.timeScale = 1f;
        Debug.Log("Spiel gestartet!");
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            Time.timeScale = 1f;
        }
    }
    
    public void TogglePauseGame()
    {
        isGameRunning = !isGameRunning;
        Time.timeScale = isGameRunning ? 1f : 0f;
        Debug.Log(isGameRunning ? "Spiel fortgesetzt" : "Spiel pausiert");
    }
    
    public bool IsGameRunning()
    {
        return isGameRunning;
    }
    
    public float GetGameTime()
    {
        return gameTime;
    }
}
