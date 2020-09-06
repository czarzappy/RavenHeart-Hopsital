using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ZEngine.Unity.Core.Factories;
// ReSharper disable HeapView.BoxingAllocation

namespace ZEngine.Unity.Core.Tests.Factories
{
    public class TestMeshFactory
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestGenerateLineVertices()
        {
            Vector3[] results = VertFactory.GenerateLineVertices(Vector3.zero, 
                new Vector3(1f, 0f), 
                2f);

            int idx = 0;
            Assert.AreEqual(new Vector3(0f, -1f), results[idx++], "Expected first vertices to match");
            Assert.AreEqual(new Vector3(1f, -1f), results[idx++], "Expected second vertices to match");
            Assert.AreEqual(new Vector3(0f, 1f), results[idx++], "Expected third vertices to match");
            Assert.AreEqual(new Vector3(1f, 1f), results[idx++], "Expected fourth vertices to match");
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestMeshFactoryWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
