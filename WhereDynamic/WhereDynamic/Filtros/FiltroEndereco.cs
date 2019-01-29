using WhereDynamic.Atributo;
using WhereDynamic.Entidade;

namespace WhereDynamic.Filtros
{
    public class FiltroEndereco
    {
        //[WhereDynamic(nameof(Endereco.Id))]
        public int Codigo { get; set; }

        public string Pais { get; set; }


        [WhereDynamic(nameof(Endereco.Cidade))]
        public FiltroCidade Cidade { get; set; }
    }
}