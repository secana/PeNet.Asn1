﻿using System.IO;
using PeNet.Asn1;
using Xunit;

namespace PeNet.Asn1_Test {
    public class Asn1NullTests : BaseTest {

        private static readonly byte[] _etalon = { 0x05, 0x00 };

        [Fact]
        public void ReadTest() {
            var node = Asn1Node.ReadNode(new MemoryStream(_etalon));
            var typed = node as Asn1Null;
            Assert.NotNull(typed);
        }

        [Fact]
        public void WriteTest() {
            var node = new Asn1Null();
            var data = node.GetBytes();
            AreEqual(_etalon, data);
        }
    }
}
