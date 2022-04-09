using System.Collections;
using Behaviors;
using Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Behaviors
{
    public class ScoreAdditionsTests
    {
        private class TestTrash : ITrash
        {
            public TestTrash(float weightInGallons = 1f, float score = 1f)
            {
                WeightAddInGallons = weightInGallons;
                Score = score;
            }
            public float WeightAddInGallons { get; }
            public float Score { get; }
            public AudioClip CollectSound => null;
        }
        [UnityTest]
        public IEnumerator ScoreAdditionsDisplaysScoreOnTrashAdd()
        {
            var sut = new GameObject().AddComponent<ScoreAdditions>();
            var testTrashbag = new GameObject().AddComponent<Trashbag>();
            var text = "";
            sut.ScoreTextDisplayedEvent += displayText => text = displayText;
            sut.trashbag = testTrashbag;
            yield return null;

            testTrashbag.Add(new TestTrash(1f, 1f));

            Assert.AreEqual("+ 1 points\n+ 1 weight", text);
        }

        [UnityTest]
        public IEnumerator ScoreAdditionsDisplaysScoreOnTrashEmpty()
        {
            var sut = new GameObject().AddComponent<ScoreAdditions>();
            var testTrashbag = new GameObject().AddComponent<Trashbag>();
            var text = "";
            sut.ScoreTextDisplayedEvent += displayText => text = displayText;
            sut.trashbag = testTrashbag;
            yield return null;

            testTrashbag.Add(new TestTrash(0f, 1f));
            testTrashbag.Add(new TestTrash(0f, 1f));
            testTrashbag.Add(new TestTrash(0f, 1f));
            testTrashbag.Empty();

            Assert.AreEqual("+ 3 points", text);
        }
    }
}