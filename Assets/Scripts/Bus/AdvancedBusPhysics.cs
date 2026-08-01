using UnityEngine;

/// <summary>
/// Verbesserte Bus-Physik mit Straßen-Erkennung und realistischem Verhalten
/// </summary>
public class AdvancedBusPhysics : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 80f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float brakePower = 20f;
    [SerializeField] private float maxSteeringAngle = 30f;
    [SerializeField] private float steeringSensitivity = 1.5f;
    [SerializeField] private float friction = 3f;
    [SerializeField] private float roadFriction = 0.5f; // Reibung auf Straßen (weniger)
    [SerializeField] private float offroadFriction = 5f; // Reibung off-road (mehr)
    [SerializeField] private float wheelbase = 8f; // Achsabstand für realistisches Lenkverhalten
    [SerializeField] private float maxTiltAngle = 5f;
    [SerializeField] private LayerMask roadLayer;
    
    private Rigidbody rb;
    private float currentSpeed = 0f;
    private float currentSteering = 0f;
    private Vector3 currentDirection = Vector3.forward;
    private int passengerCount = 0;
    private bool engineRunning = true;
    private bool onRoad = false;
    private float currentFriction = 3f;

    private void Start()
    {
        currentFriction = friction;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = 12000f;
            rb.linearDamping = 0.1f;
            rb.angularDamping = 0.1f;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void Update()
    {
        HandleInput();
        CheckIfOnRoad();
    }

    private void FixedUpdate()
    {
        UpdateBusMovement();
        ApplyPhysics();
    }

    private void HandleInput()
    {
        float throttle = InputSystemControls.GetThrottle();
        bool braking = InputSystemControls.IsBrakePressed();

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
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, currentFriction * Time.deltaTime);
        }

        // Lenkung mit Ackermann-Geometrie (realistisches Lenkverhalten)
        currentSteering = InputSystemControls.GetSteering() * steeringSensitivity;

        if (InputSystemControls.WasEngineTogglePressed())
        {
            engineRunning = !engineRunning;
            Debug.Log(engineRunning ? "Motor läuft" : "Motor aus");
        }
    }

    private void CheckIfOnRoad()
    {
        // Raycast nach unten um zu prüfen ob Bus auf Straße ist
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            // TODO: Straßen-Layer implementieren
            onRoad = true;
            currentFriction = roadFriction;
        }
        else
        {
            onRoad = false;
            currentFriction = offroadFriction; // Höhere Reibung off-road
        }
    }

    private void UpdateBusMovement()
    {
        if (!engineRunning || rb == null) return;

        // Ackermann-Lenkgeometrie für realistisches Verhalten
        float steeringAngle = currentSteering * maxSteeringAngle;
        float turningRadius = wheelbase / Mathf.Tan(steeringAngle * Mathf.Deg2Rad);

        // Bewegung
        Vector3 moveDirection = transform.forward * currentSpeed * Time.fixedDeltaTime / 3.6f;
        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);

        // Rotation mit Ackermann-Geometrie
        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            float rotationAmount = (currentSpeed / turningRadius) * Time.fixedDeltaTime;
            transform.Rotate(0, rotationAmount * Mathf.Rad2Deg, 0);
        }

        // Bus neigt sich bei Kurven
        float tiltAmount = Mathf.Clamp(currentSteering * -maxTiltAngle, -maxTiltAngle, maxTiltAngle);
        Quaternion targetRotation = Quaternion.Euler(tiltAmount * (currentSpeed / maxSpeed), transform.eulerAngles.y, tiltAmount);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 2f);
    }

    private void ApplyPhysics()
    {
        // Speed-Limitierung basierend auf Untergrund
        if (!onRoad)
        {
            currentSpeed *= 0.95f; // Off-road langsamer
        }
    }

    public float GetCurrentSpeed() => currentSpeed;
    public int GetPassengerCount() => passengerCount;
    public bool IsEngineRunning() => engineRunning;
    public bool IsOnRoad() => onRoad;

    public void AddPassenger()
    {
        passengerCount++;
        if (rb != null) rb.mass += 75f;
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
