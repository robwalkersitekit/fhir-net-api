﻿/* 
 * Copyright (c) 2016, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.githubusercontent.com/ewoutkramer/fhir-net-api/master/LICENSE
 */

using System;
using Hl7.ElementModel;
using Hl7.Fhir.Model;
using Hl7.Fhir.Specification.Navigation;
using Hl7.Fhir.Support;


namespace Hl7.Fhir.Validation
{
    internal class ElementBucket : BaseBucket
    {
        public ElementBucket(ElementDefinitionNavigator root, Validator validator) : base(root.Current)
        {
            Root = root.ShallowCopy();
            Validator = validator;
        }

        public ElementDefinitionNavigator Root { get; private set; }

        public Validator Validator { get; private set; }

        public override bool Add(IElementNavigator instance)
        {
            // Membership of an "element bucket" should be determined by element name
            //var matches = ChildNameMatcher.NameMatches(Root.PathName, candidate);
            //if (!matches)
            //    Validator.Trace(outcome, $"Element name {candidate.Name} does match definition {Root.Path}", Issue.CONTENT_ELEMENT_NAME_DOESNT_MATCH_DEFINITION, candidate);

            Members.Add(instance);
            return true;
        }


        public override OperationOutcome Validate(Validator validator, IElementNavigator errorLocation)
        {
            var outcome = base.Validate(validator, errorLocation);

            foreach(var member in Members)
            {
                outcome.Include(Validator.Validate(member, Root));
            }

            return outcome;
        }
    }
}
