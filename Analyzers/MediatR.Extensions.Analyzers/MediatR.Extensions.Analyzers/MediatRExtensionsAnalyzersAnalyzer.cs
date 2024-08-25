using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MediatR.Extensions.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RequestReturnTypeShouldBeResultOfTAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor s_rule = new DiagnosticDescriptor(
            "MEDIATR0001",
            "MediatR request should have a return type of Result<T>",
            "MediatR request should have a return type of Result<T>",
            "Design",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(s_rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;

            BaseListSyntax baseList = classDeclaration.BaseList;
            if (baseList != null)
            {
                foreach (BaseTypeSyntax baseType in baseList.Types)
                {
                    if (baseType.Type is GenericNameSyntax genericType)
                    {
                        if(genericType.Identifier.Text == "IRequest")
                        {
                            TypeSyntax typeArgument = genericType.TypeArgumentList?.Arguments.FirstOrDefault();

                            if (typeArgument is GenericNameSyntax innerGenericName
                                && innerGenericName.Identifier.ValueText == "Result")
                            {
                                continue;
                            }

                            var diagnostic = Diagnostic.Create(s_rule, classDeclaration.Identifier.GetLocation(), classDeclaration.Identifier.ValueText);
                            context.ReportDiagnostic(diagnostic);
                        }
                    }
                }
            }
        }
    }
}
