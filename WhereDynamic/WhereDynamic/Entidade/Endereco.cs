namespace WhereDynamic.Entidade
{
    public class Endereco
    {
        public Endereco()
        {
            Cidade = new Cidade();
        }

        public int Id { get; set; }
        public string Pais { get; set; }
        public Cidade Cidade { get; set; }
    }
}