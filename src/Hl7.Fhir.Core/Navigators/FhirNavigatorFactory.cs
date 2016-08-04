using System.Xml.Linq;
using System.Xml;
using Hl7.Fhir.Support;
using Newtonsoft.Json;
using System;
using Furore.MetaModel;

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

            return new XmlDomFhirNavigator(doc.Root, null, XState.Element, disallowXsiAttributesOnRoot);
        }

        public static IElementNavigator CreateXmlNavigator(string xml)
        {
            using (var reader = SerializationUtil.XmlReaderFromXmlText(xml))
            {
                return Create(reader);
            }
        }

        public static IElementNavigator Create(JsonReader reader, bool disallowXsiAttributesOnRoot = false)
        {
            throw new NotImplementedException();
        }
    }

}