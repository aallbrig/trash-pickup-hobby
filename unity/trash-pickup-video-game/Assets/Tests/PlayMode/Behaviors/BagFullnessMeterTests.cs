using System.Collections;
using Behaviors;
using Models;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.PlayMode.Behaviors
{
    public class BagFullnessMeterTests
    {
        private class TestTrash:ITrash
        {
            public TestTrash(float weightAddInGallons = default, float score = default)
            {
                WeightAddInGallons = weightAddInGallons;
                Score = score;
            }

            public float WeightAddInGallons { get; }
            public float Score { get; }
            public AudioClip CollectSound => default;
        }

        [UnityTest]
        public IEnumerator BagFullnessMeterUpdatesOnTrashBagAdd()
        {
            var eventCalled = false;
            var eventCallCount = 0;
            var sut = new GameObject().AddComponent<TrashBagFullnessMeter>();
            var testTrashBag = new GameObject().AddComponent<TrashBag>();
            sut.trashBag = testTrashBag;
            sut.MeterUiSyncedEvent += () =>
            {
                eventCalled = true;
                eventCallCount++;
            };
            yield return null;

            testTrashBag.Add(new TestTrash(1f));

            Assert.IsTrue(eventCalled);
            // Event called two times -- one for start, one for adding trash
            Assert.AreEqual(2, eventCallCount);
        }

        [UnityTest]
        public IEnumerator BagFullnessMeterUpdatesOnTrashBagFull()
        {
            var eventCalled = false;
            var eventCallCount = 0;
            var sut = new GameObject().AddComponent<TrashBagFullnessMeter>();
            var testTrashBag = new GameObject().AddComponent<TrashBag>();
            testTrashBag.trashBagCapacityInGallons = 1f;
            sut.trashBag = testTrashBag;
            sut.MeterUiSyncedEvent += () =>
            {
                eventCalled = true;
                eventCallCount++;
            };
            yield return null;

            testTrashBag.Add(new TestTrash(1f));

            Assert.IsTrue(eventCalled);
            // Event called three times -- one for start, one for adding trash, one for full
            Assert.AreEqual(3, eventCallCount);
        }

        [UnityTest]
        public IEnumerator BagFullnessMeterUpdatesOnTrashBagEmpty()
        {
            var eventCalled = false;
            var eventCallCount = 0;
            var sut = new GameObject().AddComponent<TrashBagFullnessMeter>();
            var testTrashBag = new GameObject().AddComponent<TrashBag>();
            testTrashBag.trashBagCapacityInGallons = 1f;
            sut.trashBag = testTrashBag;
            sut.MeterUiSyncedEvent += () =>
            {
                eventCalled = true;
                eventCallCount++;
            };
            yield return null;

            testTrashBag.Add(new TestTrash(1f));
            testTrashBag.Empty();

            Assert.IsTrue(eventCalled);
            // Event called four times -- one for start, one for adding trash, one for full, one for empty
            Assert.AreEqual(4, eventCallCount);
        }
    }
}