using System;
using System.Threading.Tasks;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.FireForget
{
    public interface IFireForgetHandler
    {
        void Execute<T>(Action<T> execution, Action<Exception> handlerException = null);
        void ExecuteAsync<T>(Func<T, Task> execution, Action<Exception> handlerException = null);
    }
}