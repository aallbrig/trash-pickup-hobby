using System.Collections;
using Behaviors;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Behaviors
{
    public class GarbageGeneratorTests
    {
        [UnityTest]
        public IEnumerator GarbageGeneratorGeneratesGarbage()
        {
            var eventCalled = false;
            var sut = new GameObject().AddComponent<GarbageGenerator>();
            sut.trashPrefabs.Add(new GameObject());
            sut.GarbageGeneratedEvent = () => eventCalled = true;
            sut.minimumSpawnTimeInSeconds = 0.001f;
            sut.maximumSpawnTimeInSeconds = 0.0015f;
            yield return new WaitForEndOfFrame();

            yield return new WaitForSeconds(0.1f);

            Assert.IsTrue(eventCalled);
        }
    }
}