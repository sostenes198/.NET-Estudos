namespace Estudos.Global.NameSpace.Definicao
{
    public class NameSpaceDefinition
    {
        public NameSpaceDefinition(object abstracao, object implementacao)
        {
            Abstracao = abstracao;
            Implementacao = implementacao;
        }

        public object Abstracao { get; set; }

        public object Implementacao { get; set; }
    }
}