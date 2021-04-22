namespace Estudos.NSubstitute.SomeClass
{
    public class SomeClassWithCtorArgs
    {
        private readonly int _cout;
        private readonly string _name;

        public SomeClassWithCtorArgs(int cout, string name)
        {
            _cout = cout;
            _name = name;
        }
    }
}