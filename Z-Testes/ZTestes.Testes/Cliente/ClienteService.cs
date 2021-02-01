using System.Collections.Generic;
using System.Linq;
using ZTestes.Testes.Interceptors;
using ZTestes.Testes.Interceptors.Base;

namespace ZTestes.Testes.Cliente
{
    public class ClienteService : IClienteService
    {
        private readonly IList<Cliente> _clientes;

        public ClienteService()
        {
            _clientes = new List<Cliente>();
        }
        
        public Cliente Obter(int id) => _clientes.FirstOrDefault(lnq => lnq.Id == id);

        [Interceptor(typeof(LogBInterceptor))]
        public IEnumerable<Cliente> Listar() => _clientes;
        
        public void Salvar(Cliente cliente) => _clientes.Add(cliente);
    }
}