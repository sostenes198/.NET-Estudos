namespace Estudos.Dominio.Entidades
{
    public abstract class AEntidade
    {
        public const int tamanhoStringPadrao = 100;

        public AEntidade()
        { }

        public int Codigo { get; set; }
    }
}
