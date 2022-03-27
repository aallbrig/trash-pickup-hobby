using UnityEngine;

namespace Behaviors
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class Trash : MonoBehaviour
    {
        public Data.Trash trashData;
        public void Reset() => gameObject.SetActive(false);
        private void Start() => trashData ??= ScriptableObject.CreateInstance<Data.Trash>();
    }
}