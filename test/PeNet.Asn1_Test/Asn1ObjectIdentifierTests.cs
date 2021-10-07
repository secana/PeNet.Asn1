using System.IO;
using PeNet.Asn1;
using Xunit;

namespace PeNet.Asn1_Test {
    public class Asn1ObjectIdentifierTests : BaseTest {

        private static readonly byte[] _etalon = { 0x06, 0x03, 0x55, 0x04, 0x0a };

        [Fact]
        public void ReadTest() {
            var node = Asn1Node.ReadNode(new MemoryStream(_etalon));
            var typed = node as Asn1ObjectIdentifier;
            Assert.NotNull(typed);
            Assert.Equal("2.5.4.10", typed.Value);
        }

        [Fact]
        public void WriteTest() {
            var node = new Asn1ObjectIdentifier("2.5.4.10");
            var data = node.GetBytes();
            AreEqual(_etalon, data);
        }

        [Fact]
        public void WriteWithTrailingZeroTest() {
            var data = new byte[] { 0x06, 0x07, 0x2a, 0x85, 0x03, 0x02, 0x02, 0x24, 0x00 };

            var node = Asn1Node.ReadNode(new MemoryStream(data));
            var typed = node as Asn1ObjectIdentifier;
            Assert.Equal("1.2.643.2.2.36.0", typed.Value);

            var newData = typed.GetBytes();
            AreEqual(data, newData);
        }
    }
}
