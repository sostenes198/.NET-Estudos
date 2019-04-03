using System;
using System.Collections.Generic;
using System.Linq;
using WhereDynamic.Entidade;
using WhereDynamic.Enum;
using WhereDynamic.Extensions;
using WhereDynamic.Filtros;
using Xunit;

namespace WhereDynamic.Test
{
    public class WhereDynamicTest
    {
        [Theory]
        [MemberData(nameof(FiltrosFactWhereDynamic))]
        public void Deve_Retornar_Quantidade_Elemento_Predefinido(int valorEsperado, FiltroPessoa filtro)
        {
            IEnumerable<Pessoa> pessoas = ConstruirPessoas();

            IEnumerable<Pessoa> resultado = pessoas.WhereDynamic(filtro);

            Assert.Equal(valorEsperado, resultado.Count());
        }

        public static IEnumerable<object[]> FiltrosFactWhereDynamic =>
            new List<object[]>()
            {
                new object[]{1, new FiltroPessoa(){ Codigo =  1, DataNascimento = DateTime.Now,  AnosVivido = 18, Nome = "S", Genero = Sexo.Masculino, Endereco = new FiltroEndereco(){ Codigo = 1 } } },
                new object[]{2, new FiltroPessoa(){ Codigo =  11, DataNascimento = DateTime.Now,  AnosVivido = 18, Nome = "Sostenes Go S", Genero = Sexo.Masculino, Endereco = new FiltroEndereco(){ Codigo = 1, Cidade = new FiltroCidade(){ Codigo = 10, UF = "MG" } } } },
                new object[]{2, new FiltroPessoa(){ Codigo =  12, DataNascimento = DateTime.Now,  AnosVivido = 19, Nome = "Raquel Linda", Genero = Sexo.Feminino } }
            };

        public IEnumerable<Pessoa> ConstruirPessoas()
        {
            return new List<Pessoa>()
            {
                new Pessoa(){Id = 1, DataNascimento = DateTime.Now, Idade = 18, Nome = "S", Sexo = Sexo.Masculino, Endereco = new Endereco(){ Id = 1  } },
                new Pessoa(){Id = 2, DataNascimento = DateTime.Now, Idade = 18, Nome = "So", Sexo = Sexo.Feminino, Endereco = new Endereco(){ Id = 1  } },
                new Pessoa(){Id = 3, DataNascimento = DateTime.Now, Idade = 18, Nome = "Sos", Sexo = Sexo.Masculino},
                new Pessoa(){Id = 4, DataNascimento = DateTime.Now, Idade = 18, Nome = "Sost", Sexo = Sexo.Feminino},
                new Pessoa(){Id = 5, DataNascimento = DateTime.Now, Idade = 18, Nome = "Soste", Sexo = Sexo.Masculino},
                new Pessoa(){Id = 6, DataNascimento = DateTime.Now, Idade = 18, Nome = "Sosten", Sexo = Sexo.Feminino},
                new Pessoa(){Id = 7, DataNascimento = DateTime.Now, Idade = 18, Nome = "Sostene", Sexo = Sexo.Masculino},
                new Pessoa(){Id = 8, DataNascimento = DateTime.Now, Idade = 18, Nome = "Sostenes", Sexo = Sexo.Feminino},
                new Pessoa(){Id = 9, DataNascimento = DateTime.Now, Idade = 18, Nome = "Sostenes G", Sexo = Sexo.Masculino},
                new Pessoa(){Id = 10, DataNascimento = DateTime.Now, Idade = 18, Nome = "Sostenes G S", Sexo = Sexo.Feminino},
                new Pessoa(){Id = 11, DataNascimento = DateTime.Now, Idade = 18, Nome = "Sostenes Go S", Sexo = Sexo.Masculino, Endereco = new Endereco(){ Id = 1, Cidade = new Cidade(){ Id = 10, UF = "MG" } } },
                new Pessoa(){Id = 11, DataNascimento = DateTime.Now, Idade = 18, Nome = "Sostenes Go S", Sexo = Sexo.Masculino, Endereco = new Endereco(){ Id = 1, Cidade = new Cidade(){ Id = 10, UF = "MG" } } },
                new Pessoa(){Id = 12, DataNascimento = DateTime.Now, Idade = 19, Nome = "Raquel Linda", Sexo = Sexo.Feminino },
                new Pessoa(){Id = 12, DataNascimento = DateTime.Now, Idade = 19, Nome = "Raquel Linda", Sexo = Sexo.Feminino }
            };
    }
}
}
