using System.Reflection;
using FluentAssertions;

namespace Scheduled.Message.Tests.Base.Utils;

public static class AttributeValidation
{
    public static void ValidateAttribute<TAttribute>(this Type type, IList<AttributeProperty<TAttribute>> properties)
        where TAttribute : Attribute
    {
        foreach (var property in properties)
        {
            var propertyInfo = type.GetProperty(property.Property);
            propertyInfo.Should().NotBeNull(property.Property);

            var customAttribute = propertyInfo!.GetCustomAttribute<TAttribute>();
            customAttribute.Should().NotBeNull(property.Property);
            property.ValidateAttribute.Invoke(customAttribute!);
        }

        var expectedProperties = type.GetProperties().Where(lnq => lnq.GetCustomAttribute<TAttribute>() is not null).Select(lnq => lnq.Name).OrderBy(t => t);
        var orderingProperties = properties.Select(lnq => lnq.Property).OrderBy(t => t).ToList();
        orderingProperties.Should().BeEquivalentTo(expectedProperties);
    }
}

public class AttributeProperty<TAttribute>
    where TAttribute : Attribute
{
    public string Property { get; init; } = null!;

    public Action<TAttribute> ValidateAttribute { get; init; } = null!;
}