using System;
using DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainA;
using DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainB;
using DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainC;
using DesignPatternTests.ChainOfResponsibility.Fixture.Steps;
using Microsoft.Extensions.DependencyInjection;
using DesignPattern.ChainOfResponsability.Extensions;

namespace DesignPatternTests.ChainOfResponsibility.Fixture
{
    public class ChainOfResponsibilityFixture
    {
        public IServiceProvider BuilServiceProvider(Func<IServiceCollection, IServiceCollection> build)
         => build(new ServiceCollection()).BuildServiceProvider();

        public Func<IServiceCollection, IServiceCollection> BuildChainA => collection =>
        {
            collection.AddChain<AChainOfResponsibility>()
                .AddStep<A1>()
                .AddStep<A2>()
                .AddStep<A3>()
                .AddStep<A4>()
                .AddLastStep<A5>()
                .Configure();
            return collection;
        };
        
        public Func<IServiceCollection, IServiceCollection> BuildChainB => collection =>
        {
            collection.AddChain<BChainOfResponsibility>()
                .AddStep<B1>()
                .AddStep<B2>()
                .AddStep<B3>()
                .AddStep<B4>()
                .AddLastStep<B5>()
                .Configure();
            return collection;
        };
        
        public Func<IServiceCollection, IServiceCollection> BuildChainC => collection =>
        {
            collection.AddChain<CChainOfResponsibility>()
                .AddStep<A1>()
                .AddStep<B2>()
                .AddStep<A3>()
                .AddStep<B4>()
                .AddStep<A4>()
                .AddLastStep<B5>()
                .Configure();
            return collection;
        };
    }
}