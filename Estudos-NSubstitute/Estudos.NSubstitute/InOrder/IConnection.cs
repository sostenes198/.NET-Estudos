using Microsoft.VisualBasic;

namespace Estudos.NSubstitute.InOrder
{
    public interface IConnection
    {
        void Open();
        void Close();
    }
}