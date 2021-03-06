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
            var camera = new GameObject().AddComponent<Camera>();
            var sut = new GameObject().AddComponent<GarbageGenerator>();
            sut.injectedCamera = camera;
            sut.trashPrefabs.Add(new GameObject());
            sut.GarbageGeneratedEvent = _ => eventCalled = true;
            sut.minimumSpawnTimeInSeconds = 0.001f;
            sut.maximumSpawnTimeInSeconds = 0.0015f;
            yield return null;

            sut.StartGeneration();
            yield return new WaitForSeconds(0.2f);

            Assert.IsTrue(eventCalled);
        }
        
        [UnityTest]
        public IEnumerator GarbageGeneratorGeneratesGarbageACertainDistanceAway()
        {
            Transform generatedTrash = null;
            var camera = new GameObject().AddComponent<Camera>();
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y + 10,
                camera.transform.position.z - 20);
            var sut = new GameObject().AddComponent<GarbageGenerator>();
            sut.injectedCamera = camera;
            sut.trashPrefabs.Add(new GameObject());
            sut.GarbageGeneratedEvent = _ => generatedTrash = _;
            sut.minimumSpawnTimeInSeconds = 0.001f;
            sut.maximumSpawnTimeInSeconds = 0.0015f;
            yield return null;

            sut.StartGeneration();
            yield return new WaitForSeconds(0.2f);

            Assert.IsNotNull(generatedTrash);
        }
    }
}