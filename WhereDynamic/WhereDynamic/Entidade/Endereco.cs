using System;
using System.Collections.Generic;
using System.Text;

namespace WhereDynamic.Entidade
{
    public class Endereco
    {
        public Endereco()
        {

        }

        public int Id { get; set; }
        public string Pais { get; set; }
        public string UF { get; set; }        
        public string Bairro { get; set; }
        public string Rua { get; set; }
    }
}
