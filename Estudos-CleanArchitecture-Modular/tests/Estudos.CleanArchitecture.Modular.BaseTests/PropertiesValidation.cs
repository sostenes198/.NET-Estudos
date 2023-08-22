using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace Estudos.CleanArchitecture.Modular.BaseTests
{
    [ExcludeFromCodeCoverage]
    public static class PropertiesValidation
    {
        public static void ValidateProperties(this Type type, IList<AssertProperty> expectedProperties)
        {
            var fields = type.GetFields()
               .Select(lnq => new AssertProperty {Name = lnq.Name, Type = lnq.FieldType})
               .ToList();

            var properties = type.GetProperties()
               .Select(lnq => new AssertProperty {Name = lnq.Name, Type = lnq.PropertyType})
               .ToList();

            foreach (var expectedProperty in expectedProperties)
            {
                var field = fields.FirstOrDefault(lnq => lnq.Name == expectedProperty.Name);

                var property = properties.FirstOrDefault(lnq => lnq.Name == expectedProperty.Name);

                if (field != null)
                {
                    field.Name.Should().BeEquivalentTo(expectedProperty.Name);
                    field.Type.Should().Be(expectedProperty.Type);
                    fields.Remove(field);
                }

                if (property != null)
                {
                    property.Name.Should().BeEquivalentTo(expectedProperty.Name);
                    property.Type.Should().Be(expectedProperty.Type);
                    properties.Remove(property);
                }
            }

            fields.Count.Should().Be(0);
            properties.Count.Should().Be(0);
        }
    }

    public class AssertProperty
    {
        public string Name { get; init; } = null!;

        public Type Type { get; init; } = null!;
    }
}