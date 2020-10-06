using System.Collections.Generic;
using Optional;

namespace SharpCode
{
    /// <summary>
    /// Represents the access modifier of a C# structure.
    /// </summary>
    public enum AccessModifier
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        None,
        Private,
        Internal,
        Protected,
        Public,
        PrivateInternal,
        ProtectedInternal,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// Represents the various types of C# structure members.
    /// </summary>
    public enum MemberType
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Any,
        Interface,
        Class,
        Struct,
        Enum,
        EnumMember,
        Field,
        Property,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    internal readonly struct Field
    {
        public Field(
            AccessModifier accessModifier,
            bool isReadonly = false,
            Option<string> type = default,
            Option<string> name = default,
            Option<string> summary = default)
        {
            AccessModifier = accessModifier;
            IsReadonly = isReadonly;
            Type = type;
            Name = name;
            Summary = summary;
        }

        public readonly AccessModifier AccessModifier { get; }

        public readonly bool IsReadonly { get; }

        public readonly Option<string> Type { get; }

        public readonly Option<string> Name { get; }

        public readonly Option<string> Summary { get; }

        public readonly Field With(
            Option<AccessModifier> accessModifier = default,
            Option<bool> isReadonly = default,
            Option<string> type = default,
            Option<string> name = default,
            Option<string> summary = default) =>
            new Field(
                accessModifier.ValueOr(AccessModifier),
                isReadonly.ValueOr(IsReadonly),
                type.Else(Type),
                name.Else(Name),
                summary.Else(Summary));
    }

    internal readonly struct Property
    {
        public Property(
            AccessModifier accessModifier,
            bool isStatic = false,
            Option<string> type = default,
            Option<string> name = default,
            Option<string> summary = default,
            Option<string> defaultValue = default,
            Option<string?> getter = default,
            Option<string?> setter = default)
        {
            AccessModifier = accessModifier;
            IsStatic = isStatic;
            Type = type;
            Name = name;
            Summary = summary;
            DefaultValue = defaultValue;
            Getter = getter;
            Setter = setter;
        }

        public readonly AccessModifier AccessModifier { get; }

        public readonly bool IsStatic { get; }

        public readonly Option<string> Type { get; }

        public readonly Option<string> Name { get; }

        public readonly Option<string> Summary { get; }

        public readonly Option<string> DefaultValue { get; }

        public readonly Option<string?> Getter { get; }

        public readonly Option<string?> Setter { get; }

        public readonly Property With(
            Option<AccessModifier> accessModifier = default,
            Option<bool> isStatic = default,
            Option<string> type = default,
            Option<string> name = default,
            Option<string> summary = default,
            Option<string> defaultValue = default,
            Option<Option<string?>> getter = default,
            Option<Option<string?>> setter = default) =>
            new Property(
                accessModifier: accessModifier.ValueOr(AccessModifier),
                isStatic: isStatic.ValueOr(IsStatic),
                type: type.Else(Type),
                name: name.Else(Name),
                summary: summary.Else(Summary),
                defaultValue: defaultValue.Else(DefaultValue),
                getter: getter.ValueOr(Getter),
                setter: setter.ValueOr(Setter));
    }

    internal readonly struct EnumerationMember
    {
        public EnumerationMember(Option<string> name, Option<int> value = default, Option<string> summary = default)
        {
            Name = name;
            Value = value;
            Summary = summary;
        }

        public readonly Option<string> Name { get; }

        public readonly Option<int> Value { get; }

        public readonly Option<string> Summary { get; }

        public readonly EnumerationMember With(
            Option<string> name = default,
            Option<int> value = default,
            Option<string> summary = default) =>
            new EnumerationMember(
                name: name.Else(Name),
                value: value.Else(value),
                summary: summary.Else(summary));
    }

    internal readonly struct Parameter
    {
        public Parameter(string type, string name, Option<string> receivingMember = default)
        {
            Type = type;
            Name = name;
            ReceivingMember = receivingMember;
        }

        public readonly string Type { get;  }

        public readonly string Name { get;  }

        public readonly Option<string> ReceivingMember { get;  }
    }

