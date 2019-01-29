namespace Estudos.Tests.Mutation
{
    public class Customer
    {
        public decimal Value { get; private set; }

        public Customer(decimal value)
        {
            Value = value;
            CheckValue();
        }

        private void CheckValue()
        {
            if (Value <= 0)
            {
                throw new InvalidCustomerValueException();
            }
        }
    }
}
