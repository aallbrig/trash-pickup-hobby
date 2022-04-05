using System;
using System.Collections;
using System.Collections.Generic;
using Behaviors;
using Models;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Behaviors
{
    public class HighScoreScreenTests
    {
        private class TestHighScore : IHighScore
        {
            public TestHighScore(float score = 0f, string initials = "AAA", DateTime dateTime = default)
            {
                Score = score;
                Initials = initials;
                Date = dateTime;
            }

            public float Score { get; }

            public string Initials { get; }

            public DateTime Date { get; }
        }

        [UnityTest]
        public IEnumerator HighScoreScreenRendersHighScores()
        {
            var sut = new GameObject().AddComponent<HighScoreScreen>();
            sut.highScores = new List<IHighScore>
            {
                new TestHighScore(10f, "ACA")
            };
            yield return null;
        }
    }
}