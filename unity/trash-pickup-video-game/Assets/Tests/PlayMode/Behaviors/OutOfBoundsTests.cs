using System.Collections;
using Behaviors;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Behaviors
{
    public class OutOfBoundsTests
    {
        [UnityTest]
        public IEnumerator OutOfBoundsDetectsTrash()
        {
            var testTrash = new GameObject().AddComponent<Trash>();
            var sut = new GameObject().AddComponent<OutOfBounds>();
            var eventCalled = false;
            sut.BoundaryViolationEvent += () => eventCalled = true;
            testTrash.GetComponent<Transform>().position = sut.GetComponent<Transform>().position;
            yield return null;
            yield return null;

            Assert.IsTrue(eventCalled);
        }
    }
}