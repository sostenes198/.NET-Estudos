namespace Estudos.Abstract.Dominio.Entidades
{
    public abstract class AEntidade : IEntidade
    {
        public AEntidade()
        { }

        public int Codigo { get; set; }
    }
}
