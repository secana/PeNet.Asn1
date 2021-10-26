using System.IO;
using System.Text;
using System.Xml.Linq;

namespace PeNet.Asn1 {
    public class Asn1Utf8String : Asn1StringNode {

        public const string NODE_NAME = "UTF8";

        public override string Value { get; }

        public Asn1Utf8String(string value) {
            Value = value;
        }

        public static Asn1Utf8String ReadFrom(Stream stream) {
            var data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            return new Asn1Utf8String(Encoding.UTF8.GetString(data));
        }

        public override Asn1UniversalNodeType NodeType => Asn1UniversalNodeType.Utf8String;

        public override Asn1TagForm TagForm => Asn1TagForm.Primitive;

        protected override XElement ToXElementCore() {
            return new XElement(NODE_NAME, Value);
        }

        protected override byte[] GetBytesCore() {
            return Encoding.UTF8.GetBytes(Value);
        }

        public new static Asn1Utf8String Parse(XElement xNode) {
            var value = xNode.Value.Trim(); //todo should it be trimmed?
            return new Asn1Utf8String(value);
        }
    }
}
