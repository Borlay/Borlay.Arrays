using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace Borlay.Arrays.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void EqualTest()
        {
            var g1 = ByteArray.New(64).Bytes;

            var g2 = new byte[g1.Length];

            Buffer.BlockCopy(g1, 0, g2, 0, g1.Length);

            var equal = g1.ContainsSequence64(g2);

            Assert.IsTrue(equal);
        }

        [TestMethod]
        public void ContainsTest()
        {
            var g1 = ByteArray.New(64).Bytes;

            var g2 = new byte[g1.Length - 5];

            Buffer.BlockCopy(g1, 0, g2, 0, g1.Length - 5);

            var equal = g1.ContainsSequence64(g2);

            Assert.IsTrue(equal);
        }

        [TestMethod]
        public void NotContainsTest()
        {
            var g1 = ByteArray.New(64).Bytes;

            var g2 = new byte[g1.Length];

            Buffer.BlockCopy(g1, 0, g2, 0, g1.Length - 5);

            var equal = g1.ContainsSequence64(g2);

            Assert.IsFalse(equal);
        }

        [TestMethod]
        public void NotEqualTest()
        {
            var g1 = ByteArray.New(64).Bytes;

            var g2 = ByteArray.New(64).Bytes;

            var equal = g1.ContainsSequence64(g2);

            Assert.IsFalse(equal);
        }

        [TestMethod]
        public void EqualMany64Test()
        {
            var g1 = ByteArray.New(64).Bytes;

            var g2 = new byte[g1.Length];

            Buffer.BlockCopy(g1, 0, g2, 0, g1.Length);

            var watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                var equal = g1.ContainsSequence64(g2);
            }

            watch.Stop();

            // 10M 0.49s
        }

        [TestMethod]
        public void EqualMany32Test()
        {
            var g1 = ByteArray.New(64).Bytes;

            var g2 = new byte[g1.Length];

            Buffer.BlockCopy(g1, 0, g2, 0, g1.Length);

            var watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                var equal = g1.ContainsSequence32(g2);
            }

            watch.Stop();

            // 10M 0.53s
        }

        [TestMethod]
        public void EqualIterateManyTest()
        {
            var g1 = ByteArray.New(64).Bytes;

            var g2 = new byte[g1.Length];

            Buffer.BlockCopy(g1, 0, g2, 0, g1.Length);

            var watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                var equal = EqualSec(g1, g2);
            }

            watch.Stop();

            // 10M 2.4s
        }

        public bool EqualSec(byte[] left, byte[] right)
        {
            for(int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i]) return false;
            }
            return true;
        }
    }
}
