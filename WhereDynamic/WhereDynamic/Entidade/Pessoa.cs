using System;
using WhereDynamic.Enum;

namespace WhereDynamic.Entidade
{
    public class Pessoa
    {
        public Pessoa()
        {
            Endereco = new Endereco();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public DateTime DataNascimento { get; set; }
        public Sexo Sexo { get; set; }

        public Endereco Endereco { get; set; }
    }
}