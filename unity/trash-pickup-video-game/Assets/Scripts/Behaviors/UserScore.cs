using System.Collections.Generic;
using System.Linq;
using Models;
using TMPro;
using UnityEngine;

namespace Behaviors
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UserScore : MonoBehaviour
    {
        public MainMenu mainMenu;
        public TrashBag trashBag;
        private TextMeshProUGUI _displayText;

        public float CurrentScore { get; private set; }

        private void Start()
        {
            _displayText = GetComponent<TextMeshProUGUI>();
            trashBag ??= FindObjectOfType<TrashBag>();
            trashBag ??= new GameObject().AddComponent<TrashBag>();
            mainMenu ??= FindObjectOfType<MainMenu>();

            trashBag.TrashAddEvent += OnTrashAdd;
            trashBag.TrashBagEmptyEvent += OnTrashBagEmpty;

            if (mainMenu) mainMenu.PlayButtonPressedEvent += ResetScore;

            SyncDisplayText();
        }

        private void ResetScore()
        {
            CurrentScore = 0;
            SyncDisplayText();
        }
        private void OnTrashAdd(ITrash trash)
        {
            CurrentScore += 5.0f;
            SyncDisplayText();
        }
        private void SyncDisplayText() => _displayText.text = $"Score: {CurrentScore}";
        private void OnTrashBagEmpty(List<ITrash> emptiedTrash)
        {
            CurrentScore +=
                emptiedTrash.Select(trash => trash.Score).Aggregate((score1, score2) => score1 + score2);
            SyncDisplayText();
        }
    }
}