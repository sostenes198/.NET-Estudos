using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Estudos.Tests.Mutation.UnitTest
{
    public class CashierTest
    {
        #region Attributes

        public static IEnumerable<object[]> FullAvailableChangeMoney
        {
            get
            {
                yield return new object[] { 40.0D, 40.0M };
                yield return new object[] { 40.0D, 50.0M };
                yield return new object[] { 43.0D, 50.0M };
                yield return new object[] { 42.25D, 50.0M };
                yield return new object[] { 2.21D, 50.0M };
                yield return new object[] { 42.24D, 50.0M };
            }
        }

        public static IEnumerable<object[]> UnavailableChangeMoney
        {
            get
            {
                yield return new object[] { 40.0D, 40.0M, new List<Money> { { new Money(10.0M, MoneyType.BankNote) } } };
                yield return new object[] { 43.0D, 50.0M, new List<Money> { { new Money(10.0M, MoneyType.BankNote) } } };
                yield return new object[] { 42.25D, 50.0M, new List<Money> { { new Money(5.0M, MoneyType.BankNote) } } };
                yield return new object[] { 2.21D, 50.0M, new List<Money> { { new Money(10.0M, MoneyType.BankNote) } } };
                yield return new object[] { 42.24D, 50.0M, new List<Money> { { new Money(2.0M, MoneyType.BankNote) }, { new Money(0.25M, MoneyType.Coin) } } };
            }
        }

        #endregion

        [Theory]
        [MemberData(nameof(FullAvailableChangeMoney))]
        [Trait("ReturnsOK-Troco", "Retorna troco correto com todas as Moedas e Notas dispon�veis")]
        public void Test_GetChangeMoney_Full_ChangeMoney_AvailableShouldBeOK(decimal purchaseValue, decimal customerMoneyValue)
        {
            Cashier cashier = new Cashier();

            IList<Money> changeMoney = cashier.GetChangeMoney(purchaseValue, customerMoneyValue);

            decimal total = purchaseValue + changeMoney.Sum(x => x.Value);

            Assert.Equal(total, customerMoneyValue);
        }

        [Theory]
        [MemberData(nameof(UnavailableChangeMoney))]
        [Trait("ReturnsOK-Troco", "Retorna troco correto com Moedas e/ou Notas indispon�veis.")]
        public void Test_GetChangeMoney_Partial_ChangeMoney_AvailableShouldBeOK(decimal purchaseValue, decimal customerMoneyValue, List<Money> unAvailableChangeMoney)
        {
            IList<Money> changeMoney = new Cashier(unAvailableChangeMoney).GetChangeMoney(purchaseValue, customerMoneyValue);

            decimal total = purchaseValue + changeMoney.Sum(x => x.Value);

            bool resultNotContainMoney = unAvailableChangeMoney.TrueForAll(x => !changeMoney.Contains(x));

            Assert.True(resultNotContainMoney);

            Assert.Equal(total, customerMoneyValue);
        }

        [Fact]
        [Trait("Wait-For-Exception", "Troco retornou muitos items do mesmo tipo")]
        public void Test_GetChangeMoney_Exceed_Max_Count_WaitFor_Exception()
        {
            List<Money> unavailableChangeMoney = new List<Money> {
                { new Money(10.0M, MoneyType.BankNote) },
                { new Money(5.0M, MoneyType.BankNote) },
                { new Money(2.0M, MoneyType.BankNote) },
                { new Money(1.0M, MoneyType.Coin) },
                { new Money(0.50M, MoneyType.Coin) },
                { new Money(0.25M, MoneyType.Coin) },
                { new Money(0.10M, MoneyType.Coin) }
            };
            Cashier cashier = new Cashier(unavailableChangeMoney);
            Assert.Throws<InvalidChangeMoneyException>(() =>  cashier.GetChangeMoney(40.0M, 50.0M));
        }

        [Fact]
        [Trait("Wait-For-Exception", "Valor da compra menor ou igual a 0")]
        public void Test_GetChangeMoney_PurchaseValue_MinorOrEqual_Zero_WaitFor_Exception()
        {
            Cashier cashier = new Cashier();
            Assert.Throws<InvalidPurchaseValueException>(() => cashier.GetChangeMoney(0.0M, 50.0M));
        }

        [Fact]
        [Trait("Wait-For-Exception", "Valor dado pelo cliente igual a 0")]
        public void Test_GetChangeMoney_CustomerValue_MinorOrEqual_Zero_WaitFor_Exception()
        {
            Cashier cashier = new Cashier();
            Assert.Throws<InvalidCustomerValueException>(() => cashier.GetChangeMoney(50.0M, 0.0M));
        }
    }
}
