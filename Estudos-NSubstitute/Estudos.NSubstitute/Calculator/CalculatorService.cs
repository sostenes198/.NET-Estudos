namespace Estudos.NSubstitute.Calculator
{
    public class CalculatorService
    {
        private readonly ICalculator _calculator;

        public CalculatorService(ICalculator calculator)
        {
            _calculator = calculator;
        }
        
        public int Add(int a, int b)
        {
            return _calculator.Add(a, b);
        }

        public string GetMode() => _calculator.Mode;
        
        public void SetMode(string mode) => _calculator.Mode = mode;

        public void SayHello(string message) => _calculator.SayHello(message);
    }
}