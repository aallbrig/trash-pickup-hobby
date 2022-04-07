using System.Collections;
using Behaviors;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.PlayMode.Behaviors
{
    public class MenuAdPlacementTests
    {
        [UnityTest]
        public IEnumerator MenuAdPlacementCanConfigureAnAd()
        {
            var eventCalled = false;
            var sut = new GameObject().AddComponent<MenuAdPlacement>();
            sut.AdvertisementSystemConfiguredEvent += () => eventCalled = true;
            yield return null;

            Assert.IsTrue(eventCalled);
        }

        [UnityTest]
        public IEnumerator MenuAdPlacementCanRenderAnAd()
        {
            var eventCalled = false;
            var sut = new GameObject().AddComponent<MenuAdPlacement>();
            sut.AdvertisementRequestedEvent += () => eventCalled = true;
            yield return null;

            Assert.IsTrue(eventCalled);
        }
    }
}