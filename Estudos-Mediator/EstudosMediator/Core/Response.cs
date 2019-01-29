using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EstudosMediator.Core
{
    public class Response
    {
        private readonly IList<string> _messages = new List<string>();

        public IEnumerable<string> Errors { get; }
        public object Result { get; }

        public Response() => Errors = new ReadOnlyCollection<string>(_messages);

        public Response(object result) : this() => Result = result;

        public Response AddError(string message)
        {
            _messages.Add(message);
            return this;
        }

        public static Response CreateSucessResponse(object result)
        {
            return new Response(result);
        }
    }
}