using Estudos.Checklist.ConfiguracoesPc.Domain;

namespace Estudos.Checklist.ConfiguracoesPc.Tests.Fixtures
{
    public class BaseFixture : Base
    {
        public BaseFixture()
        {
            AddServices = (services) => services.RegisterApplication(Configuration);
        }
    }
}