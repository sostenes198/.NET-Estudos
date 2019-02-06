using AutoMapper;
using Estudos.Abstract.Servico.DTOs.DTO_Cardapio;
using Estudos.Dominio.Entidades.Entidades_Cardapio;
using System.Collections.Generic;

namespace Estudos.Abstract.Servico.AutoMapper_Profile.Profile_CardapioCategoria
{
    public class CardapioCategoriaProfile: Profile
    {
        public CardapioCategoriaProfile()
        {
            CreateMap<CardapioCategoriaDTO, CardapioCategoria>();

            CreateMap<CardapioCategoria, CardapioCategoriaDTO>();

            CreateMap<List<CardapioCategoriaDTO>, List<CardapioCategoria>>();

            CreateMap<List<CardapioCategoria>, List<CardapioCategoriaDTO>>();
        }
    }
}
