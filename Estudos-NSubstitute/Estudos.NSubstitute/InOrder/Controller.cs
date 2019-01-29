namespace Estudos.NSubstitute.InOrder
{
    public class Controller
    {
        private readonly ICommand _command;
        private readonly IConnection _connection;

        public Controller(ICommand command, IConnection connection)
        {
            _command = command;
            _connection = connection;
        }

        public void DoStuff()
        {
            _connection.Open();
            _command.Execute();
            _connection.Close();
        }
    }
}