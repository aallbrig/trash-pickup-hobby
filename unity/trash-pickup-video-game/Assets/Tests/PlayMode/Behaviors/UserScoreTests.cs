using System.Collections;
using Behaviors;
using Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Behaviors
{
    public class UserScoreTests
    {

        [UnityTest]
        public IEnumerator UserScoreIncreasesWhenTrashIsAdded()
        {
            var testTrashbag = new GameObject().AddComponent<Trashbag>();
            var sut = new GameObject().AddComponent<UserScore>();
            yield return null;

            testTrashbag.Add(new TestTrash());

            Assert.AreEqual(5.0f, sut.CurrentScore);
        }

        [UnityTest]
        public IEnumerator UserScoreIncreasesWhenTrashIsEmptied()
        {
            var testTrashbag = new GameObject().AddComponent<Trashbag>();
            var sut = new GameObject().AddComponent<UserScore>();
            yield return null;

            testTrashbag.Add(new TestTrash());
            testTrashbag.Empty();

            Assert.AreEqual(5.0f + 5.0f, sut.CurrentScore);
        }

        public class TestTrash : ITrash
        {
            public float WeightAddInGallons => 15.0f;

            public float Score => 5.0f;
        }
    }
}