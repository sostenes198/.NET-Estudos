namespace Estudos.Abstract.Dominio.Entidades
{
    public abstract class AEntidade : IEntidade
    {
        public const int TamanhoStringPadrao = 100;

        public AEntidade()
        { }

        public int Codigo { get; set; }
    }
}
