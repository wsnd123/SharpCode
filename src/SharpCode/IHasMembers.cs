using System;

namespace SharpCode
{
    /// <summary>
    /// Allows the object to expose introspectiong data of its members.
    /// </summary>
    public interface IHasMembers
    {
        /// <summary>
        /// Checks whether the described member exists.
        /// </summary>
        /// <param name="name">
        /// The name of the member.
        /// </param>
        /// <param name="memberType">
        /// The type of the member. By default all members will be taken into account.
        /// </param>
        /// <param name="comparison">
        /// The comparision type to be performed when comparing the described name against the names of the actual
        /// members. By default casing is ignored.
        /// </param>
        bool HasMember(
            string name,
            MemberType memberType = MemberType.Any,
            StringComparison comparison = StringComparison.InvariantCultureIgnoreCase);
    }
}
