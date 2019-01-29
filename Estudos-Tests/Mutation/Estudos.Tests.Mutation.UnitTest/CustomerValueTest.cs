using Xunit;

namespace Estudos.Tests.Mutation.UnitTest
{
    public class CustomerValueTest
    {
        [Fact]
        [Trait("Wait-For-Exception", "Valor dado pelo cliente igual a 0")]
        public void Test_Ctor_CustomerValue_VlrMinorOrEqual_Zero_WaitFor_Exception()
        {            
            Assert.Throws<InvalidCustomerValueException>(() => new Customer(0.0M));
        }
    }
}
