using UnityEngine;

/// <summary>
/// Verbesserte Bus-Physik und Steuerung
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class BusPhysicsController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 80f; // km/h
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float brakePower = 20f;
    [SerializeField] private float turnSpeed = 90f;
    [SerializeField] private float steeringSensitivity = 1.5f;
    [SerializeField] private float friction = 3f;
    [SerializeField] private float maxTiltAngle = 5f;
    
    private Rigidbody rb;
    private float currentSpeed = 0f;
    private float currentSteering = 0f;
    private int passengerCount = 0;
    private bool engineRunning = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        UpdateBusMovement();
        ApplyPhysics();
    }

    private void HandleInput()
    {
        // Geschwindigkeit
        float throttle = BuiltInInputControls.GetThrottle();
        bool braking = BuiltInInputControls.IsBrakePressed();

        if (throttle > 0.1f && engineRunning)
        {
            currentSpeed = Mathf.Min(currentSpeed + throttle * acceleration * Time.deltaTime, maxSpeed);
        }
        else if (throttle < -0.1f && engineRunning)
        {
            currentSpeed = Mathf.Max(currentSpeed - Mathf.Abs(throttle) * acceleration * 0.5f * Time.deltaTime, -maxSpeed * 0.3f);
        }
        else if (braking)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brakePower * Time.deltaTime);
        }
        else
        {
            // Reibung
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, friction * Time.deltaTime);
        }

        // Lenkung
        currentSteering = BuiltInInputControls.GetSteering() * steeringSensitivity;

        // Motor Start/Stop
        if (BuiltInInputControls.WasEngineTogglePressed())
        {
            engineRunning = !engineRunning;
            Debug.Log(engineRunning ? "Motor läuft" : "Motor aus");
        }
    }

    private void UpdateBusMovement()
    {
        if (rb == null) return;

        // Bewegung
        Vector3 moveVelocity = transform.forward * (currentSpeed / 3.6f); // km/h zu m/s
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);

        // Rotation
        float rotationAmount = currentSteering * turnSpeed * Time.fixedDeltaTime * (currentSpeed / maxSpeed);
        // Bus neigt sich bei Kurven
        float tiltAmount = Mathf.Clamp(currentSteering * -maxTiltAngle, -maxTiltAngle, maxTiltAngle);
        Quaternion steeringRotation = rb.rotation * Quaternion.Euler(0f, rotationAmount, 0f);
        Quaternion targetRotation = Quaternion.Euler(0f, steeringRotation.eulerAngles.y, tiltAmount);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 2f));
    }

    private void ApplyPhysics()
    {
        // Zusätzliche Physik-Effekte können hier hinzugefügt werden
        // z.B. Suspension, Rollover-Prävention, etc.
    }

    public float GetCurrentSpeed() => currentSpeed;
    public int GetPassengerCount() => passengerCount;
    public bool IsEngineRunning() => engineRunning;

    public void AddPassenger()
    {
        passengerCount++;
        // Gewicht erhöht sich
        if (rb != null) rb.mass += 75f; // Durchschnittliches Fahrgast-Gewicht
    }

    public void RemovePassenger()
    {
        if (passengerCount > 0)
        {
            passengerCount--;
            if (rb != null) rb.mass -= 75f;
        }
    }
}
