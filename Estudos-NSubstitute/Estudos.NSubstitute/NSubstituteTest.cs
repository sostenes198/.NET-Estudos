using System;
using System.Collections.Generic;
using Estudos.NSubstitute.Calculator;
using Estudos.NSubstitute.EventInvocation;
using Estudos.NSubstitute.InOrder;
using Estudos.NSubstitute.Lookup;
using Estudos.NSubstitute.SomeClass;
using FluentAssertions;
using NSubstitute;
using Xunit;
using ICommand = System.Windows.Input.ICommand;

namespace Estudos.NSubstitute
{
    public class NSubstituteTest
    {
        private readonly ICalculator _calculator;
        private readonly CalculatorService _calculatorService;

        public NSubstituteTest()
        {
            _calculator = Substitute.For<ICalculator>();
            _calculatorService = new CalculatorService(_calculator);
        }

        [Fact]
        public void Test()
        {
            _calculator.Add(1, 3).Returns(4);

            _calculatorService.Add(1, 3).Should().Be(4);
            _calculator.Received(1).Add(1, 3);
        }

        [Fact]
        public void Test_Property_GetMode()
        {
            _calculator.Mode.Returns("TEST");

            var result = _calculatorService.GetMode();
            result.Should().Be("TEST");
        }

        [Fact]
        public void Test_Property_SetMode()
        {
            _calculatorService.SetMode("TESTANDO");


            _calculator.Mode.Should().Be("TESTANDO");
        }

        [Fact]
        public void Test_Matching_Arguments()
        {
            _calculator.Add(Arg.Any<int>(), Arg.Any<int>())
                .Returns(x => (int) x[0] + (int) x[1]);

            _calculatorService.Add(5, -2).Should().Be(3);
            _calculator.Received().Add(Arg.Is<int>(t => t == 5), Arg.Is<int>(t => t == -2));
        }

        [Fact]
        public void Test_Multiple_Arguments_To_Set_Up_A_Sequence_Of_Return_Values()
        {
            _calculator.Mode.Returns("TEST", "TEST1", "TEST2");

            _calculator.Mode.Should().Be("TEST");
            _calculator.Mode.Should().Be("TEST1");
            _calculator.Mode.Should().Be("TEST2");
        }

        [Fact]
        public void Test_Raise_Event()
        {
            bool eventRaised = false;
            _calculator.PoweringUp += (sender, args) => eventRaised = true;
            _calculator.PoweringUp += Raise.Event();
            eventRaised.Should().BeTrue();
        }

        [Fact]
        public void Test_Some_Class_With_Ctor_Args()
        {
            var someClass = Substitute.For<SomeClassWithCtorArgs>(5, "Olá mundo");
        }

        [Fact]
        public void Test_Substituting_For_Multiples_Interfaces()
        {
            var substitute = Substitute.For(new[]
                {
                    typeof(ICommand), typeof(SomeClassWithCtorArgs)
                },
                new object[] {5, "Olá mundo"});
        }

        [Fact]
        public void Test_Substituting_For_Delegates()
        {
            var func = Substitute.For<Func<string>>();

            func().Returns("hello");
            func().Should().Be("hello");
            func.Received(1).Invoke();
        }

        [Fact]
        public void Test_For_Any_Args()
        {
            _calculator.Add(1, 3).ReturnsForAnyArgs(100);
            _calculatorService.Add(default, default).Should().Be(100);
        }

        [Fact]
        public void Test_Checking_Received_Calls()
        {
            _calculator.Add(1, 3).Returns(4);
            _calculatorService.Add(1, 3);
            _calculator.Received(1).Add(1, 3);
        }


        [Fact]
        public void Test_Checking_Was_Not_Received_Calls()
        {
            _calculator.Add(1, 3).Returns(4);
            _calculatorService.Add(5, 5);
            _calculator.DidNotReceive().Add(1, 3);
        }

        [Fact]
        public void Test_Checking_Calls_To_Properties()
        {
            _calculator.Mode = "TEST";
            _calculator.Received().Mode = "TEST";
        }

        [Fact]
        public void Test_Checking_Calls_To_Indexers()
        {
            var dictionary = Substitute.For<IDictionary<string, int>>();
            dictionary["test"] = 1;

            dictionary.Received()["test"] = 1;
            dictionary.Received()["test"] = Arg.Is<int>(x => x < 5);
        }

        [Fact]
        public void Test_Event_Invocation_ShouldRaiseLowFuel_WithoutNSub()
        {
            var fuelManagement = new FuelManagement();
            var eventWasRaised = false;
            fuelManagement.LowFuelDetected += (o, e) => eventWasRaised = true;

            fuelManagement.DoSomething();
            eventWasRaised.Should().BeTrue();
        }

        [Fact]
        public void Test_Event_Invocation_ShouldRaiseLowFuel()
        {
            var fuelManagement = new FuelManagement();
            var handler = Substitute.For<EventHandler<LowFuelWarningEventArgs>>();
            fuelManagement.LowFuelDetected += handler;

            fuelManagement.DoSomething();

            handler.Received().Invoke(fuelManagement, Arg.Is<LowFuelWarningEventArgs>(x => x.PercentLeft < 20));
        }

        [Fact]
        public void Test_Callbacks_For_Void_Calls()
        {
            var counter = 0;
            _calculator.When(x => x.SayHello(Arg.Any<string>()))
                .Do(x => counter++);

            counter.Should().Be(1);
        }

        [Fact]
        public void Test_Throwing_Exceptions()
        {
            // for non voids
            _calculator.Add(Arg.Any<int>(), Arg.Any<int>()).Returns(x => throw new Exception("Non Voids"));

            // for voids
            _calculator
                .When(x => x.SayHello(Arg.Any<string>()))
                .Do(x => throw new Exception("Voids"));

            _calculatorService.Invoking(lnq => lnq.Add(-1, 1)).Should().Throw<Exception>().WithMessage("Non Voids");
            _calculator.Invoking(lnq => lnq.SayHello("Test")).Should().Throw<Exception>().WithMessage("Voids");
        }

        [Fact]
        public void Test_Setting_Out_And_Ref_Args()
        {
            var lookup = Substitute.For<ILookup>();
            lookup.TryLookup(Arg.Any<string>(), out Arg.Any<string>())
                .Returns(x =>
                {
                    x[1] = (string) x[0];
                    return true;
                });

            var result = lookup.TryLookup("olá mundo", out var outResult);
            result.Should().BeTrue();
            outResult.Should().Be("olá mundo");
        }

        [Fact]
        public void Test_Checking_Call_Order()
        {
            var connection = Substitute.For<IConnection>();
            var command = Substitute.For<InOrder.ICommand>();
            var subject = new Controller(command, connection);

            subject.DoStuff();

            Received.InOrder(() => {
                connection.Open();
                command.Execute();
                connection.Close();
            });
        }

    }
}