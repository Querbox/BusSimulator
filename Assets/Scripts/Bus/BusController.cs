using UnityEngine;

/// <summary>
/// Kontrolliert die Bus-Mechanik und Fahrdynamik
/// </summary>
public class BusController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 50f; // km/h
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float brakePower = 15f;
    [SerializeField] private float turnSpeed = 120f;
    [SerializeField] private float steeringSensitivity = 2f;
    
    private Rigidbody rb;
    private float currentSpeed = 0f;
    private float currentSteering = 0f;
    private int passengerCount = 0;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("BusController benötigt einen Rigidbody!");
        }
    }
    
    private void Update()
    {
        HandleInput();
        UpdateBusMovement();
    }
    
    private void HandleInput()
    {
        // Beschleunigung / Bremse
        float throttle = InputSystemControls.GetThrottle();
        float brake = InputSystemControls.IsBrakePressed() ? 1f : 0f;
        
        if (throttle > 0)
        {
            currentSpeed = Mathf.Min(currentSpeed + throttle * acceleration * Time.deltaTime, maxSpeed);
        }
        else if (throttle < 0 || brake > 0)
        {
            currentSpeed = Mathf.Max(currentSpeed - brakePower * Time.deltaTime, 0f);
        }
        else
        {
            // Natürliches Abbremsen
            currentSpeed = Mathf.Max(currentSpeed - 2f * Time.deltaTime, 0f);
        }
        
        // Lenkung
        currentSteering = InputSystemControls.GetSteering() * steeringSensitivity;
    }
    
    private void UpdateBusMovement()
    {
        if (rb == null) return;
        
        // Bewegung nach vorne
        Vector3 moveDirection = transform.forward * currentSpeed * Time.deltaTime;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, moveDirection.z);
        
        // Rotation
        float rotationAmount = currentSteering * turnSpeed * Time.deltaTime;
        transform.Rotate(0, rotationAmount, 0);
    }
    
    public void AddPassenger()
    {
        passengerCount++;
        Debug.Log($"Fahrgast hinzugefügt. Aktuelle Anzahl: {passengerCount}");
    }
    
    public void RemovePassenger()
    {
        if (passengerCount > 0)
        {
            passengerCount--;
            Debug.Log($"Fahrgast entfernt. Aktuelle Anzahl: {passengerCount}");
        }
    }
    
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
    
    public int GetPassengerCount()
    {
        return passengerCount;
    }
}
