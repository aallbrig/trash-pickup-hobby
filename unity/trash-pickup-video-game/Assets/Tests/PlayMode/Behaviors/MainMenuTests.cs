using System.Collections;
using Behaviors;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Behaviors
{
    public class MainMenuTests
    {
        [UnityTest]
        public IEnumerator MainMenuCanStartGame()
        {
            var eventCalled = false;
            var sut = new GameObject().AddComponent<MainMenu>();
            sut.PlayButtonPressedEvent += () => eventCalled = true;
            yield return null;
            
            // This is called by a unity button
            sut.ReadyToPlay();
            
            Assert.IsTrue(eventCalled);
        }
    }
}