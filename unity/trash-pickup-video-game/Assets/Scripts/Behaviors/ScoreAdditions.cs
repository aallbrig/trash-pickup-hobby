using System.Collections.Generic;
using System.Linq;
using Models;
using TMPro;
using UnityEngine;

namespace Behaviors
{
    public delegate void ScoreTextHidden();
    public delegate void ScoreTextDisplayed(string displayText);
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScoreAdditions : MonoBehaviour
    {
        public ScoreTextDisplayed ScoreTextDisplayedEvent;
        public ScoreTextHidden ScoreTextHiddenEvent;
        public Trashbag trashbag;
        public float displayTimeSeconds = 1.5f;
        private TextMeshProUGUI _text;
        [SerializeField] private float scoreAddDisplayedTime;
        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();

            trashbag ??= FindObjectOfType<Trashbag>();
            
            trashbag.TrashAddEvent += OnTrashAdd;
            trashbag.TrashbagEmptyEvent += OnTrashEmpty;
        }
        private void Update()
        {
            if (scoreAddDisplayedTime == 0) return;
            if (Time.time - scoreAddDisplayedTime > displayTimeSeconds)
            {
                HideText();
                scoreAddDisplayedTime = 0;
            }
        }
        private void OnTrashEmpty(List<ITrash> emptiedtrash)
        {
            DisplayText();
            var totalScore = emptiedtrash.Select(_ => _.Score).Aggregate(0f, (f1, f2) => f1 + f2);
            var displayText = $"+ {totalScore} points";
            scoreAddDisplayedTime = Time.time;
            _text.SetText(displayText);
            ScoreTextDisplayedEvent?.Invoke(displayText);
        }
        private void OnTrashAdd(ITrash trash)
        {
            DisplayText();
            // Text appears on screen that shows the points gained
            var displayText = $"+ {trash.Score} points\n+ {trash.WeightAddInGallons} weight";
            scoreAddDisplayedTime = Time.time;
            _text.SetText(displayText);
            ScoreTextDisplayedEvent?.Invoke(displayText);
        }
        private void HideText()
        {
            // if (_text.enabled == false) return;
            // _text.enabled = false;
            _text.SetText("");
            ScoreTextHiddenEvent?.Invoke();
        }
        private void DisplayText()
        {
            if (_text.enabled) return;
            _text.enabled = true;
        }
    }
}