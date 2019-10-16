using System.Collections.Generic;
using AutoMapper;
using Estudos.Abstract.Servico.DTOs.DTO_Cardapio;
using Estudos.AutoMapper.Base;
using Estudos.Dominio.Entidades.Entidades_Cardapio;

namespace Estudos.AutoMapper.Mapper_Profile.Profile_CardapioCategoria
{
    public class CardapioCategoriaProfile : Profile, IProfile
    {
        public CardapioCategoriaProfile()
        {
            CreateMap<CardapioCategoriaDTO, CardapioCategoria>()
                .ReverseMap();

            CreateMap<List<CardapioCategoriaDTO>, List<CardapioCategoria>>()
                .ReverseMap();
        }
    }
}