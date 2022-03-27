using System.Collections;
using Behaviors;
using Controllers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Controllers
{
    public class PlayerControllerTests : InputTestFixture
    {
        [UnityTest]
        public IEnumerator PlayerCanTapOnTrash()
        {
            InputSystem.AddDevice<Touchscreen>();
            var pointer = InputSystem.AddDevice<Pointer>();
            var testCamera = new GameObject { transform = { position = new Vector3(10, 1, -10) } }.AddComponent<Camera>();
            var testTrash = new GameObject { transform = { position = new Vector3(10, 0, 0) } }.AddComponent<Trash>();
            testTrash.transform.GetComponent<Rigidbody>().useGravity = false;
            var sut = new GameObject().AddComponent<PlayerController>();
            sut.mainCamera = testCamera;
            var eventCalled = false;
            sut.PlayerTrashPickupEvent += _ => eventCalled = true;
            yield return new WaitForFixedUpdate();

            // Player uses pointer to tap on screen, causing raycast to see the test trash
            var worldSpaceToScreen = testCamera.WorldToScreenPoint(testTrash.transform.position);
            BeginTouch(pointer.deviceId, worldSpaceToScreen);
            EndTouch(pointer.deviceId, worldSpaceToScreen);

            Assert.IsTrue(eventCalled);
        }
    }
}