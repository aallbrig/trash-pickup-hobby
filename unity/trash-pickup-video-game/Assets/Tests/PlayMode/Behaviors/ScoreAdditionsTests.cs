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
            public float WeightAddInGallons => 25.0f;

            public float Score => 1.0f;

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
            
            testTrashbag.Add(new TestTrash());
            
            Assert.AreEqual("+ 1 points\n+ 25 weight", text);
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
            
            testTrashbag.Add(new TestTrash());
            testTrashbag.Add(new TestTrash());
            testTrashbag.Add(new TestTrash());
            testTrashbag.Empty();
            
            Assert.AreEqual("+ 3 points", text);
        }
    }
}