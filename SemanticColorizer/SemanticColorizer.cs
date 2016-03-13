using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CSharp = Microsoft.CodeAnalysis.CSharp;
using VB = Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.CSharp;


namespace SemanticColorizer
{

    [Export(typeof(ITaggerProvider))]
    [ContentType("CSharp")]
    [ContentType("Basic")]
    [TagType(typeof(IClassificationTag))]
    internal class SemanticColorizerProvider : ITaggerProvider
    {
        [Import]
        internal IClassificationTypeRegistryService ClassificationRegistry = null; // Set via MEF

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag {
            return (ITagger<T>)new SemanticColorizer(buffer, ClassificationRegistry);
        }
    }

    class SemanticColorizer : ITagger<IClassificationTag>
    {
        private ITextBuffer theBuffer;
        private IClassificationType fieldType;
        private IClassificationType enumFieldType;
        private IClassificationType extensionMethodType;
        private IClassificationType staticMethodType;
        private IClassificationType normalMethodType;
        private IClassificationType constructorType;
        private IClassificationType typeParameterType;
        private IClassificationType parameterType;
        private IClassificationType namespaceType;
        private IClassificationType propertyType;
        private IClassificationType localType;
        private IClassificationType typeSpecialType;
        private IClassificationType typeNormalType;
		private IClassificationType modifierType;
		private IClassificationType keywordType;
		private IClassificationType constantType;
		private IClassificationType declarationType;
		private IClassificationType eventType;
		private IClassificationType attributeType;
        private Cache cache;
#pragma warning disable CS0067
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
#pragma warning restore CS0067

        internal SemanticColorizer(ITextBuffer buffer, IClassificationTypeRegistryService registry) {
            theBuffer = buffer;
            fieldType = registry.GetClassificationType(Constants.FieldFormat);
            enumFieldType = registry.GetClassificationType(Constants.EnumFieldFormat);
            extensionMethodType = registry.GetClassificationType(Constants.ExtensionMethodFormat);
            staticMethodType = registry.GetClassificationType(Constants.StaticMethodFormat);
            normalMethodType = registry.GetClassificationType(Constants.NormalMethodFormat);
            constructorType = registry.GetClassificationType(Constants.ConstructorFormat);
            typeParameterType = registry.GetClassificationType(Constants.TypeParameterFormat);
            parameterType = registry.GetClassificationType(Constants.ParameterFormat);
            namespaceType = registry.GetClassificationType(Constants.NamespaceFormat);
            propertyType = registry.GetClassificationType(Constants.PropertyFormat);
            localType = registry.GetClassificationType(Constants.LocalFormat);
            typeSpecialType = registry.GetClassificationType(Constants.TypeSpecialFormat);
            typeNormalType = registry.GetClassificationType(Constants.TypeNormalFormat);
			modifierType = registry.GetClassificationType(Constants.ModifierFormat);
			keywordType = registry.GetClassificationType(Constants.KeywordFormat);
			constantType = registry.GetClassificationType(Constants.ConstantFormat);
			declarationType = registry.GetClassificationType(Constants.DeclarationFormat);
			eventType = registry.GetClassificationType(Constants.EventFormat);
			attributeType = registry.GetClassificationType(Constants.AttributeFormat);

			symbol_kind_to_classification_ = SymbolKindToClassificationType();
			syntax_kind_to_classification_ = NodeKindToClassificationType();
			declaration_kind_ = DeclarationTypes();
        }

        public IEnumerable<ITagSpan<IClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans) {
            if (spans.Count == 0) {
                return Enumerable.Empty<ITagSpan<IClassificationTag>>();
            }
            if (this.cache == null || this.cache.Snapshot != spans[0].Snapshot) {
                // this makes me feel dirty, but otherwise it will not
                // work reliably, as TryGetSemanticModel() often will return false
                // should make this into a completely async process somehow
                var task = Cache.Resolve(theBuffer, spans[0].Snapshot);
                try
                {
                    task.Wait();
                }
                catch (Exception)
                {
                    // TODO: report this to someone.
                    return Enumerable.Empty<ITagSpan<IClassificationTag>>();
                }
                cache = task.Result;
                if (cache == null)
                {
                    // TODO: report this to someone.
                    return Enumerable.Empty<ITagSpan<IClassificationTag>>();
                }
            }
            return GetTagsImpl(this.cache, spans);
        }


		Dictionary<SyntaxKind, IClassificationType> syntax_kind_to_classification_ = null;
		Dictionary<SymbolKind, IClassificationType> symbol_kind_to_classification_ = null;
		HashSet<SyntaxKind> declaration_kind_ = null;

