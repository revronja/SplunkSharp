using System;  
using System.Xml.Serialization;

namespace SplunkSharp
{
    [XmlType("response")]
    public class Token
    {
        [XmlElement("sessionKey")]
        public string sessionKey { get; set; }
    }
}


