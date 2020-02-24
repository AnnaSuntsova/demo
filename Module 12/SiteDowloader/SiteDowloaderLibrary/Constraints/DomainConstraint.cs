using System;

namespace SiteDownloaderLibrary.Constraints
{
    public class DomainConstraint : IConstraintRule
    {
        private readonly Uri _baseUri;
        private readonly DomainLimitations _typeConstraint;

        public DomainConstraint(Uri baseUri, DomainLimitations typeConstraint)
        {
            _baseUri = baseUri;
            _typeConstraint = typeConstraint;
        }

        public bool IsValid(Uri uri)
        {
            switch (_typeConstraint)
            {
                case DomainLimitations.NoLimitation:
                {
                    return true;
                }
                case DomainLimitations.OnlyCurrentDomain:
                {
                    return _baseUri.DnsSafeHost == uri.DnsSafeHost;
                }
                case DomainLimitations.NotHigherInitialUrl:
                {
                    return _baseUri.IsBaseOf(uri);
                }
                default:
                {
                    throw new ArgumentException(nameof(_typeConstraint));
                }
            }
        }

        public bool IsValid(string url)
        {
            return IsValid(new Uri(url));
        }
    }
}
