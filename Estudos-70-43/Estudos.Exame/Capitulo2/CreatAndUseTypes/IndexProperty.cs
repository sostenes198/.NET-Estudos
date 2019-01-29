namespace Estudos.Exame.Capitulo2.CreatAndUseTypes
{
    public class IndexProperty
    {
        private int[] _array = new int[100];

        public int this[int i]
        {
            get => _array[i];
            set => _array[i] = value;
        }
    }
}