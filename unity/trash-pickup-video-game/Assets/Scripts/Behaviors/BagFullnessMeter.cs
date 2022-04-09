using System;
using UnityEngine;

namespace Behaviors
{
    public class BagFullnessMeter : MonoBehaviour
    {
        public Action MeterUiSyncedEvent;
        public Trashbag trashbag;
        private void Start()
        {
            trashbag ??= FindObjectOfType<Trashbag>();
            #if UNITY_EDITOR
            Debug.Assert(trashbag != null, "A trash bag is needed for this component");
            #endif
            if (trashbag)
            {
                trashbag.TrashAddEvent += _ => SyncFullMeter();
                trashbag.TrashbagEmptyEvent += _ => SyncFullMeter();
                trashbag.TrashbagFullEvent += SyncFullMeter;
            }
        }
        private void SyncFullMeter()
        {
            MeterUiSyncedEvent?.Invoke();
        }
    }
}