		Dictionary<SymbolKind, IClassificationType> SymbolKindToClassificationType()
		{
			return new Dictionary<SymbolKind, IClassificationType>()
			{
				{ SymbolKind.Parameter, parameterType },
				{ SymbolKind.TypeParameter, typeParameterType },
				{ SymbolKind.Namespace, namespaceType },
				{ SymbolKind.Property, propertyType },
				{ SymbolKind.Local, localType },
				{ SymbolKind.Event, eventType },
			};
		}

		Dictionary<SyntaxKind, IClassificationType> NodeKindToClassificationType()
		{
			return new Dictionary<SyntaxKind, IClassificationType>()
			{
				{ SyntaxKind.Attribute, attributeType },

				{ SyntaxKind.NullLiteralExpression, constantType },
				{ SyntaxKind.TrueLiteralExpression, constantType },
				{ SyntaxKind.TrueKeyword, constantType },
				{ SyntaxKind.FalseLiteralExpression, constantType },
				{ SyntaxKind.FalseKeyword, constantType },
				{ SyntaxKind.ThisExpression, constantType },
				{ SyntaxKind.BaseExpression, constantType },

				{ SyntaxKind.PrivateKeyword, modifierType },
				{ SyntaxKind.PublicKeyword, modifierType },
				{ SyntaxKind.ProtectedKeyword, modifierType },
				{ SyntaxKind.InternalKeyword, modifierType },
				{ SyntaxKind.StaticKeyword, modifierType },
				{ SyntaxKind.AbstractKeyword, modifierType },
				{ SyntaxKind.VirtualKeyword, modifierType },
				{ SyntaxKind.SealedKeyword, modifierType },
				{ SyntaxKind.ConstKeyword, modifierType },
				{ SyntaxKind.ReadOnlyKeyword, modifierType },
				{ SyntaxKind.OverrideKeyword, modifierType },

				{ SyntaxKind.Parameter, declarationType },

				{ SyntaxKind.PredefinedType, typeSpecialType }
			};
		}

		HashSet<SyntaxKind> DeclarationTypes()
		{
			return new HashSet<SyntaxKind>(new SyntaxKind[]
			{
				SyntaxKind.ClassDeclaration,
				SyntaxKind.InterfaceDeclaration,
				SyntaxKind.MethodDeclaration,
				SyntaxKind.ConstructorDeclaration,
				SyntaxKind.OperatorDeclaration,
				SyntaxKind.ConversionOperatorDeclaration,
				SyntaxKind.PropertyDeclaration,
				SyntaxKind.FieldDeclaration,
				SyntaxKind.EventDeclaration,
				SyntaxKind.EventFieldDeclaration,
				SyntaxKind.IndexerDeclaration,
				SyntaxKind.DelegateDeclaration,
				SyntaxKind.VariableDeclaration,
				SyntaxKind.EnumDeclaration,
				SyntaxKind.LocalDeclarationStatement,
				SyntaxKind.GetAccessorDeclaration,
				SyntaxKind.SetAccessorDeclaration,
				SyntaxKind.AddAccessorDeclaration,
				SyntaxKind.RemoveAccessorDeclaration
			});
		}

