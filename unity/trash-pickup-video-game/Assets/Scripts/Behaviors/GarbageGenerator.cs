using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Behaviors
{
    public delegate void GarbageGenerated(Transform transform);

    // 😡 Curses to the apathy (or whatever it is) that lead people to litter
    public class GarbageGenerator : MonoBehaviour
    {
        public Camera camera;
        public List<GameObject> trashPrefabs = new List<GameObject>();

        [Range(0f, 5f)] public float generatorPadding = 5f;
        [Range(0.1f, 2.0f)] public float minimumSpawnTimeInSeconds = 0.25f;

        [Range(0.1f, 2.0f)] public float maximumSpawnTimeInSeconds = 1.0f;
        [Range(10f, 20f)] public float minimumUpwardVelocity = 10f;
        [Range(10f, 20f)] public float maximumUpwardVelocity = 10f;

        private Coroutine _coroutine;

        private readonly Dictionary<GameObject, List<Transform>>
            _objectPool = new Dictionary<GameObject, List<Transform>>(); // 💦

        private readonly int _poolSize = 10;
        public GarbageGenerated GarbageGeneratedEvent;
        private bool _generate;

        private void Start()
        {
            camera ??= Camera.main;
            if (camera == null)
            {
                #if UNITY_EDITOR
                Debug.LogError("A garbage generator needs a camera to know where to spawn garbage");
                #endif
            }
            FillObjectPool();
            _coroutine = StartCoroutine(RepeatedlySpawnTrash());
        }

        private void OnDestroy() => StopCoroutine(_coroutine);

        private GameObject RandomTrashPrefab() =>
            trashPrefabs.Count > 0 ? trashPrefabs[Random.Range(0, trashPrefabs.Count)] : null;

        private void SpawnTrash()
        {
            if (_generate == false) return;
            var trashPrefab = RandomTrashPrefab();
            if (trashPrefab == null)
            {
                #if UNITY_EDITOR
                Debug.LogWarning("A garbage generator does not have garbage prefabs to use!");
                #endif
                return;
            }

            var trash = GetFromObjectPool(trashPrefab);
            trash.position = new Vector3(DesiredSpawnX(), transform.position.y, transform.position.z);
            var rigidBody = trash.GetComponent<Rigidbody>();
            if (rigidBody)
            {
                rigidBody.AddForce(new Vector3(0, RandomUpwardForce(), 0), ForceMode.VelocityChange);
                rigidBody.AddTorque(trash.transform.up * Random.Range(1f, 180f));
            }

            GarbageGeneratedEvent?.Invoke(trash);
        }
        private float DesiredSpawnX()
        {
            var frustumHeight = 2.0f * DesiredDistance() * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            var frustumWidth = frustumHeight * camera.aspect;
            var halfWidth = frustumWidth * 0.5f;
            var lowerBound = (camera.transform.position.x - halfWidth) + generatorPadding;
            var upperBound = (camera.transform.position.x + halfWidth) - generatorPadding;
            return Random.Range(lowerBound, upperBound);
        }
        private float RandomUpwardForce() => Random.Range(minimumUpwardVelocity, maximumUpwardVelocity);
        private Transform GetFromObjectPool(GameObject trashPrefab)
        {
            var possibleObjects = _objectPool[trashPrefab].Where(trash => trash.gameObject.activeSelf == false).ToList();
            var poolTrash = possibleObjects.Count > 0 ? possibleObjects[0] : new GameObject().transform;
            poolTrash.gameObject.SetActive(true);
            return poolTrash;
        }

        private IEnumerator RepeatedlySpawnTrash()
        {
            while (true)
            {
                SpawnTrash();
                yield return new WaitForSeconds(Random.Range(minimumSpawnTimeInSeconds, maximumSpawnTimeInSeconds));
            }
        }

        private void FillObjectPool() => trashPrefabs.ForEach(trashPrefab =>
        {
            if (!_objectPool.ContainsKey(trashPrefab))
                _objectPool[trashPrefab] = new List<Transform>();

            for (var i = 0; i < _poolSize; i++)
            {
                var trash = Instantiate(trashPrefab, transform);
                trash.SetActive(false);
                _objectPool[trashPrefab].Add(trash.transform);
            }
        });

        private float DesiredDistance()
        {
            // Desired distance = how far is garbage generator from the camera?
            return Vector3.Distance(camera.transform.position, transform.position);
        }
        public void StartGeneration() => _generate = true;
        public void StopGeneration() => _generate = false;
    }
}