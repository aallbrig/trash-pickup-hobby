using System.Collections;
using Behaviors;
using Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Behaviors
{
    public class TrashbagInstructionTextTest
    {
        [UnityTest]
        public IEnumerator TrashbagInstructionsFadesAfterFirstTrashbagEmpty()
        {
            var eventCalled = false;
            var testTrashbag = new GameObject().AddComponent<Trashbag>();
            var sut = new GameObject().AddComponent<TrashbagInstructionText>();
            sut.trashbag = testTrashbag;
            sut.InstructionsDismissedEvent += () => eventCalled = true;
            yield return null;
            
            testTrashbag.Empty();
            
            Assert.IsTrue(eventCalled);
        }
        [UnityTest]
        public IEnumerator TrashbagEmptyInstructionsAppearWhenTrashbagIsFull()
        {
            var eventCalled = false;
            var testTrashbag = new GameObject().AddComponent<Trashbag>();
            var sut = new GameObject().AddComponent<TrashbagInstructionText>();
            sut.trashbag = testTrashbag;
            sut.InstructionsActivatedEvent += () => eventCalled = true;
            yield return null;
            
            testTrashbag.Add(new TestTrash());
            testTrashbag.Empty();
            
            Assert.IsTrue(eventCalled);
        }
        [UnityTest]
        public IEnumerator TrashbagEmptyInstructionTextOnlyAppearsBeforeFirstEmpty()
        {
            var eventCallCount = 0;
            var testTrashbag = new GameObject().AddComponent<Trashbag>();
            var sut = new GameObject().AddComponent<TrashbagInstructionText>();
            sut.trashbag = testTrashbag;
            sut.InstructionsActivatedEvent += () => eventCallCount++;
            yield return null;
            
            testTrashbag.Add(new TestTrash());
            testTrashbag.Empty();
            testTrashbag.Add(new TestTrash());
            testTrashbag.Empty();
            
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