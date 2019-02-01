namespace Estudos.Global.NameSpace.Definicao
{
    public class NameSpaceDefinition
    {
        public NameSpaceDefinition(string abstracao, string implementacao)
        {
            Abstracao = abstracao;
            Implementacao = implementacao;
        }

        public string Abstracao { get; set; }

        public string Implementacao { get; set; }
    }
}
