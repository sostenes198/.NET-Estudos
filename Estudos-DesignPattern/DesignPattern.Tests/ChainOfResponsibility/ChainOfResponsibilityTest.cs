using System;
using System.Linq;
using DesignPattern.ChainOfResponsability.Contracts.Base;
using DesignPatternTests.ChainOfResponsibility.Fixture;
using DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainA;
using DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainB;
using DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainC;
using DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainD;
using DesignPatternTests.ChainOfResponsibility.Fixture.Steps;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;
using DesignPattern.ChainOfResponsability.Extensions;

namespace DesignPatternTests.ChainOfResponsibility
{
    public class ChainOfResponsibilityTest : IClassFixture<ChainOfResponsibilityFixture>
    {
        private readonly ChainOfResponsibilityFixture _fixture;

        public ChainOfResponsibilityTest(ChainOfResponsibilityFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Should_Validate_Chain()
        {
            var build = _fixture.BuildChainA;
            build += _fixture.BuildChainB;
            build += _fixture.BuildChainC;
            build += collection =>
            {
                collection.TryAddScoped<ARecordStep>();
                collection.TryAddScoped<BRecordStep>();
                return collection;
            };
            var serviceProvider = _fixture.BuilServiceProvider(build);
            
            // Validate Chain A
            var chainA = serviceProvider.GetRequiredService<AChainOfResponsibility>();
            var aRecordStep =  serviceProvider.GetRequiredService<ARecordStep>();
            chainA.Execute();
            aRecordStep.Records.Should().BeEquivalentTo("A1", "A2", "A3", "A4", "A5"); 
            
            // Validate Chain B
            var chainB = serviceProvider.GetRequiredService<BChainOfResponsibility>();
            var bRecordStep =  serviceProvider.GetRequiredService<BRecordStep>();
            chainB.Execute();
            bRecordStep.Records.Should().BeEquivalentTo("B1", "B2", "B3", "B4", "B5");
            
            // Validate Chain C
            aRecordStep.Records.Clear();
            bRecordStep.Records.Clear();
            var chainC= serviceProvider.GetRequiredService<CChainOfResponsibility>();
            chainC.Execute();
            
            var cRecordStep = aRecordStep.Records.Union(bRecordStep.Records);
            cRecordStep.Should().BeEquivalentTo("A1", "B2", "A3", "B4", "A4", "B5");
        }

        [Fact]
        public void Should_Throw_Exception_When_Type_Of_Chain_Is_IChainOfResponsibility()
        {
            _fixture.Invoking(lnq => lnq.BuilServiceProvider(collection =>
                {
                    collection.AddChain<IChainOfResponsibility>();
                    return collection;
                }))
                .Should()
                .Throw<InvalidOperationException>()
                .And
                .Message
                .Should().Be("Parameter needs to inherit from IChainOfResponsibility.");
        }

        [Fact]
        public void Should_Throw_Exception_When_Type_Of_Chain_Is_Not_A_Class()
        {
            _fixture.Invoking(lnq => lnq.BuilServiceProvider(collection =>
                {
                    collection.AddChain<IAChainOfResponsibility>();
                    return collection;
                }))
                .Should()
                .Throw<InvalidOperationException>()
                .And
                .Message
                .Should().Be("Parameter IAChainOfResponsibility needs to be a class.");
        }

        [Fact]
        public void Should_Throw_Exception_When_Steps_Duplicates_In_Chain()
        {
            _fixture.Invoking(lnq => lnq.BuilServiceProvider(collection =>
                {
                    collection.AddChain<DChainOfResponsibility>()
                        .AddStep<A1>()
                        .AddStep<A1>()
                        .AddLastStep<A5>()
                        .Configure();
                    return collection;
                }))
                .Should()
                .Throw<InvalidOperationException>()
                .And
                .Message
                .Should().Be("There can be no duplicate steps in the chain.");
        }
    }
}