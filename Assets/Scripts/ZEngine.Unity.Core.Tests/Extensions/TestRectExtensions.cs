using NUnit.Framework;
using UnityEngine;
using ZEngine.Unity.Core.Extensions;

namespace ZEngine.Unity.Core.Tests.Extensions
{
    public class TestRectExtensions
    {
        [Test]
        public void TestMax()
        {
            Rect rectA = new Rect
            {
                xMin = -1,
                yMin = -1,
                xMax = 1,
                yMax = 1,
            };
            
            Rect rectB = new Rect
            {
                xMin = -2,
                yMin = -2,
                xMax = 2,
                yMax = 2,
            };
            
            Assert.AreEqual(2, rectA.width);
            Assert.AreEqual(2, rectA.height);
            
            Assert.AreEqual(4, rectB.width);
            Assert.AreEqual(4, rectB.height);

            var result = RectExtensions.Max(rectA, rectB);
            
            Assert.AreEqual(-2, result.xMin);
            Assert.AreEqual(-2, result.yMin);
            Assert.AreEqual(2, result.xMax);
            Assert.AreEqual(2, result.yMax);
        }
        
        [Test]
        public void TestMax2()
        {
            Rect rectA = new Rect
            {
                xMin = -1,
                yMin = -1,
                xMax = 1,
                yMax = 1,
            };
            
            rectA.center = new Vector2(0f, 3f);
            
            Rect rectB = new Rect
            {
                xMin = -2,
                yMin = -2,
                xMax = 2,
                yMax = 2,
            };
            
            Assert.AreEqual(2, rectA.width);
            Assert.AreEqual(2, rectA.height);
            
            Assert.AreEqual(4, rectB.width);
            Assert.AreEqual(4, rectB.height);

            var result = RectExtensions.Max(rectA, rectB);
            
            Assert.AreEqual(-2, result.xMin);
            Assert.AreEqual(-2, result.yMin);
            Assert.AreEqual(2, result.xMax);
            Assert.AreEqual(4, result.yMax);
        }
    }
}