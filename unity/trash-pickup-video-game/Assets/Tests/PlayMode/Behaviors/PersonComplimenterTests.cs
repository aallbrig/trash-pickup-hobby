using System.Collections;
using Behaviors;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Behaviors
{
    public class PersonComplimenterTests
    {
        [UnityTest]
        public IEnumerator PersonComplimenterTestsWithEnumeratorPasses()
        {
            var sut = new GameObject().AddComponent<PersonComplimenter>();
            yield return null;
        }
    }
}