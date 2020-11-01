namespace FinanceApp.Library.Domain
{
    public sealed class Amount
    {
        public Amount(decimal value)
        {
            Value = value;
        }

        public decimal Value { get; }

    }
}