using System;
using Estudos.DynamicProxy.Contracts;
using Estudos.DynamicProxy.Entities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Estudos.DynamicProxy.Test
{
    public class Test
    {
        [Fact]
        public void Deve_Validar_DynamicProxy_Com_Atributo()
        {
            // arrange
            var id = 1;
            var resultadoEsperado = new Blog
            {
                Id = id,
                Description = "description",
                Title = "test",
                CreatedAt = DateTime.Now.Date
            };
            var serviceCollection = BootStraper.CreateServiceProvider();
            var blogService = serviceCollection.GetRequiredService<IBlogService>();

            // act
            var resultado = blogService.Get(1);

            // assert
            resultado.Should().BeEquivalentTo(resultadoEsperado);

        }
        
        [Fact]
        public void Deve_Validar_DynamicProxy_Com_Atributo_Na_Interface()
        {
            // arrange
            var serviceCollection = BootStraper.CreateServiceProvider();
            var blogService = serviceCollection.GetRequiredService<IBlogService>();

            // act
            blogService.Invoking(lnq => lnq.Test()).Should().NotThrow();

            // assert

        }
        
        
        [Fact]
        public void Deve_Validar_DynamicProxy_Com_Atributo_Generico()
        {
            // arrange
            var blog = new Blog
            {
                Id = 1,
                Description = "description",
                Title = "test",
                CreatedAt = DateTime.Now.Date
            };
            var serviceCollection = BootStraper.CreateServiceProvider();
            var blogService = serviceCollection.GetRequiredService<IBlogService>();

            // act
            blogService.Invoking(lnq => lnq.Save(blog)).Should().NotThrow();

            // assert

        }
    }
}