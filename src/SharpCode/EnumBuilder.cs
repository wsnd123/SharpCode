using System;
using System.Collections.Generic;
using System.Linq;
using Optional;
using Optional.Collections;
using Optional.Unsafe;

namespace SharpCode
{
    /// <summary>
    /// Provides functionalty for building enum structures. <see cref="EnumBuilder"/> instances are <b>not</b>
    /// immutable.
    /// </summary>
    public class EnumBuilder : IHasMembers
    {
        private readonly List<EnumMemberBuilder> _members = new List<EnumMemberBuilder>();

        internal EnumBuilder()
        {
        }

        internal EnumBuilder(string name, AccessModifier accessModifier)
        {
            Enumeration.Name = name;
            Enumeration.AccessModifier = accessModifier;
        }

        internal Enumeration Enumeration { get; } = new Enumeration();

        /// <summary>
        /// Sets the access modifier of the enum being built.
        /// </summary>
        public EnumBuilder WithAccessModifier(AccessModifier accessModifier)
        {
            Enumeration.AccessModifier = accessModifier;
            return this;
        }

        /// <summary>
        /// Sets the name of the enum being built.
        /// </summary>
        public EnumBuilder WithName(string name)
        {
            Enumeration.Name = name;
            return this;
        }

        /// <summary>
        /// Adds a member to the enum being built.
        /// </summary>
        public EnumBuilder WithMember(EnumMemberBuilder builder)
        {
            _members.Add(builder);
            return this;
        }

        /// <summary>
        /// Adds a bunch of members to the enum being built.
        /// </summary>
        public EnumBuilder WithMembers(params EnumMemberBuilder[] builders)
        {
            _members.AddRange(builders);
            return this;
        }

        /// <summary>
        /// Adds a bunch of members to the enum being built.
        /// </summary>
        public EnumBuilder WithMembers(IEnumerable<EnumMemberBuilder> builders)
        {
            _members.AddRange(builders);
            return this;
        }

        /// <summary>
        /// Specifies whether the enum being built represents a set of flags or not. A flags enum will be marked with
        /// the <see cref="FlagsAttribute"/> in the generated source code. The members of a flags enum will be assigned
        /// appropriate, auto-generated values. <b>Note</b> that explicitly set values will be overwritten.
        /// </summary>
        /// <param name="makeFlagsEnum">
        /// Indicates whether the enum represents a set of flags.
        /// </param>
        public EnumBuilder MakeFlagsEnum(bool makeFlagsEnum = true)
        {
            Enumeration.IsFlag = makeFlagsEnum;
            return this;
        }

        /// <summary>
        /// Adds XML summary documentation to the enum.
        /// </summary>
        /// <param name="summary">
        /// The content of the summary documentation.
        /// </param>
        public EnumBuilder WithSummary(string summary)
        {
            Enumeration.Summary = Option.Some<string?>(summary);
            return this;
        }

        /// <inheritdoc/>
        public bool HasMember(
            string name,
            MemberType memberType = MemberType.Any,
            StringComparison comparison = StringComparison.InvariantCultureIgnoreCase) =>
            memberType switch
            {
                MemberType.EnumMember => _members.Any(x => x.EnumerationMember.Name.Exists(n => n.Equals(name, comparison))),
                MemberType.Any => HasMember(name, MemberType.EnumMember, comparison),
                _ => false
            };

        /// <summary>
        /// Returns the source code of the built enum.
        /// </summary>
        /// <param name="formatted">
        /// Indicates whether to format the source code.
        /// </param>
        public string ToSourceCode(bool formatted = true) =>
            Build().ToSourceCode(formatted);

        /// <summary>
        /// Returns the source code of the built enum.
        /// </summary>
        public override string ToString() =>
            ToSourceCode();

        internal Enumeration Build()
        {
            if (string.IsNullOrWhiteSpace(Enumeration.Name))
            {
                throw new MissingBuilderSettingException(
                    "Providing the name of the enum is required when building an enum.");
            }

            if (Enumeration.IsFlag && _members.All(x => !x.EnumerationMember.Value.HasValue))
            {
                for (var i = 0; i < _members.Count; i++)
                {
                    _members[i] = _members[i].WithValue(i == 0 ? 0 : (int)Math.Pow(2, i - 1));
                }
            }

            Enumeration.Members.AddRange(_members.Select(x => x.Build()));
            Enumeration.Members
                .GroupBy(x => x.Name.ValueOrFailure())
                .Where(x => x.AtLeast(2))
                .Select(x => x.Key)
                .FirstOrNone()
                .MatchSome(duplicateMemberName => throw new SyntaxException(
                    $"The enum '{Enumeration.Name}' already contains a definition for '{duplicateMemberName}'."));

            return Enumeration;
        }
    }
}
