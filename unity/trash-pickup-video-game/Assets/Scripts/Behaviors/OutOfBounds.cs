using UnityEngine;

namespace Behaviors
{
    public delegate void BoundaryViolation();
    [RequireComponent(typeof(BoxCollider))]
    public class OutOfBounds : MonoBehaviour
    {
        public event BoundaryViolation BoundaryViolationEvent;

        private void OnCollisionEnter(Collision collision)
        {
            var trash = collision.transform.GetComponent<Trash>();
            if (trash)
            {
                trash.Reset();
                BoundaryViolationEvent?.Invoke();
            }
        }
    }
}