using System;

namespace PeNet.Asn1
{
    public class Asn1ParsingException : Exception
    {
        public Asn1ParsingException(string message) : base(message)
        {
        }
    }
}