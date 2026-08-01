using UnityEngine;

namespace BusSimulator
{
    public sealed class FollowCamera : MonoBehaviour
    {
        private Transform target;
        private Vector3 offset;

        public void Initialize(Transform newTarget, Vector3 newOffset)
        {
            target = newTarget;
            offset = newOffset;
            transform.position = target.TransformPoint(offset);
        }

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desiredPosition = target.TransformPoint(offset);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 6f * Time.deltaTime);
            transform.LookAt(target.position + Vector3.up * 1.3f);
        }
    }
}
