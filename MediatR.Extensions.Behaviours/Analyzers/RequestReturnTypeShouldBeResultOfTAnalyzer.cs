using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MediatR.Extensions.Behaviours.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class RequestReturnTypeShouldBeResultOfTAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor Rule = new(
        "MEDIATR0001",
        "MediatR request should have a return type of Result<T>",
        "MediatR request should have a return type of Result<T>",
        "Design",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
    }

    private static void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;

        BaseListSyntax? baseList = classDeclaration.BaseList;
        if (baseList != null)
        {
            foreach (BaseTypeSyntax baseType in baseList.Types)
            {
                string baseTypeName = baseType.Type.GetType().Name;

                if (baseTypeName.Contains("IRequest"))
                {
                    if (baseType.Type is GenericNameSyntax genericName)
                    {
                        TypeSyntax? typeArgument = genericName.TypeArgumentList?.Arguments.FirstOrDefault();

                        if (typeArgument is GenericNameSyntax innerGenericName && innerGenericName.Identifier.ValueText == "Result")
                        {
                            // Check if it is Result<T> where T can be any type
                            var diagnostic = Diagnostic.Create(Rule, classDeclaration.Identifier.GetLocation(), classDeclaration.Identifier.ValueText);
                            context.ReportDiagnostic(diagnostic);
                        }
                    }
                }
            }
        }
    }
}
