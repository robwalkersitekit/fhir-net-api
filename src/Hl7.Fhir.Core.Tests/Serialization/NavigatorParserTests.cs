using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.FluentPath;
using System.Diagnostics;

namespace Hl7.Fhir.Core.Tests.Serialization
{
    [TestClass]
    public class NavigatorParserTests
    {

        public static void Render(IElementNavigator navigator, int nest = 0)
        {
            do
            {
                string indent = new string(' ', nest * 4);
                Debug.WriteLine($"{indent}{navigator}");

                var child = navigator.Clone();
                if (child.MoveToFirstChild())
                {
                    Render(child, nest + 1);
                }
            }
            while (navigator.MoveToNext());
        }

        [TestMethod]
        public void NavigatorParseXmlTest()
        {
            string xml = File.ReadAllText(@"TestData\TestPatient.xml");
            var nav = FhirNavigatorFactory.CreateXmlNavigator(xml);
            Render(nav);
        }
    }
}
