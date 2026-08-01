using UnityEngine;

/// <summary>
/// Kontrolliert die Bus-Mechanik und Fahrdynamik
/// </summary>
[RequireComponent(typeof(Rigidbody))]
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
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate() => UpdateBusMovement();
    
    private void HandleInput()
    {
        // Beschleunigung / Bremse
        float throttle = BuiltInInputControls.GetThrottle();
        float brake = BuiltInInputControls.IsBrakePressed() ? 1f : 0f;
        
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
        currentSteering = BuiltInInputControls.GetSteering() * steeringSensitivity;
    }
    
    private void UpdateBusMovement()
    {
        if (rb == null) return;
        
        // Bewegung nach vorne
        Vector3 moveVelocity = transform.forward * (currentSpeed / 3.6f);
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
        
        // Rotation
        float rotationAmount = currentSteering * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, rotationAmount, 0f));
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
