using System;
using Optional;
using Optional.Unsafe;

namespace SharpCode
{
    /// <summary>
    /// Provides functionality for building properties. <see cref="PropertyBuilder"/> instances are <b>not</b>
    /// immutable.
    /// </summary>
    public class PropertyBuilder
    {
        internal PropertyBuilder()
        {
        }

        internal PropertyBuilder(AccessModifier accessModifier, string type, string name)
        {
            Property = Property.With(
                accessModifier: Option.Some(accessModifier),
                type: Option.Some(type),
                name: Option.Some(name));
        }

        internal PropertyBuilder(AccessModifier accessModifier, Type type, string name)
            : this(accessModifier, type.Name, name)
        {
        }

        internal Property Property { get; private set; } = new Property(
            AccessModifier.Public,
            getter: Option.Some<string?>(null),
            setter: Option.Some<string?>(null));

        /// <summary>
        /// Sets the access modifier of the property being built.
        /// </summary>
        public PropertyBuilder WithAccessModifier(AccessModifier accessModifier)
        {
            Property = Property.With(accessModifier: Option.Some(accessModifier));
            return this;
        }

        /// <summary>
        /// Sets the type of the property being built.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// The specified <paramref name="type"/> is <c>null</c>.
        /// </exception>
        public PropertyBuilder WithType(string type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Property = Property.With(type: Option.Some(type));
            return this;
        }

        /// <summary>
        /// Sets the type of the property being built.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// The specified <paramref name="type"/> is <c>null</c>.
        /// </exception>
        public PropertyBuilder WithType(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Property = Property.With(type: Option.Some(type.Name));
            return this;
        }

        /// <summary>
        /// Sets the name of the property being built.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// The specified <paramref name="name"/> is <c>null</c>.
        /// </exception>
        public PropertyBuilder WithName(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Property = Property.With(name: Option.Some(name));
            return this;
        }

        /// <summary>
        /// Sets the getter logic of the property being built.
        /// </summary>
        /// <param name="expression">
        /// The expression or block body of the getter. If not specified the default getter will be used - <c>get;</c>.
        /// </param>
        /// <example>
        /// This example shows the generated code for a property with a default getter.
        /// <code>
        /// // PropertyBuilder.WithName("Identifier").WithGetter()
        /// public int Identifier
        /// {
        ///     get;
        /// }
        /// </code>
        /// </example>
        /// <example>
        /// This example shows the generated code for a property with a getter that is an expression.
        /// <code>
        /// // PropertyBuilder.WithName("Identifier").WithGetter("_id")
        /// public int Identifier
        /// {
        ///     get => _id;
        /// }
        /// </code>
        /// </example>
        /// <example>
        /// This example shows the generated code for a property with a getter that has a block body.
        /// <code>
        /// // PropertyBuilder.WithName("IsCreated").WithGetter("{ if (_id == -1) { return false; } else { return true; } }")
        /// public int Identifier
        /// {
        ///     get
        ///     {
        ///         if (_id == -1)
        ///         {
        ///             return false;
        ///         }
        ///         else
        ///         {
        ///             return true;
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        public PropertyBuilder WithGetter(string? expression = null)
        {
            Property = Property.With(getter: Option.Some(Option.Some(expression)));
            return this;
        }

        /// <summary>
        /// Specifies that the property being built should not have a getter.
        /// </summary>
        public PropertyBuilder WithoutGetter()
        {
            Property = Property.With(getter: Option.Some(Option.None<string?>()));
            return this;
        }

