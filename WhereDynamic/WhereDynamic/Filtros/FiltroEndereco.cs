using System;
using System.Collections.Generic;
using System.Text;
using WhereDynamic.Atributo;
using WhereDynamic.Entidade;

namespace WhereDynamic.Filtros
{
    public class FiltroEndereco
    {
        public FiltroEndereco()
        {

        }

        [WhereDynamic(nameof(Endereco.Id))]
        public int Codigo { get; set; }

        [WhereDynamic(nameof(Endereco.Pais))]
        public string Pais { get; set; }

        public string UF { get; set; }

        public string Bairro { get; set; }

        public string Rua { get; set; }
    }
}
