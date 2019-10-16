using WhereDynamic.Atributo;
using WhereDynamic.Entidade;

namespace WhereDynamic.Filtros
{
    public class FiltroCidade
    {
        [WhereDynamic(nameof(Cidade.Id))] public int Codigo { get; set; }

        [WhereDynamic(nameof(Cidade.UF))] public string UF { get; set; }

        public string Bairro { get; set; }

        public string Rua { get; set; }
    }
}