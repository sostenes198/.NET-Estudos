using System;
using System.Collections.Generic;
using System.Text;
using WhereDynamic.Atributo;
using WhereDynamic.Entidade;
using WhereDynamic.Enum;

namespace WhereDynamic.Filtros
{
    public class FiltroPessoa
    {
        public FiltroPessoa()
        {
            Endereco = new FiltroEndereco();
        }

        [WhereDynamic(nameof(Pessoa.Id))]
        public int Codigo { get; set; }

        [WhereDynamic(nameof(Pessoa.Nome))]
        public string Nome { get; set; }

        [WhereDynamic(nameof(Pessoa.Idade))]
        public int AnosVivido { get; set; }

        public DateTime DataNascimento { get; set; }

        [WhereDynamic(nameof(Pessoa.Sexo))]
        public Sexo Genero{ get; set; }

        [WhereDynamic(nameof(Pessoa.Endereco))]
        public FiltroEndereco Endereco { get; set; }
    }
}
