using System;
using System.Windows.Markup;

namespace FinancePortfolioDatabase
{
    /// <summary>
    /// A markup type to enable the display of generics. For example,
    /// to display a type Labelled{T,S}, one can use this class with
    /// Labelled as the BaseType, and the specifics of T and S
    /// as the InnerTypes.
    /// </summary>
    /// <example>
    /// financeportfoliodatabase:GenericType BaseType="{x:Type TypeName=cs:Labelled`2}" InnerTypes="{StaticResource ListWithTwoStringTypes}" x:Key="DictionaryStringString"
    /// </example>
    public sealed class GenericType : MarkupExtension
    {
        /// <summary>
        /// The main type to use.
        /// </summary>
        public Type BaseType
        {
            get; set;
        }

        /// <summary>
        /// Provides a list of the generic types used in the Base.
        /// </summary>
        public Type[] InnerTypes
        {
            get; set;
        }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public GenericType()
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GenericType(Type baseType, params Type[] innerTypes)
        {
            BaseType = baseType;
            InnerTypes = innerTypes;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Type result = BaseType.MakeGenericType(InnerTypes);
            return result;
        }
    }
}