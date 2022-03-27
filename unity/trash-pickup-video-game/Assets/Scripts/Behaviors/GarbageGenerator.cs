using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Behaviors
{
    public delegate void GarbageGenerated();
    // 😡 Curses to the apathy (or whatever it is) that lead people to litter
    public class GarbageGenerator : MonoBehaviour
    {
        public GarbageGenerated GarbageGeneratedEvent;
        public List<GameObject> trashPrefabs = new List<GameObject>();
        [Range(0.1f, 2.0f)]
        public float minimumSpawnTimeInSeconds = 0.25f;
        [Range(0.1f, 2.0f)]
        public float maximumSpawnTimeInSeconds = 1.0f;

        private Coroutine _coroutine;
        private int _poolSize = 10;
        private Dictionary<GameObject, List<Transform>> _objectPool = new Dictionary<GameObject, List<Transform>>(); // 💦

        private GameObject RandomTrashPrefab() => trashPrefabs.Count > 0 ? trashPrefabs[Random.Range(0, trashPrefabs.Count)] : null;

        private void SpawnTrash()
        {
            var trashPrefab = RandomTrashPrefab();
            if (trashPrefab == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("A garbage generator does not have garbage prefabs to use!");
#endif
                return;
            }

            var trash = GetFromObjectPool(trashPrefab);
            trash.position = new Vector3(Random.Range(-7.5f, 7.5f), transform.position.y, transform.position.z);
            var rigidBody = trash.GetComponent<Rigidbody>();
            if (rigidBody) rigidBody.AddForce(new Vector3(0, Random.Range(10, 20), 0), ForceMode.VelocityChange);

            GarbageGeneratedEvent?.Invoke();
        }
        private Transform GetFromObjectPool(GameObject trashPrefab)
        {
            var poolTrash = _objectPool[trashPrefab].Where(trash => trash.gameObject.activeSelf == false).ToList()[0];
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

        private void Start()
        {
            FillObjectPool();
            _coroutine = StartCoroutine(RepeatedlySpawnTrash());
        }

        private void OnDestroy() => StopCoroutine(_coroutine);

        private void FillObjectPool()
        {
            trashPrefabs.ForEach(trashPrefab =>
            {
                if (!_objectPool.ContainsKey(trashPrefab))
                    _objectPool[trashPrefab] = new List<Transform>();

                for (int i = 0; i < _poolSize; i++)
                {
                    var trash = Instantiate(trashPrefab, transform);
                    trash.SetActive(false);
                    _objectPool[trashPrefab].Add(trash.transform);
                }
            });
        }

    }
}