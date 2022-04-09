using System.Collections;
using Behaviors;
using Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Behaviors
{
    public class TrashbagMonobehaviourTests
    {

        [UnityTest]
        public IEnumerator TrashbagCanGainTrash()
        {
            var sut = new GameObject().AddComponent<Trashbag>();
            var eventCalled = false;
            sut.TrashAddEvent += _ => eventCalled = true;
            yield return null;

            sut.Add(new TestTrash());

            Assert.IsTrue(eventCalled);
        }

        [UnityTest]
        public IEnumerator TrashbagCanBeFilledToFull()
        {
            var sut = new GameObject().AddComponent<Trashbag>();
            var eventCalled = false;
            sut.trashbagCapacityInGallons = 1f;
            sut.TrashbagFullEvent += () => eventCalled = true;
            yield return null;

            sut.Add(new TestTrash());

            Assert.IsTrue(eventCalled);
        }

        [UnityTest]
        public IEnumerator TrashbagCanBeEmptied()
        {
            var sut = new GameObject().AddComponent<Trashbag>();
            var eventCalled = false;
            sut.TrashbagEmptyEvent += _ => eventCalled = true;
            yield return null;

            sut.Add(new TestTrash());
            sut.Empty();

            Assert.IsTrue(eventCalled);
        }

        private class TestTrash : ITrash
        {
            public TestTrash(float weight = 1f, float score = 1f)
            {
                WeightAddInGallons = weight;
                Score = score;
            }

            public float WeightAddInGallons { get; }

            public float Score { get; }

            public AudioClip CollectSound => null;
        }
    }
}