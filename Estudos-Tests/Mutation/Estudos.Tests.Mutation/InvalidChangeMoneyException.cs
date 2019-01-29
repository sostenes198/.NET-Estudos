using System;
using System.Runtime.Serialization;

namespace Estudos.Tests.Mutation
{
    [Serializable]
    public class InvalidChangeMoneyException : Exception
    {
        public InvalidChangeMoneyException()
        {
        }

        public InvalidChangeMoneyException(string message) : base(message)
        {
        }

        public InvalidChangeMoneyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidChangeMoneyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}