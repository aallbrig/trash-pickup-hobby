using System.Collections;
using Behaviors;
using Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Behaviors
{
    public class TrashBagInstructionTextTest
    {
        [UnityTest]
        public IEnumerator TrashBagInstructionsFadesAfterFirstTrashBagEmpty()
        {
            var eventCalled = false;
            var testTrashBag = new GameObject().AddComponent<TrashBag>();
            var sut = new GameObject().AddComponent<TrashBagInstructionText>();
            sut.trashBag = testTrashBag;
            sut.InstructionsDismissedEvent += () => eventCalled = true;
            yield return null;
            
            testTrashBag.Empty();
            
            Assert.IsTrue(eventCalled);
        }
        [UnityTest]
        public IEnumerator TrashBagEmptyInstructionsAppearWhenTrashBagIsFull()
        {
            var eventCalled = false;
            var testTrashBag = new GameObject().AddComponent<TrashBag>();
            var sut = new GameObject().AddComponent<TrashBagInstructionText>();
            sut.trashBag = testTrashBag;
            sut.InstructionsActivatedEvent += () => eventCalled = true;
            yield return null;
            
            testTrashBag.Add(new TestTrash());
            testTrashBag.Empty();
            
            Assert.IsTrue(eventCalled);
        }
        [UnityTest]
        public IEnumerator TrashBagEmptyInstructionTextOnlyAppearsBeforeFirstEmpty()
        {
            var eventCallCount = 0;
            var testTrashBag = new GameObject().AddComponent<TrashBag>();
            var sut = new GameObject().AddComponent<TrashBagInstructionText>();
            sut.trashBag = testTrashBag;
            sut.InstructionsActivatedEvent += () => eventCallCount++;
            yield return null;
            
            testTrashBag.Add(new TestTrash());
            testTrashBag.Empty();
            testTrashBag.Add(new TestTrash());
            testTrashBag.Empty();
            
            Assert.AreEqual(1, eventCallCount);
        }
        private class TestTrash : ITrash
        {
            public float WeightAddInGallons => 25.0f;

            public float Score => 1.0f;

            public AudioClip CollectSound => null;
        }
    }
}