using Hl7.Fhir.FluentPath;
using System.Xml.Linq;
using System.Xml;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Serialization
{
    public static class FhirNavigatorFactory
    {
        public static IElementNavigator Create(XmlReader reader, bool disallowXsiAttributesOnRoot = false)
        {
            var internalReader = SerializationUtil.WrapXmlReader(reader);
            XDocument doc;
            try
            {
                doc = XDocument.Load(internalReader, LoadOptions.SetLineInfo);
            }
            catch (XmlException xec)
            {
                throw Error.Format("Cannot parse xml: " + xec.Message, null);
            }

            return new XmlDomFhirNavigator(null, doc.Root, disallowXsiAttributesOnRoot);
        }
    }
}