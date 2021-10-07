using System.IO;
using System.Text;
using System.Xml.Linq;

namespace PeNet.Asn1 {
    public class Asn1NumericString : Asn1Node {

        public const string NODE_NAME = "NumericString";

        public string Value { get; set; }

        public static Asn1NumericString ReadFrom(Stream stream) {
            var data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            return new Asn1NumericString { Value = Encoding.ASCII.GetString(data) };
        }

        public override Asn1UniversalNodeType NodeType => Asn1UniversalNodeType.NumericString;

        public override Asn1TagForm TagForm => Asn1TagForm.Primitive;

        protected override XElement ToXElementCore() {
            return new XElement(NODE_NAME, Value);
        }

        protected override byte[] GetBytesCore() {
            return Encoding.ASCII.GetBytes(Value);
        }

        public new static Asn1NumericString Parse(XElement xNode) {
            var res = new Asn1NumericString();
            res.Value = xNode.Value.Trim(); //todo should it be trimmed?
            return res;
        }
    }
}
