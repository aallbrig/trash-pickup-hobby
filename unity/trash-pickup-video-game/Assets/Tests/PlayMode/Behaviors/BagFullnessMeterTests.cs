using System.Collections;
using Behaviors;
using Models;
using NUnit.Framework;
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
        public IEnumerator BagFullnessMeterUpdatesOnTrashbagAdd()
        {
            var eventCalled = false;
            var sut = new GameObject().AddComponent<BagFullnessMeter>();
            var testTrashbag = new GameObject().AddComponent<Trashbag>();
            sut.trashbag = testTrashbag;
            sut.MeterUiSyncedEvent += () => eventCalled = true;
            yield return null;

            testTrashbag.Add(new TestTrash(1f));

            Assert.IsTrue(eventCalled);
        }

        [UnityTest]
        public IEnumerator BagFullnessMeterUpdatesOnTrashbagFull()
        {
            var eventCalled = false;
            var eventCallCount = 0;
            var sut = new GameObject().AddComponent<BagFullnessMeter>();
            var testTrashbag = new GameObject().AddComponent<Trashbag>();
            testTrashbag.trashbagCapacityInGallons = 1f;
            sut.trashbag = testTrashbag;
            sut.MeterUiSyncedEvent += () =>
            {
                eventCalled = true;
                eventCallCount++;
            };
            yield return null;

            testTrashbag.Add(new TestTrash(1f));

            Assert.IsTrue(eventCalled);
            // Event called twice -- one for add, one for full
            Assert.AreEqual(2, eventCallCount);
        }

        [UnityTest]
        public IEnumerator BagFullnessMeterUpdatesOnTrashbagEmpty()
        {
            var eventCalled = false;
            var eventCallCount = 0;
            var sut = new GameObject().AddComponent<BagFullnessMeter>();
            var testTrashbag = new GameObject().AddComponent<Trashbag>();
            testTrashbag.trashbagCapacityInGallons = 1f;
            sut.trashbag = testTrashbag;
            sut.MeterUiSyncedEvent += () =>
            {
                eventCalled = true;
                eventCallCount++;
            };
            yield return null;

            testTrashbag.Add(new TestTrash(1f));
            testTrashbag.Empty();

            Assert.IsTrue(eventCalled);
            // Event called three times -- one for add, one for full, one for empty
            Assert.AreEqual(3, eventCallCount);
        }
    }
}