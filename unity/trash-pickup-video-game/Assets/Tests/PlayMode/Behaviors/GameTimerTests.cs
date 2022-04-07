using System.Collections;
using Behaviors;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.PlayMode.Behaviors
{
    public class GameTimerTests
    {
        [UnityTest]
        public IEnumerator GameTimerCanCountDown()
        {
            var eventCalled = false;
            var sut = new GameObject().AddComponent<GameTimer>();
            sut.CountDownByOneSecondEvent += _ => eventCalled = true;
            sut.gameTimeInSeconds = 2f;
            yield return null;

            yield return new WaitForSeconds(1.1f);

            Assert.IsTrue(eventCalled);
        }
        [UnityTest]
        public IEnumerator GameTimerCanRunOutOfTime()
        {
            var eventCalled = false;
            var sut = new GameObject().AddComponent<GameTimer>();
            sut.OutOfTimeEvent += () => eventCalled = true;
            sut.gameTimeInSeconds = 0.25f;
            yield return null;

            yield return new WaitForSeconds(0.26f);

            Assert.IsTrue(eventCalled);
        }
        [UnityTest]
        public IEnumerator GameTimerCanCountBackFromThree()
        {
            var countDownCallCount = 0;
            var eventCalled = false;
            var sut = new GameObject().AddComponent<GameTimer>();
            sut.CountDownByOneSecondEvent += _ => countDownCallCount++;
            sut.OutOfTimeEvent += () => eventCalled = true;
            sut.gameTimeInSeconds = 3f;
            yield return null;

            yield return new WaitForSeconds(3.1f);

            Assert.IsTrue(eventCalled);
            Assert.AreEqual(3, countDownCallCount);
        }
    }
}