using Xunit;

namespace Estudos.Tests.Mutation.UnitTest
{
    public class NegateTest
    {        
        [Fact]
        [Trait("Example-Negate", "Deve retornar true, prova de igualdade.")]
        public void Sample_Negate_Test()
        {
            Assert.True(new Math().AreEqual(1, 1));
        }
    }
}
