using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace RoslynCodeAnalize
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RoslynCodeAnalizeAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RoslynCodeAnalize";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";
        private const string Suffix = "Controller";   

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeNameOfClass, SymbolKind.NamedType);
        }

        private static void AnalyzeNameOfClass(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
            var controller = context.Compilation.GetTypeByMetadataName("System.Web.Mvc.Controller");

            var baseClasses = GetBaseClasses(namedTypeSymbol, context.Compilation.ObjectType);
            if (baseClasses.Contains(controller) && !namedTypeSymbol.Name.EndsWith(Suffix))
            {
                var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static ImmutableArray<INamedTypeSymbol> GetBaseClasses(INamedTypeSymbol type, INamedTypeSymbol objectType)
        {
            if (type == null || type.TypeKind == TypeKind.Error)
                return ImmutableArray<INamedTypeSymbol>.Empty;

            if (type != null && type.TypeKind != TypeKind.Error)
                return GetBaseClasses(type.BaseType, objectType).Add(type.BaseType);

            return ImmutableArray<INamedTypeSymbol>.Empty;
        }
    }
}
