using UnityEngine;

namespace BusSimulator
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class BusController : MonoBehaviour
    {
        [SerializeField] private float acceleration = 7f;
        [SerializeField] private float reverseAcceleration = 4f;
        [SerializeField] private float steeringSpeed = 42f;
        [SerializeField] private float maximumSpeedKph = 70f;
        [SerializeField] private float grip = 4f;

        private Rigidbody busBody;
        private Vector3 spawnPosition;
        private Quaternion spawnRotation;

        public float SpeedKph => Vector3.Dot(busBody.linearVelocity, transform.forward) * 3.6f;

        private void Awake()
        {
            busBody = GetComponent<Rigidbody>();
            spawnPosition = transform.position;
            spawnRotation = transform.rotation;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetBus();
            }
        }

        private void FixedUpdate()
        {
            float throttle = Input.GetAxisRaw("Vertical");
            float steering = Input.GetAxisRaw("Horizontal");
            float forwardSpeed = Vector3.Dot(busBody.linearVelocity, transform.forward);
            float requestedAcceleration = throttle >= 0f ? acceleration : reverseAcceleration;

            if (Mathf.Abs(SpeedKph) < maximumSpeedKph || Mathf.Sign(throttle) != Mathf.Sign(forwardSpeed))
            {
                busBody.AddForce(transform.forward * (throttle * requestedAcceleration), ForceMode.Acceleration);
            }

            float steeringAuthority = Mathf.Clamp01(Mathf.Abs(forwardSpeed) / 2f);
            float direction = forwardSpeed < -0.1f ? -1f : 1f;
            busBody.MoveRotation(busBody.rotation * Quaternion.Euler(0f, steering * steeringSpeed * steeringAuthority * direction * Time.fixedDeltaTime, 0f));

            Vector3 localVelocity = transform.InverseTransformDirection(busBody.linearVelocity);
            localVelocity.x = Mathf.MoveTowards(localVelocity.x, 0f, grip * Time.fixedDeltaTime);
            busBody.linearVelocity = transform.TransformDirection(localVelocity);

            if (Input.GetKey(KeyCode.Space))
            {
                busBody.linearVelocity = Vector3.MoveTowards(busBody.linearVelocity, Vector3.zero, 18f * Time.fixedDeltaTime);
            }
        }

        private void ResetBus()
        {
            busBody.position = spawnPosition;
            busBody.rotation = spawnRotation;
            busBody.linearVelocity = Vector3.zero;
            busBody.angularVelocity = Vector3.zero;
        }
    }
}
