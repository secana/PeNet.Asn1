namespace PeNet.Asn1
{
    public enum Asn1UniversalNodeType
    {
        Boolean             = 0x01,
        Integer             = 0x02,
        BitString           = 0x03,
        OctetString         = 0x04,
        Null                = 0x05,
        ObjectId            = 0x06,
        ObjectDescriptor    = 0x07, // Not supported by PeNet.Asn1
        InstanceOf          = 0x08, // Not supported by PeNet.Asn1
        Real                = 0x09,
        Enumerated          = 0x0a, // Not supported by PeNet.Asn1
        EmbeddedPdv         = 0x0b, // Not supported by PeNet.Asn1
        Utf8String          = 0x0c,
        RelativeOid         = 0x0d, // Not supported by PeNet.Asn1
        // 0x0e, 0x0f do not exist
        Sequence            = 0x10,
        Set                 = 0x11,
        NumericString       = 0x12,
        PrintableString     = 0x13,
        TeletextString      = 0x14, // Not supported by PeNet.Asn1
        VideotextString     = 0x15, // Not supported by PeNet.Asn1
        Ia5String           = 0x16,
        UtcTime             = 0x17,
        GeneralizedTime     = 0x18, // Not supported by PeNet.Asn1
        GraphicString       = 0x1a, // Not supported by PeNet.Asn1
        VisibleString       = 0x1b, // Not supported by PeNet.Asn1
        GeneralString       = 0x1a, // Not supported by PeNet.Asn1
        UniversalString     = 0x1c, // Not supported by PeNet.Asn1
        CharacterString     = 0x1d, // Not supported by PeNet.Asn1
        BmpString           = 0x1e
    }
}