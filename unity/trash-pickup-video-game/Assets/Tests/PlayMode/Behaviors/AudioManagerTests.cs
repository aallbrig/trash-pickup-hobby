using System.Collections;
using Behaviors;
using Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Behaviors
{
    public class AudioManagerTests
    {
        public class TestTrash : ITrash
        {

            public float WeightAddInGallons => 0f;

            public float Score => 0f;

            // 😅 Getting these values was trial and error. I can't wait until dependent types are more universal and help.
            public AudioClip CollectSound => AudioClip.Create("fake-name", 1, 1, 1000, false, _ => {});
        }

        // TODO: Fix
        // [UnityTest]
        public IEnumerator AudioManagerCanPlaySoundsForCollectingTrash()
        {
            var audioListener = new GameObject().AddComponent<AudioListener>();
            var eventCalled = false;
            var sut = new GameObject().AddComponent<AudioManager>();
            var testTrashbag = new GameObject().AddComponent<Trashbag>();
            sut.trashbag = testTrashbag;
            sut.CollectAudioStartedEvent = () => eventCalled = true;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            testTrashbag.Add(new TestTrash());

            Assert.IsTrue(eventCalled);
        }
    }
}