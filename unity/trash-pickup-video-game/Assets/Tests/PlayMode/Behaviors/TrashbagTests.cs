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

        public class TestTrash : ITrash
        {
            public float WeightAddInGallons => 25.0f;

            public float Score => 1.0f;

            public AudioClip CollectSound => null;
        }
    }
}