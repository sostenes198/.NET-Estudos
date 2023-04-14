using Microsoft.Extensions.DependencyInjection;

namespace Estudos.ExpressionThree;

public class Test
{
    public static IServiceProvider AddFilters()
    {
        var serviceCollection = new ServiceCollection();

        //serviceCollection.AddConsumerFilters()

        return serviceCollection.BuildServiceProvider();
    }
}