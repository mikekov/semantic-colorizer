using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace SemanticColorizer
{
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.FieldFormat)]
    [Name(Constants.FieldFormat)]
    [UserVisible(true)]
    [Order(After = Priority.High)]
    internal sealed class SemanticFieldFormat : ClassificationFormatDefinition
    {
        public SemanticFieldFormat() {
            this.DisplayName = "Semantic Field";
            //this.ForegroundColor = Colors.SaddleBrown;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.EnumFieldFormat)]
    [Name(Constants.EnumFieldFormat)]
    [UserVisible(true)]
    [Order(After = Priority.High)]
    internal sealed class SemanticEnumFieldFormat : ClassificationFormatDefinition
    {
        public SemanticEnumFieldFormat() {
            this.DisplayName = "Semantic Enum Field";
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ExtensionMethodFormat)]
    [Name(Constants.ExtensionMethodFormat)]
    [UserVisible(true)]
    [Order(After = Priority.High)]
    internal sealed class SemanticExtensionMethodFormat : ClassificationFormatDefinition
    {
        public SemanticExtensionMethodFormat() {
            this.DisplayName = "Semantic Extension Method";
//			TextDecorations = System.Windows.TextDecorations.Underline;
//            this.IsItalic = true;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.StaticMethodFormat)]
    [Name(Constants.StaticMethodFormat)]
    [UserVisible(true)]
    [Order(After = Priority.Default)]
    internal sealed class SemanticStaticMethodFormat : ClassificationFormatDefinition
    {
        public SemanticStaticMethodFormat() {
            this.DisplayName = "Semantic Static Method";
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.NormalMethodFormat)]
    [Name(Constants.NormalMethodFormat)]
    [UserVisible(true)]
    [Order(After = Priority.Default)]
    internal sealed class SemanticNormalMethodFormat : ClassificationFormatDefinition
    {
        public SemanticNormalMethodFormat() {
            this.DisplayName = "Semantic Normal Method";
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ConstructorFormat)]
    [Name(Constants.ConstructorFormat)]
    [UserVisible(true)]
    [Order(After = Priority.Default)]
    internal sealed class SemanticConstructorFormat : ClassificationFormatDefinition
    {
        public SemanticConstructorFormat() {
            this.DisplayName = "Semantic Constructor";
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.TypeParameterFormat)]
    [Name(Constants.TypeParameterFormat)]
    [UserVisible(true)]
    [Order(After = Priority.Default)]
    internal sealed class SemanticTypeParameterFormat : ClassificationFormatDefinition
    {
        public SemanticTypeParameterFormat() {
            this.DisplayName = "Semantic Type Parameter";
            //this.ForegroundColor = Colors.SlateGray;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ParameterFormat)]
    [Name(Constants.ParameterFormat)]
    [UserVisible(true)]
    [Order(After = Priority.High)]
    internal sealed class SemanticParameterFormat : ClassificationFormatDefinition
    {
        public SemanticParameterFormat() {
            this.DisplayName = "Semantic Parameter";
            //this.ForegroundColor = Colors.SlateGray;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.NamespaceFormat)]
    [Name(Constants.NamespaceFormat)]
    [UserVisible(true)]
    [Order(After = Priority.High)]
    internal sealed class SemanticNamespaceFormat : ClassificationFormatDefinition
    {
        public SemanticNamespaceFormat() {
            this.DisplayName = "Semantic Namespace";
            //this.ForegroundColor = Colors.LimeGreen;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.PropertyFormat)]
    [Name(Constants.PropertyFormat)]
    [UserVisible(true)]
    [Order(After = Priority.Default)]
    internal sealed class SemanticPropertyFormat : ClassificationFormatDefinition
    {
        public SemanticPropertyFormat() {
            this.DisplayName = "Semantic Property";
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.LocalFormat)]
    [Name(Constants.LocalFormat)]
    [UserVisible(true)]
    [Order(After = Priority.Default)]
    internal sealed class SemanticLocalFormat : ClassificationFormatDefinition
    {
        public SemanticLocalFormat() {
            this.DisplayName = "Semantic Local";
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.TypeSpecialFormat)]
    [Name(Constants.TypeSpecialFormat)]
    [UserVisible(true)]
    [Order(After = Priority.High)]
    internal sealed class SemanticTypeSpecialFormat : ClassificationFormatDefinition
    {
        public SemanticTypeSpecialFormat() {
            this.DisplayName = "Semantic Special Type";
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.TypeNormalFormat)]
    [Name(Constants.TypeNormalFormat)]
    [UserVisible(true)]
    [Order(After = Priority.Default)]
    internal sealed class SemanticTypeNormalFormat : ClassificationFormatDefinition
    {
        public SemanticTypeNormalFormat() {
            this.DisplayName = "Semantic Normal Type";
        }
    }

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.ModifierFormat)]
	[Name(Constants.ModifierFormat)]
	[UserVisible(true)]
	[Order(After = Priority.High)]
	internal sealed class SemanticTypeAccessModifier : ClassificationFormatDefinition
	{
		public SemanticTypeAccessModifier()
		{
			DisplayName = "Semantic Modifier";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.KeywordFormat)]
	[Name(Constants.KeywordFormat)]
	[UserVisible(true)]
	[Order(After = Priority.Default)]
	internal sealed class SemanticKeywordFormat : ClassificationFormatDefinition
	{
		public SemanticKeywordFormat()
		{
			DisplayName = "Semantic Keyword";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.ConstantFormat)]
	[Name(Constants.ConstantFormat)]
	[UserVisible(true)]
	[Order(After = Priority.High)]
	internal sealed class SemanticConstantFormat : ClassificationFormatDefinition
	{
		public SemanticConstantFormat()
		{
			DisplayName = "Semantic Constant";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.DeclarationFormat)]
	[Name(Constants.DeclarationFormat)]
	[UserVisible(true)]
	[Order(After = Priority.High)]
	internal sealed class SemanticDeclarationFormat : ClassificationFormatDefinition
	{
		public SemanticDeclarationFormat()
		{
			DisplayName = "Semantic Declaration";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.EventFormat)]
	[Name(Constants.EventFormat)]
	[UserVisible(true)]
	[Order(After = Priority.High)]
	internal sealed class SemanticEventFormat : ClassificationFormatDefinition
	{
		public SemanticEventFormat()
		{
			DisplayName = "Semantic Event";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.AttributeFormat)]
	[Name(Constants.AttributeFormat)]
	[UserVisible(true)]
	[Order(After = Priority.High)]
	internal sealed class SemanticAttributeFormat : ClassificationFormatDefinition
	{
		public SemanticAttributeFormat()
		{
			DisplayName = "Semantic Attribute";
		}
	}

}
