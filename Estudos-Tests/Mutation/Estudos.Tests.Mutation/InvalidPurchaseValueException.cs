using System;
using System.Runtime.Serialization;

namespace Estudos.Tests.Mutation
{
    [Serializable]
    public class InvalidPurchaseValueException : Exception
    {
        public InvalidPurchaseValueException()
        {
        }

        public InvalidPurchaseValueException(string message) : base(message)
        {
        }

        public InvalidPurchaseValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidPurchaseValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}