using System.Collections;
using Behaviors;
using Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Behaviors
{
    public class TrashBagMonobehaviourTests
    {

        [UnityTest]
        public IEnumerator TrashBagCanGainTrash()
        {
            var sut = new GameObject().AddComponent<TrashBag>();
            var eventCalled = false;
            sut.TrashAddEvent += _ => eventCalled = true;
            yield return null;

            sut.Add(new TestTrash());

            Assert.IsTrue(eventCalled);
        }

        [UnityTest]
        public IEnumerator TrashBagCanBeFilledToFull()
        {
            var sut = new GameObject().AddComponent<TrashBag>();
            var eventCalled = false;
            sut.trashBagCapacityInGallons = 1f;
            sut.TrashBagFullEvent += () => eventCalled = true;
            yield return null;

            sut.Add(new TestTrash());

            Assert.IsTrue(eventCalled);
        }

        [UnityTest]
        public IEnumerator TrashBagCanBeEmptied()
        {
            var sut = new GameObject().AddComponent<TrashBag>();
            var eventCalled = false;
            sut.TrashBagEmptyEvent += _ => eventCalled = true;
            yield return null;

            sut.Add(new TestTrash());
            sut.Empty();

            Assert.IsTrue(eventCalled);
        }

        [UnityTest]
        public IEnumerator TrashBagCannotAcceptTrashOnceFull()
        {
            var sut = new GameObject().AddComponent<TrashBag>();
            sut.trashBagCapacityInGallons = 1f;
            sut.disableAddAfterThisDecimalPercent = 1.0f;
            var callCount = 0;
            sut.TrashAddEvent += _ => callCount++;
            yield return null;

            sut.Add(new TestTrash(1f));
            // The trash bag is now full and cannot accept more trash
            sut.Add(new TestTrash(1f));

            Assert.AreEqual(1, callCount);
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