using System.Collections;
using Behaviors;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Behaviors
{
    public class MovingBackgroundTests
    
    {
        [UnityTest]
        public IEnumerator MovingBackgroundResetsOnPlayPressed()
        {
            var sut = new GameObject().AddComponent<MovingBackground>();
            var eventCalled = false;
            sut.BackgroundResetEvent += () => eventCalled = true;
            var testMainMenu = new GameObject().AddComponent<MainMenu>();
            sut.mainMenu = testMainMenu;
            yield return null;

            testMainMenu.ReadyToPlay();

            Assert.IsTrue(eventCalled);
        }
    }
}