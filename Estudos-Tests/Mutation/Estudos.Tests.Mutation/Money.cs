using System;
using System.Diagnostics.CodeAnalysis;

namespace Estudos.Tests.Mutation
{
    public class Money : Value<Money>, IEquatable<Money>
    {
        #region Properties
        public decimal Value { get; private set; }
        public MoneyType MoneyType { get; private set; }
        #endregion

        public Money(decimal value, MoneyType moneyType)
        {
            Value = value;
            MoneyType = moneyType;
        }

        #region Override Methods 
        
        public bool Equals([AllowNull] Money other)
        {
            return base.Equals(other);           
        }

        #endregion
    }
}
