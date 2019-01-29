using System.Collections.Generic;

namespace WhereDynamic.Entidade
{
    public class Empresa
    {
        public int Id { get; set; }

        public string NomeEmpresa { get; set; }

        public IEnumerable<Pessoa> Colaboradores { get; set; }
    }
}