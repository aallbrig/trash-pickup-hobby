using System;
using UnityEngine;

namespace Behaviors
{
    public delegate void BoundaryViolation();
    [RequireComponent(typeof(BoxCollider))]
    public class OutOfBounds : MonoBehaviour
    {
        public event BoundaryViolation BoundaryViolationEvent;
        public BoxCollider collider;

        private void Start()
        {
            collider ??= GetComponent<BoxCollider>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            var trash = collision.transform.GetComponent<Trash>();
            if (trash)
            {
                trash.Reset();
                BoundaryViolationEvent?.Invoke();
            }
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, collider.size);
        }
#endif
    }
}