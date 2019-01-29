using Estudos.Abstract.Dominio.Entidades;

namespace Estudos.Dominio.Entidades
{
    public abstract class AEntidade : IEntidade
    {
        public AEntidade()
        { }

        public int Codigo { get; set; }
    }
}
