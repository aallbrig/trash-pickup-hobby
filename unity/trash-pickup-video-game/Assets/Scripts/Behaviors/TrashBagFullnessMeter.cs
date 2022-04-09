using System;
using UnityEngine;

namespace Behaviors
{
    public class TrashBagFullnessMeter : MonoBehaviour
    {
        public Action MeterUiSyncedEvent;
        public Trashbag trashbag;
        public RectTransform fillBarRectTransform;
        private void Start()
        {
            trashbag ??= FindObjectOfType<Trashbag>();
            #if UNITY_EDITOR
            if (trashbag == null) Debug.LogError("A trash bag is needed for this component");
            #endif

            // Default -- for mostly testing
            fillBarRectTransform ??= new GameObject { transform = { parent = transform } }.AddComponent<RectTransform>();
            SyncFullMeter();

            if (trashbag)
            {
                trashbag.TrashAddEvent += _ => SyncFullMeter();
                trashbag.TrashbagEmptyEvent += _ => SyncFullMeter();
                trashbag.TrashbagFullEvent += SyncFullMeter;
            }
        }
        private void SyncFullMeter()
        {
            fillBarRectTransform.localScale = new Vector3(Mathf.Clamp(trashbag.FullPercentInDecimal(), 0f, 1f), 1, 1);
            MeterUiSyncedEvent?.Invoke();
        }
    }
}