using System.Collections.Generic;

namespace Estudos.Dominio.Entidades.Entidades_Cardapio
{
    public class CardapioCategoria : AEntidade
    {
        public string Descricao { get; set; }

        public IEnumerable<Cardapio> Cardapios { get; set; }
    }
}