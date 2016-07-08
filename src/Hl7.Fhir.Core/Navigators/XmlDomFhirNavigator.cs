using System;
using Hl7.Fhir.FluentPath;
using System.Xml.Linq;
using Hl7.Fhir.Support;
using System.Xml;

namespace Hl7.Fhir.Serialization
{
    public class XmlDomFhirNavigator : IElementNavigator, IPositionInfo
    {
        private XmlDomFhirNavigator(XObject parent, XObject current, bool disallowXsiAttributesOnRoot = false)
        {
            this.parent = parent;
            this.current = current;
            this.DisallowXsiAttributesOnRoot = disallowXsiAttributesOnRoot;
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string TypeName
        {
            get
            {
                if (current is XElement)
                    return ((XElement)current).Name.LocalName;
                else
                    throw Error.Format("Cannot get resource type name: reader not at an element", this);
            }
        }

        public object Value
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IElementNavigator Clone()
        {
            return new XmlDomFhirNavigator(current, this.DisallowXsiAttributesOnRoot);
        }

        public bool MoveToFirstChild()
        {
            if (current is XElement)
            {
                bool ok = MoveToFirstXAttribute();
                if (ok)
                {
                    return ok;
                }
                else
                {
                    return MoveToFirstXElement();
                }
            }
            else
            {
                return false;
            }
        }

        public bool MoveToNext()
        {
            bool ok = true;

            if (IsAttribute)
            {
                ok = MoveToNextXAttribute();
                if (ok)
                {
                    return ok;
                }
                else
                {
                    return MoveToNextXElement();
                }
            }
            else
            {
                return MoveToNextXElement();
            } 
        }

        private void MovetoRoot(XObject root)
        {
            if (root is XDocument)
                current = ((XDocument)root).Root;
            else
                current = root;
        }

        public const string BINARY_CONTENT_MEMBER_NAME = "content";
        public bool DisallowXsiAttributesOnRoot
        {
            get;
            set;
        }

        public int LineNumber
        {
            get
            {
                var li = (IXmlLineInfo)current;

                if (!li.HasLineInfo())
                    throw Error.InvalidOperation("No lineinfo available. Please read the Xml document using LoadOptions.SetLineInfo.");

                return li.LineNumber;
            }
        }

        public int LinePosition
        {
            get
            {
                var li = (IXmlLineInfo)current;

                if (!li.HasLineInfo())
                    throw Error.InvalidOperation("No lineinfo available. Please read the Xml document using LoadOptions.SetLineInfo.");

                return li.LinePosition;
            }
        }


        private bool ValidAttribute(XAttribute attr)
        {
            return true;
        }

        private bool MoveToFirstXAttribute()
        {
            if (!(current is XElement)) return false;

            var element = (XElement)current;
            if (element.HasAttributes)
            {
                parent = element;
                current = element.FirstAttribute;
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool MoveToNextXAttribute()
        {
            if (current is XAttribute)
            {
                var a = (XAttribute)current;
                bool ok;
                do
                {
                    a = a.NextAttribute;
                    ok = (a != null);
                }
                while (ok && !ValidAttribute(a));
                if (!ok) parent = null; // warning, circumstantial evidence
                return ok;
            }
            else
            {
                return false;
            }
        }

        private bool MoveToFirstXElement()
        {
            if (!(current is XElement)) return false;

            if (parent.HasElements) 
            { 
                current = parent.FirstNode;
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool MoveToNextXElement()
        {
            if (current is XElement)
            {
                var node = (XNode)current;
                current = node.NextNode;
                return (current != null);
            }
            else
            {
                return false;
            }
        }

        private bool IsAttribute
        {
            get 
            {
                return (current is XAttribute);
            }
        }
        
        XElement parent;
        XObject current;
    }
}