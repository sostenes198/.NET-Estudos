using System;
using Estudos.IdempotentConsumer.Utils;
using FluentAssertions;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.Utils;

public class TypeUtilsTest
{
    private readonly Type _baseType = typeof(ImplementationBase<>);

    [Fact(DisplayName = "Deve obter tipo genérico")]
    public void ShouldGetGenericType()
    {
        // act - act
        var resultType = TypeUtils.GetGenericBaseType<Implementation1>(_baseType)!;

        // assert
        var result = resultType.GetGenericTypeDefinition() == _baseType.GetGenericTypeDefinition();
        result.Should().BeTrue();
    }

    [Fact(DisplayName = "Deve retornar nullo quando tipo genérico não encontrado")]
    public void ShouldReturnNullWhenGenericTypeNotFound()
    {
        // act - act
        var resultType = TypeUtils.GetGenericBaseType<NoneImplementation>(_baseType);

        // assert
        resultType.Should().BeNull();
    }
    
    public class NoneImplementation
    {
        
    }
    
    public class Implementation1: Implementation<int>
    {
        
    }
    
    public abstract class Implementation<T>: ImplementationBase<T>
    {
        
    }
    
    // ReSharper disable once UnusedTypeParameter
    public abstract class ImplementationBase<T>
    {
        
    }
}