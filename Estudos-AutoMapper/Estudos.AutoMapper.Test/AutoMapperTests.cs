using System;
using AutoMapper;
using Estudos.AutoMapper.TestesAutoMapper;
using FluentAssertions;
using Xunit;

namespace Estudos.AutoMapper.Test
{
    public class AutoMapperTests
    {
        private string Debugar(ClasseTesteDto classeTesteDto)
        {
            return ((char) classeTesteDto.TesteEnum).ToString();
        }

        [Fact]
        public void ClasseTesteChar_Testes_Automapper_Retorno_Char()
        {
            var config = new MapperConfiguration(
                cfg =>
                    cfg.CreateMap<ClasseTesteDto, ClasseTesteChar>()
                        .ForMember(dest => dest.TesteChar, opt => opt.MapFrom(dest => (char) dest.TesteEnum))
            );
            var mapper = config.CreateMapper();

            var resultadoEsperado = (char) TesteEnum.Teste1;
            var teste = new ClasseTesteDto {TesteEnum = TesteEnum.Teste1};
            var resultado = mapper.Map<ClasseTesteChar>(teste);
            resultado.TesteChar.Should().Be(resultadoEsperado);
        }

        [Fact]
        public void ClasseTesteChar_Testes_Automapper_Retorno_String()
        {
            var config = new MapperConfiguration(
                cfg =>
                    cfg.CreateMap<ClasseTesteDto, ClasseTesteChar>()
                        .ForMember(dest => dest.TesteChar, opt => opt.MapFrom(dest => dest.TesteEnum))
            );
            var mapper = config.CreateMapper();

            var teste = new ClasseTesteDto {TesteEnum = TesteEnum.Teste1};
            mapper.Invoking(lnq => lnq.Map<ClasseTesteChar>(teste))
                .Should().Throw<Exception>();
        }

        [Fact]
        public void ClasseTesteString_Testes_AutoMapper_Retorno_Char()
        {
            var config = new MapperConfiguration(
                cfg =>
                    cfg.CreateMap<ClasseTesteDto, ClasseTesteString>()
                        .ForMember(dest => dest.TesteString, opt => opt.MapFrom(dest => Debugar(dest)))
            );
            var mapper = config.CreateMapper();

            var resultadoEsperado = (char) TesteEnum.Teste1;
            var teste = new ClasseTesteDto {TesteEnum = TesteEnum.Teste1};
            var resultado = mapper.Map<ClasseTesteString>(teste);
            resultado.TesteString.Should().Be(resultadoEsperado.ToString());
        }

        [Fact]
        public void ClasseTesteString_Testes_AutoMapper_Retorno_String()
        {
            var config = new MapperConfiguration(
                cfg =>
                    cfg.CreateMap<ClasseTesteDto, ClasseTesteString>()
                        .ForMember(dest => dest.TesteString, opt => opt.MapFrom(dest => dest.TesteEnum))
            );
            var mapper = config.CreateMapper();

            var teste = new ClasseTesteDto {TesteEnum = TesteEnum.Teste1};
            var resultado = mapper.Map<ClasseTesteString>(teste);
            resultado.TesteString.Should().Be(TesteEnum.Teste1.ToString());
        }
    }
}