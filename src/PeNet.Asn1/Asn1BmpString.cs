using System.IO;
using System.Text;
using System.Xml.Linq;

namespace PeNet.Asn1 {
    public class Asn1BmpString: Asn1StringNode {

        public const string NODE_NAME = "BMPString";

        public override string Value { get; }

        public Asn1BmpString(string value) {
            Value = value;
        }

        public static Asn1BmpString ReadFrom(Stream stream) {
            if (stream.Length % 2 != 0)
                throw new Asn1ParsingException("Length of BMPString must be a multiple of 2.");

            var result = new StringBuilder((int)(stream.Length / 2));

            for (;;) {
                var firstByte = stream.ReadByte();
                if (firstByte == -1)
                    break;

                var secondByte = stream.ReadByte();

                result.Append((char)((firstByte << 8) | secondByte));
            }

            return new Asn1BmpString(result.ToString());
        }

        public override Asn1UniversalNodeType NodeType => Asn1UniversalNodeType.BmpString;

        public override Asn1TagForm TagForm => Asn1TagForm.Primitive;

        protected override XElement ToXElementCore() {
            return new XElement(NODE_NAME, Value);
        }

        protected override byte[] GetBytesCore() {
            var bytes = new byte[Value.Length * 2];

            for (var i = 0; i < Value.Length; ++i) {
                bytes[i * 2] = (byte)(Value[i] / 0x100);
                bytes[i * 2 + 1] = (byte)(Value[i] % 0x100);
            }

            return bytes;
        }

        public new static Asn1BmpString Parse(XElement xNode) {
            var value = xNode.Value.Trim(); //todo should it be trimmed?
            return new Asn1BmpString(value);
        }
    }
}
