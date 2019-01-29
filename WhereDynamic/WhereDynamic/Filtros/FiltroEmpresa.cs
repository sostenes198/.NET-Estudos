using System.Collections.Generic;
using WhereDynamic.Atributo;
using WhereDynamic.Entidade;

namespace WhereDynamic.Filtros
{
    public class FiltroEmpresa
    {
        //[WhereDynamic(nameof(Empresa.Id))]
        public int Codigo { get; set; }

        public string NomeEmpresa { get; set; }

        [WhereDynamic(nameof(Empresa.Colaboradores))]
        public IEnumerable<FiltroPessoa> Pessoas { get; set; }
    }
}