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
        public IEnumerator GarbageGeneratorExists()
        {
            var sut = new GameObject().AddComponent<GarbageGenerator>();
            yield return null;
            Assert.IsNotNull(sut);
        }
    }
}