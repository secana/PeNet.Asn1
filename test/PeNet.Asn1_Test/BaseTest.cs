using Xunit;

namespace PeNet.Asn1_Test {
    public abstract class BaseTest {

        public static void AreEqual(byte[] expected, byte[] actual) {
            Assert.Equal(expected.Length, actual.Length);
            for (var i = 0; i < expected.Length; i++) {
                Assert.Equal(expected[i], actual[i]);
            }
        }
    }
}