        /// <summary>
        /// Sets the setter logic of the property being built.
        /// </summary>
        /// <param name="expression">
        /// The expression or block body of the setter. If not specified the default setter will be used - <c>set;</c>
        /// Custom setter logic can make use of the <c>value</c> provided to the property setter.
        /// </param>
        /// <example>
        /// This example shows the generated code for a property with a default setter.
        /// <code>
        /// // PropertyBuilder.WithName("Identifier").WithSetter()
        /// public int Identifier
        /// {
        ///     set;
        /// }
        /// </code>
        /// </example>
        /// <example>
        /// This example shows the generated code for a property with a setter that is an expression.
        /// <code>
        /// // PropertyBuilder.WithName("Identifier").WithSetter("_id = value")
        /// public int Identifier
        /// {
        ///     set => _id = value;
        /// }
        /// </code>
        /// </example>
        /// <example>
        /// This example shows the generated code for a property with a setter that has a block body.
        /// <code>
        /// // PropertyBuilder.WithName("Value")
        /// //    .WithSetter("{ if (value &lt;= 0) { throw new Exception(); } _value = value; }")
        /// public int Identifier
        /// {
        ///     set
        ///     {
        ///         if (_id == -1)
        ///         {
        ///             return false;
        ///         }
        ///         else
        ///         {
        ///             return true;
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        public PropertyBuilder WithSetter(string? expression = null)
        {
            Property = Property.With(setter: Option.Some(Option.Some(expression)));
            return this;
        }

        /// <summary>
        /// Specifies that the property being built should not have a setter.
        /// </summary>
        public PropertyBuilder WithoutSetter()
        {
            Property = Property.With(setter: Option.Some(Option.None<string?>()));
            return this;
        }

        /// <summary>
        /// Sets the default value of the property being built.
        /// </summary>
        /// <param name="defaultValue">
        /// The default value of the property. The value is used as-is, therefore you should wrap any string values in
        /// escaped quotes. For example,
        /// <c>WithDefaultValue(defaultValue: "\"this will be generated as a string\"")</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The specified <paramref name="defaultValue"/> is <c>null</c>.
        /// </exception>
        public PropertyBuilder WithDefaultValue(string defaultValue)
        {
            if (defaultValue is null)
            {
                throw new ArgumentNullException(nameof(defaultValue));
            }

            Property = Property.With(defaultValue: Option.Some(defaultValue));
            return this;
        }

        /// <summary>
        /// Sets whether the property being built should be static or not.
        /// </summary>
        /// <param name="makeStatic">
        /// Indicates whether the property should be static or not.
        /// </param>
        public PropertyBuilder MakeStatic(bool makeStatic = true)
        {
            Property = Property.With(isStatic: Option.Some(makeStatic));
            return this;
        }

        /// <summary>
        /// Adds XML summary documentation to the property.
        /// </summary>
        /// <param name="summary">
        /// The content of the summary documentation.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The specified <paramref name="summary"/> is <c>null</c>.
        /// </exception>
        public PropertyBuilder WithSummary(string summary)
        {
            if (summary is null)
            {
                throw new ArgumentNullException(nameof(summary));
            }

            Property = Property.With(summary: Option.Some<string>(summary));
            return this;
        }

        /// <summary>
        /// Returns the source code of the built property.
        /// </summary>
        /// <param name="formatted">
        /// Indicates whether to format the source code.
        /// </param>
        public string ToSourceCode(bool formatted = true) =>
            Build().ToSourceCode(formatted);

        /// <summary>
        /// Returns the source code of the built property.
        /// </summary>
        public override string ToString() =>
            ToSourceCode();

        internal Property Build()
        {
            if (string.IsNullOrWhiteSpace(Property.Name.ValueOrDefault()))
            {
                throw new MissingBuilderSettingException(
                    "Providing the name of the property is required when building a property.");
            }
            else if (string.IsNullOrWhiteSpace(Property.Type.ValueOrDefault()))
            {
                throw new MissingBuilderSettingException(
                    "Providing the type of the property is required when building a property.");
            }
            else if (Property.Setter.Exists(value => string.IsNullOrWhiteSpace(value)))
            {
                if (!Property.Getter.HasValue)
                {
                    throw new SyntaxException(
                        "Properties with auto implemented setters must also have auto implemented getters. (CS8051)");
                }
                else if (Property.Getter.Exists(value => !string.IsNullOrWhiteSpace(value)))
                {
                    throw new SyntaxException(
                        "Properties with custom getters cannot have auto implemented setters.");
                }
            }
            else if (Property.DefaultValue.HasValue)
            {
                if (Property.Getter.Exists(value => !string.IsNullOrWhiteSpace(value)) ||
                    Property.Setter.Exists(value => !string.IsNullOrWhiteSpace(value)))
                {
                    throw new SyntaxException("Only auto implemented properties can have a default value. (CS8050)");
                }
            }

            return Property;
        }
    }
}
