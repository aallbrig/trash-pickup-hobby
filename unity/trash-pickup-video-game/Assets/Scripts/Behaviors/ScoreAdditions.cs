using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using TMPro;
using UnityEngine;

namespace Behaviors
{
    public delegate void ScoreAddDisplayed(string displayText);
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScoreAdditions : MonoBehaviour
    {
        public ScoreAddDisplayed ScoreAddDisplayedEvent;
        public Trashbag trashbag;
        private TextMeshProUGUI _displayText;
        private void Start()
        {
            _displayText = GetComponent<TextMeshProUGUI>();

            trashbag ??= FindObjectOfType<Trashbag>();
            
            trashbag.TrashAddEvent += OnTrashAdd;
            trashbag.TrashbagEmptyEvent += OnTrashEmpty;
        }
        private void OnTrashEmpty(List<ITrash> emptiedtrash)
        {
            var totalScore = emptiedtrash.Select(_ => _.Score).Aggregate(0f, (f1, f2) => f1 + f2);
            var displayText = $"+ {totalScore} points";
            _displayText.text = displayText;
            ScoreAddDisplayedEvent?.Invoke(displayText);
        }
        private void OnTrashAdd(ITrash trash)
        {
            // Text appears on screen that shows the points gained
            var displayText = $"+ {trash.Score} points\n+ {trash.WeightAddInGallons} weight";
            _displayText.text = displayText;
            ScoreAddDisplayedEvent?.Invoke(displayText);
        }
    }
}