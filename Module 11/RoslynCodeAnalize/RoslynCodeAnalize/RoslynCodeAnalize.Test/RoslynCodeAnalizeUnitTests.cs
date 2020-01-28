using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;
using RoslynCodeAnalize;

namespace RoslynCodeAnalize.Test
{
    [TestClass]
    public class UnitTest : CodeFixVerifier
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void WithoutControllersTest()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void WrongClassName()
        {
            var test = @"
                        using System;
                        using System.Collections.Generic;
                        using System.Linq;
                        using System.Web;
                        using System.Web.Mvc;

                        namespace TestProject
                        {
                            public class Home : Controller
                            {
                                public int Index()
                                {
                                    Home controller = new Home();
                                    return 13;
                                }
                            }
                        }";
            var expected = new DiagnosticResult
            {
                Id = "RoslynCodeAnalize",
                Message = String.Format("Class '{0}' doesn't contain suffix Controller", "Home"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 11, 15)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
                            using System;
                            using System.Collections.Generic;
                            using System.Linq;
                            using System.Text;
                            using System.Threading.Tasks;
                            using System.Diagnostics;

                            namespace ConsoleApplication1
                            {
                                public class baseClassController : Controller
                                {   
                                    public const int = 123;
                                }
                            }";
            VerifyCSharpFix(test, fixtest);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new RoslynCodeAnalizeCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new RoslynCodeAnalizeAnalyzer();
        }
    }
}
