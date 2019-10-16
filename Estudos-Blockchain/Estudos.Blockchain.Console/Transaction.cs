namespace Estudos.Blockchain.Console
{
    public class Transaction
    {
        public Transaction(string fromAddress, string toAddress, int amount)
        {
            FromAddress = fromAddress;
            ToAddress = toAddress;
            Amount = amount;
        }

        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public int Amount { get; set; }
    }
}