namespace Thread
{
    public delegate void ResultCallbackDelegate(int Results);

    //Creating the Helper class
    public class NumberHelper
    {
        //Creating two private variables to hold the Number and ResultCallback instance
        private readonly int _number;

        private readonly ResultCallbackDelegate _resultCallbackDelegate;

        //Initializing the private variables through constructor
        //So while creating the instance you need to pass the value for Number and callback delegate
        public NumberHelper(int number, ResultCallbackDelegate resultCallbackDelagate)
        {
            _number = number;
            _resultCallbackDelegate = resultCallbackDelagate;
        }

        //This is the Thread function which will calculate the sum of the numbers
        public void CalculateSum()
        {
            var result = 0;
            for (var i = 1; i <= _number; i++) result = result + i;
            //Before the end of the thread function call the callback method
            if (_resultCallbackDelegate != null) _resultCallbackDelegate(result);
        }
    }
}