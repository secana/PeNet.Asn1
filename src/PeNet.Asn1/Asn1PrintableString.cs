﻿using System.IO;
using System.Text;
using System.Xml.Linq;

namespace PeNet.Asn1 {
    public class Asn1PrintableString : Asn1StringNode {

        public const string NODE_NAME = "PrintableString";

        public override string Value { get; }

        public Asn1PrintableString(string value) {
            Value = value;
        }

        public static Asn1PrintableString ReadFrom(Stream stream) {
            var data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            return new Asn1PrintableString(Encoding.ASCII.GetString(data));
        }

        public override Asn1UniversalNodeType NodeType => Asn1UniversalNodeType.PrintableString;

        public override Asn1TagForm TagForm => Asn1TagForm.Primitive;

        protected override XElement ToXElementCore() {
            return new XElement(NODE_NAME, Value);
        }

        protected override byte[] GetBytesCore() {
            return Encoding.ASCII.GetBytes(Value);
        }

        public new static Asn1PrintableString Parse(XElement xNode) {
            var value = xNode.Value.Trim(); //todo should it be trimmed?
            return new Asn1PrintableString(value);
        }
    }
}
