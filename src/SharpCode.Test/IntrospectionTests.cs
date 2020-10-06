using System;
using NUnit.Framework;

namespace SharpCode.Test
{
    [TestFixture]
    public class IntrospectionTests
    {
        [Test]
        public void ClassBuilder_HasMember_WorksWithFields()
        {
            var builder = Code.CreateClass(name: "Test")
                .WithFields(
                    Code.CreateField("string", "_name"),
                    Code.CreateField("bool", "_hasValue"));

            Assert.IsTrue(builder.HasMember("_name"));
            Assert.IsTrue(builder.HasMember("_name", MemberType.Field));

            Assert.IsTrue(builder.HasMember("_HaSValUE", MemberType.Field));
            Assert.IsFalse(builder.HasMember("_HaSValUE", MemberType.Field, StringComparison.Ordinal));

            Assert.IsFalse(builder.HasMember("_hasValue", MemberType.Property));
        }

        [Test]
        public void ClassBuilder_HasMember_WorksWithProperties()
        {
            var builder = Code.CreateClass(name: "Test")
                .WithProperties(
                    Code.CreateProperty("string", "Name"),
                    Code.CreateProperty("bool", "HasValue"));

            Assert.IsTrue(builder.HasMember("Name"));
            Assert.IsTrue(builder.HasMember("Name", MemberType.Property));

            Assert.IsTrue(builder.HasMember("HaSValUE", MemberType.Property));
            Assert.IsFalse(builder.HasMember("HaSValUE", MemberType.Property, StringComparison.Ordinal));

            Assert.IsFalse(builder.HasMember("Name", MemberType.Field));
        }

        [Test]
        public void ClassBuilder_HasMember_WorksWithAnyMember()
        {
            var builder = Code.CreateClass(name: "Test")
                .WithFields(
                    Code.CreateField("string", "_name"),
                    Code.CreateField("int", "_count"))
                .WithProperties(
                    Code.CreateProperty("string", "Name"),
                    Code.CreateProperty("bool", "HasValue"));

            Assert.IsTrue(builder.HasMember("_name"));
            Assert.IsFalse(builder.HasMember("_name", MemberType.Property));

            Assert.IsTrue(builder.HasMember("_count", MemberType.Field));
            Assert.IsTrue(builder.HasMember("_COUNT", MemberType.Field));

            Assert.IsTrue(builder.HasMember("Name"));
            Assert.IsTrue(builder.HasMember("Name", MemberType.Property));
            Assert.IsFalse(builder.HasMember("Name", MemberType.Field));

            Assert.IsTrue(builder.HasMember("HaSValUE", MemberType.Property));
            Assert.IsFalse(builder.HasMember("HaSValUE", MemberType.Field));
        }

        [Test]
        public void EnumBuilder_HasMember_Works()
        {
            var builder = Code.CreateEnum(name: "Test").WithMembers(
                Code.CreateEnumMember("None"),
                Code.CreateEnumMember("Some"));

            Assert.IsTrue(builder.HasMember("none"));
            Assert.IsTrue(builder.HasMember("some", MemberType.EnumMember));

            Assert.IsFalse(builder.HasMember("any", MemberType.Field));
            Assert.IsFalse(builder.HasMember("NONE", MemberType.EnumMember, StringComparison.Ordinal));
        }
    }
}
