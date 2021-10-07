using System.IO;
using Xunit;
using PeNet.Asn1;

namespace PeNet.Asn1_Test
{
    public class Asn1UnsupportedUniversalTypeTest : BaseTest
    {
        
        [Fact]
        public void ReadNode_UnsupportedUniversalType_DoesNotCrashButReturnsCustomType()
        {
            var cert = File.ReadAllBytes(@"./test_files/old_firefox_x86.pkcs7");
            var node = Asn1Node.ReadNode(cert);
            

            Assert.Equal(Asn1TagClass.Universal, node.Nodes[0].TagClass); // Just access some value from the parsed cert.
        }
    }
}
