using Microsoft.Extensions.DependencyInjection;
using ZTestes.Testes.Cliente;

namespace ZTestes.Testes  
{
    class Program  
    {  
        static void Main(string[] args)
        {
            var container = BootStraper.CreateServiceProvider();
            var service = container.GetRequiredService<IClienteService>();
            service.Salvar(new Cliente.Cliente{Id = 1, Nome = "Teste"});
            service.Listar();
            service.Obter(1);
        }  
    }
}  