    internal readonly struct Constructor
    {
        public Constructor(
            AccessModifier accessModifier,
            bool isStatic = false,
            Option<string> className = default,
            Option<string> summary = default,
            Option<List<Parameter>> parameters = default,
            Option<IEnumerable<string>> baseCallParameters = default)
        {
            AccessModifier = accessModifier;
            IsStatic = isStatic;
            ClassName = className;
            Summary = summary;
            Parameters = parameters.ValueOr(new List<Parameter>());
            BaseCallParameters = baseCallParameters;
        }

        public readonly AccessModifier AccessModifier { get; }

        public readonly bool IsStatic { get; }

        public readonly Option<string> ClassName { get; }

        public readonly Option<string> Summary { get; }

        public readonly List<Parameter> Parameters { get; }

        public readonly Option<IEnumerable<string>> BaseCallParameters { get; }

        public readonly Constructor With(
            Option<AccessModifier> accessModifier = default,
            Option<bool> isStatic = default,
            Option<string> className = default,
            Option<string> summary = default,
            Option<IEnumerable<string>> baseCallParameters = default) =>
            new Constructor(
                accessModifier: accessModifier.ValueOr(AccessModifier),
                isStatic: isStatic.ValueOr(IsStatic),
                className: className.Else(ClassName),
                summary: summary.Else(Summary),
                parameters: Option.Some(Parameters),
                baseCallParameters: baseCallParameters.Else(BaseCallParameters));
    }

    internal readonly struct Class
    {
        public Class(
            AccessModifier accessModifier,
            bool isStatic = false,
            Option<string> name = default,
            Option<string> summary = default,
            Option<string> inheritedClass = default,
            Option<List<string>> implementedInterfaces = default,
            Option<List<Field>> fields = default,
            Option<List<Property>> properties = default,
            Option<List<Constructor>> constructors = default)
        {
            AccessModifier = accessModifier;
            IsStatic = isStatic;
            Name = name;
            Summary = summary;
            InheritedClass = inheritedClass;
            ImplementedInterfaces = implementedInterfaces.ValueOr(new List<string>());
            Fields = fields.ValueOr(new List<Field>());
            Properties = properties.ValueOr(new List<Property>());
            Constructors = constructors.ValueOr(new List<Constructor>());
        }

        public readonly AccessModifier AccessModifier { get; }

        public readonly bool IsStatic { get; }

        public readonly Option<string> Name { get; }

        public readonly Option<string> Summary { get; }

        public readonly Option<string> InheritedClass { get; }

        public readonly List<string> ImplementedInterfaces { get; }

        public readonly List<Field> Fields { get; }

        public readonly List<Property> Properties { get; }

        public readonly List<Constructor> Constructors { get; }

        public readonly Class With(
            Option<AccessModifier> accessModifier = default,
            Option<bool> isStatic = default,
            Option<string> name = default,
            Option<string> summary = default,
            Option<string> inheritedClass = default) =>
            new Class(
                accessModifier: accessModifier.ValueOr(AccessModifier),
                isStatic: isStatic.ValueOr(IsStatic),
                name: name.Else(Name),
                summary: summary.Else(Summary),
                inheritedClass: inheritedClass.Else(inheritedClass),
                implementedInterfaces: Option.Some(ImplementedInterfaces),
                fields: Option.Some(Fields),
                properties: Option.Some(Properties),
                constructors: Option.Some(Constructors));
    }

    internal class Struct
    {
        public AccessModifier AccessModifier { get; set; } = AccessModifier.Public;

        public string? Name { get; set; }

        public Option<string?> Summary { get; set; } = Option.None<string?>();

        public List<string?> ImplementedInterfaces { get; } = new List<string?>();

        public List<Constructor> Constructors { get; } = new List<Constructor>();

        public List<Field> Fields { get; } = new List<Field>();

        public List<Property> Properties { get; } = new List<Property>();
    }

    internal class Interface
    {
        public AccessModifier AccessModifier { get; set; } = AccessModifier.Public;

        public string? Name { get; set; }

        public Option<string?> Summary { get; set; } = Option.None<string?>();

        public List<string> ImplementedInterfaces { get; } = new List<string>();

        public List<Property> Properties { get; } = new List<Property>();
    }

    internal class Enumeration
    {
        public AccessModifier AccessModifier { get; set; } = AccessModifier.Public;

        public string? Name { get; set; }

        public Option<string?> Summary { get; set; } = Option.None<string?>();

        public bool IsFlag { get; set; }

        public List<EnumerationMember> Members { get; } = new List<EnumerationMember>();
    }

    internal class Namespace
    {
        public string? Name { get; set; }

        public List<string> Usings { get; } = new List<string>();

        public List<Class> Classes { get; } = new List<Class>();

        public List<Struct> Structs { get; } = new List<Struct>();

        public List<Interface> Interfaces { get; } = new List<Interface>();

        public List<Enumeration> Enums { get; } = new List<Enumeration>();
    }
}
