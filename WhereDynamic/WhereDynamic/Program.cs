using System;
using System.Collections.Generic;
using WhereDynamic.Entidade;
using WhereDynamic.Enum;
using WhereDynamic.Extensions;
using WhereDynamic.Filtros;

namespace WhereDynamic
{
    class Program
    {
        static void Main(string[] args)
        {
            var pessoas = ConstruirPessoas();

            var filtro = new FiltroPessoa()
            {
                Codigo = 1,
                DataNascimento = DateTime.Now,
                AnosVivido = 18,
                Nome = "S",
                Genero = Sexo.Masculino,
                Endereco = new FiltroEndereco()
                {
                    Codigo = 1                    
                }
            };

            var resultado = pessoas.WhereDynamic(filtro);

            //WhereDynamicExtension.WhereDynamic(filtro);
        }


        public static IEnumerable<Pessoa> ConstruirPessoas()
        {
            return new List<Pessoa>()
            {
                new Pessoa(){Id = 1, DataNascimento = DateTime.Now, Idade = 18, Nome = "S", Sexo = Sexo.Masculino},
                new Pessoa(){Id = 2, DataNascimento = DateTime.Now, Idade = 18, Nome = "So", Sexo = Sexo.Feminino},
                new Pessoa(){Id = 3, DataNascimento = DateTime.Now, Idade = 18, Nome = "Sos", Sexo = Sexo.Masculino},
                new Pessoa(){Id = 4, DataNascimento = DateTime.Now, Idade = 18, Nome = "Sost", Sexo = Sexo.Feminino},
                new Pessoa(){Id = 5, DataNascimento = DateTime.Now, Idade = 18, Nome = "Soste", Sexo = Sexo.Masculino},
                new Pessoa(){Id = 6, DataNascimento = DateTime.Now, Idade = 18, Nome = "Sosten", Sexo = Sexo.Feminino},
                new Pessoa(){Id = 7, DataNascimento = DateTime.Now, Idade = 18, Nome = "Sostene", Sexo = Sexo.Masculino},
                new Pessoa(){Id = 8, DataNascimento = DateTime.Now, Idade = 18, Nome = "Sostenes", Sexo = Sexo.Feminino},
                new Pessoa(){Id = 9, DataNascimento = DateTime.Now, Idade = 18, Nome = "Sostenes G", Sexo = Sexo.Masculino},
                new Pessoa(){Id = 10, DataNascimento = DateTime.Now, Idade = 18, Nome = "Sostenes G S", Sexo = Sexo.Feminino}
            };
        }
    }
}
