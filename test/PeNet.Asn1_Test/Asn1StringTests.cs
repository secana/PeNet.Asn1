using System.Collections.Generic;
using System.IO;
using PeNet.Asn1;
using Xunit;

namespace PeNet.Asn1_Test
{
    public class Asn1StringTests
    {
        [Fact]
        public void Filter_On_Asn1StringNode()
        {
            var cert = File.ReadAllBytes(@"./test_files/pidgin.pkcs7");
            var node = Asn1Node.ReadNode(cert);
            var organizationalUnitNames = FindObjectIdentifiers(node, Asn1ObjectIdentifier.OrganizationalUnitName);

            Assert.Equal(2, organizationalUnitNames.Count);
            Assert.Contains(organizationalUnitNames, s => s == "Secure Digital Certificate Signing");
            Assert.Contains(organizationalUnitNames, s => s == "http://www.usertrust.com");
        }

        private static IReadOnlyCollection<string> FindObjectIdentifiers(Asn1Node node, Asn1ObjectIdentifier oid)
        {
            var results = new HashSet<string>();
            FindObjectIdentifiers(node, oid, results);
            return results;
        }

        private static void FindObjectIdentifiers(Asn1Node node, Asn1ObjectIdentifier oid, ICollection<string> results)
        {
            var nodes = node.Nodes;
            if (nodes.Count == 2 && nodes[0] is Asn1ObjectIdentifier objectIdentifier && objectIdentifier == oid && nodes[1] is Asn1StringNode stringNode)
            {
                results.Add(stringNode.Value);
            }
            foreach (var child in nodes)
            {
                FindObjectIdentifiers(child, oid, results);
            }
        }
    }
}