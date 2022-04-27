using System;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviors
{
    public class TrashBagFullnessMeter : MonoBehaviour
    {
        public Action MeterUiSyncedEvent;
        public TrashBag trashBag;
        public RectTransform fillBarRectTransform;
        public RawImage fillBarImage;
        public Color fillingColor = new Color(0.1976986f, 1, 0, 1);
        public Color fullColor = new Color(101f/255f, 40f/255f, 40f/255f, 255);
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
                trashBag.TrashBagHasResetEvent += SyncFullMeter;
            }

            if (fillBarImage)
            {
                fillBarImage.color = fillingColor;
                trashBag.TrashBagFullEvent += () => fillBarImage.color = fullColor;
                trashBag.TrashBagEmptyEvent += _ => fillBarImage.color = fillingColor;
                trashBag.TrashBagHasResetEvent += () => fillBarImage.color = fillingColor;
            }
        }
        private void SyncFullMeter()
        {
            fillBarRectTransform.localScale = new Vector3(Mathf.Clamp(trashBag.FullPercentInDecimal(), 0f, 2f), 1, 1);
            MeterUiSyncedEvent?.Invoke();
        }
    }
}