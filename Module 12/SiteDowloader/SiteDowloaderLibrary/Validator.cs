using SiteDownloaderLibrary.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteDownloaderLibrary
{
    public class Validator 
    {
        private readonly IEnumerable<IConstraintRule> _constraintRules;

        public Validator(IEnumerable<IConstraintRule> constraintRules)
        {
            _constraintRules = constraintRules;
        }

        public bool IsValid(Uri uri)
        {
            return _constraintRules.All(rule => rule.IsValid(uri));
        }

        public bool IsValid(string uri)
        {
            return _constraintRules.All(rule => rule.IsValid(uri));
        }
    }
}
