using System.Collections.Generic;
using ZTestes.Testes.Interceptors;
using ZTestes.Testes.Interceptors.Base;

namespace ZTestes.Testes.Cliente
{
    public interface IClienteService
    {
        [Interceptor(typeof(LogAInterceptor))]
        Cliente Obter(int id);
        IEnumerable<Cliente> Listar();
        void Salvar(Cliente cliente);
    }
}