		IEnumerable<ITagSpan<IClassificationTag>> GetTagsImpl(Cache doc, NormalizedSnapshotSpanCollection spans)
		{
			var snapshot = spans[0].Snapshot;

			Func<ClassifiedSpan, IClassificationType, ITagSpan<IClassificationTag>> tag =
				(span, type) => new TagSpan<IClassificationTag>(new SnapshotSpan(snapshot, span.TextSpan.Start, span.TextSpan.Length), new ClassificationTag(type));

			foreach (var span in GetClassifiedSpans(doc.Workspace, doc.SemanticModel, spans))
			{
				var node = GetExpression(span.TextSpan, doc.SyntaxRoot.FindNode(span.TextSpan, true, true));

				if (node == null)
					continue;

				var kind = node.Kind();

				// drill down if necessary
				if (declaration_kind_.Contains(kind))
				{
					var maybe_node = node.ChildThatContainsPosition(span.TextSpan.Start);

					if (maybe_node == null)
					{ }
					else if (maybe_node.IsToken)
					{
						kind = maybe_node.AsToken().Kind();
					}
					else if (maybe_node.IsNode)
					{
						node = maybe_node.AsNode();
						kind = node.Kind();
					}
				}

				var parent = node.Parent;

				// if inside attribute(list), then swallow everything
				if (kind == SyntaxKind.Attribute || kind == SyntaxKind.AttributeList ||
					(parent != null && (parent.Kind() == SyntaxKind.Attribute || parent.Kind() == SyntaxKind.AttributeArgument)))
				{
					yield return tag(span, attributeType);
					continue;
				}

				switch (span.ClassificationType)
				{
				case ClassificationTypeNames.ClassName:
				case ClassificationTypeNames.DelegateName:
				case ClassificationTypeNames.EnumName:
				case ClassificationTypeNames.Identifier:
				case ClassificationTypeNames.InterfaceName:
				case ClassificationTypeNames.ModuleName:
				case ClassificationTypeNames.StructName:
					{
						var symbol = doc.SemanticModel.GetSymbolInfo(node).Symbol ?? doc.SemanticModel.GetDeclaredSymbol(node);
						if (symbol == null)
							continue;
						IClassificationType type;
						if (symbol_kind_to_classification_.TryGetValue(symbol.Kind, out type))
							yield return tag(span, type);

						// more cases to examine: field, method, named type
						switch (symbol.Kind)
						{
						case SymbolKind.Field:
							{
								var field = symbol as IFieldSymbol;
								if (symbol.ContainingType.TypeKind == TypeKind.Enum)
									yield return tag(span, enumFieldType);
								else if (field != null && field.IsConst)
									yield return tag(span, constantType);
								else
									yield return tag(span, fieldType);
							}
							break;

						case SymbolKind.Method:
							if (IsExtensionMethod(symbol))
								yield return tag(span, extensionMethodType);
							else if (symbol.IsStatic)
								yield return tag(span, staticMethodType);
							else
								yield return tag(span, normalMethodType);
							break;

						case SymbolKind.NamedType:
							if (IsSpecialType(symbol))
								yield return tag(span, typeSpecialType);
							else
								yield return tag(span, typeNormalType);
							break;
						}
					}
					break;

				case ClassificationTypeNames.TypeParameterName:
					yield return tag(span, typeParameterType);
					break;

				case ClassificationTypeNames.Keyword:
					{
						IClassificationType type;
						if (syntax_kind_to_classification_.TryGetValue(kind, out type))
							yield return tag(span, type);
						else
						{
							if (kind == SyntaxKind.IdentifierName)
							{
								var syntax = node as CSharp.Syntax.TypeSyntax;
								if (syntax != null && syntax.IsVar)
									yield return tag(span, declarationType);
							}

							yield return tag(span, keywordType);
						}
					}
					break;

				default:
					break;
				}
			}
		}

        private bool IsSpecialType(ISymbol symbol) {
            var type = (INamedTypeSymbol)symbol;
            return type.SpecialType != SpecialType.None;
        }

        private bool IsExtensionMethod(ISymbol symbol) {
            var method = (IMethodSymbol)symbol;
            return method.IsExtensionMethod;
        }

        private SyntaxNode GetExpression(TextSpan span, SyntaxNode node)
		{
			if (node == null)
				return null;
			else if (node.CSharpKind() == CSharp.SyntaxKind.Argument) {
                return ((CSharp.Syntax.ArgumentSyntax)node).Expression;
            }
            else if (node.CSharpKind() == CSharp.SyntaxKind.AttributeArgument) {
                return ((CSharp.Syntax.AttributeArgumentSyntax)node).Expression;
            }
            else if (node.VBKind() == VB.SyntaxKind.SimpleArgument) {
                return ((VB.Syntax.SimpleArgumentSyntax)node).Expression;
            }
            return node;
        }

		IEnumerable<ClassifiedSpan> GetClassifiedSpans(Workspace workspace, SemanticModel model, NormalizedSnapshotSpanCollection spans)
		{
			return spans.SelectMany(span => Classifier.GetClassifiedSpans(model, TextSpan.FromBounds(span.Start, span.End), workspace));
		}


        private class Cache
        {
            public Workspace Workspace { get; private set; }
            public Document Document { get; private set; }
            public SemanticModel SemanticModel { get; private set; }
            public SyntaxNode SyntaxRoot { get; private set; }
            public ITextSnapshot Snapshot { get; private set; }

            private Cache() {}

            public static async Task<Cache> Resolve(ITextBuffer buffer, ITextSnapshot snapshot) {
                var workspace = buffer.GetWorkspace();
                var document = snapshot.GetOpenDocumentInCurrentContextWithChanges();
                if (document == null)
                {
                    // Razor cshtml returns a null document for some reason.
                    return null;
                }

                // the ConfigureAwait() calls are important,
                // otherwise we'll deadlock VS
                var semanticModel = await document.GetSemanticModelAsync().ConfigureAwait(false);
                var syntaxRoot = await document.GetSyntaxRootAsync().ConfigureAwait(false);
                return new Cache {
                    Workspace = workspace,
                    Document = document,
                    SemanticModel = semanticModel,
                    SyntaxRoot = syntaxRoot,
                    Snapshot = snapshot
                };
            }
        }
    }
}
