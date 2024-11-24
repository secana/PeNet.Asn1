using System.Threading.Tasks;
using PeNet.Asn1;
using PublicApiGenerator;
using VerifyXunit;
using Xunit;

namespace PeNet.Asn1_Test
{
    public class PublicApi
    {
        [Fact]
        public Task ApprovePublicApi()
        {
            var options = new ApiGeneratorOptions { IncludeAssemblyAttributes = false };
            var publicApi = typeof(Asn1Node).Assembly.GeneratePublicApi(options);
            return Verifier.Verify(publicApi).UseFileName("PublicApi");
        }
    }
}
