using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstudosMediator.Entity.Customer;

namespace EstudosMediator.Infra
{
    public class CustomerRepository : ICustomerRepository
    {
        public List<CustomerEntity> Customers { get; }

        public CustomerRepository()
        {
            Customers = new List<CustomerEntity>
            {
                new CustomerEntity(1, "1", "1", "1@1.com", "1111"),
                new CustomerEntity(2, "2", "2", "2@1.com", "2222"),
                new CustomerEntity(3, "3", "3", "3@1.com", "3333"),
                new CustomerEntity(4, "4", "3", "4@1.com", "4444"),
                new CustomerEntity(5, "5", "5", "5@1.com", "5555")
            };
        }

        public Task Save(CustomerEntity customer)
        {
            return Task.Run(() => Customers.Add(customer));
        }

        public async Task Update(int id, CustomerEntity customer)
        {
            int index = Customers.FindIndex(m => m.Id == id);
            if (index >= 0)
                await Task.Run(() => Customers[index] = customer);
        }

        public Task Delete(int id)
        {
            int index = Customers.FindIndex(m => m.Id == id);
            return Task.Run(() => Customers.RemoveAt(index));
        }

        public Task<CustomerEntity> GetById(int id)
        {
            var result = Customers.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(result);
        }

        public async Task<IEnumerable<CustomerEntity>> GetAll()
        {
            return await Task.FromResult(Customers);
        }
    }
}