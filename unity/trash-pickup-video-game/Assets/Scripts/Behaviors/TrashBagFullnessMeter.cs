using System;
using UnityEngine;

namespace Behaviors
{
    public class TrashBagFullnessMeter : MonoBehaviour
    {
        public Action MeterUiSyncedEvent;
        public TrashBag trashBag;
        public RectTransform fillBarRectTransform;
        private void Start()
        {
            trashBag ??= FindObjectOfType<TrashBag>();
            #if UNITY_EDITOR
            if (trashBag == null) Debug.LogError("A trash bag is needed for this component");
            #endif

            // Default -- for mostly testing
            fillBarRectTransform ??= new GameObject { transform = { parent = transform } }.AddComponent<RectTransform>();
            SyncFullMeter();

            if (trashBag)
            {
                trashBag.TrashAddEvent += _ => SyncFullMeter();
                trashBag.TrashBagEmptyEvent += _ => SyncFullMeter();
                trashBag.TrashBagFullEvent += SyncFullMeter;
            }
        }
        private void SyncFullMeter()
        {
            fillBarRectTransform.localScale = new Vector3(Mathf.Clamp(trashBag.FullPercentInDecimal(), 0f, 1f), 1, 1);
            MeterUiSyncedEvent?.Invoke();
        }
    }
}