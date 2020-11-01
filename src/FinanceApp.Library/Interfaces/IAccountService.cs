using FinanceApp.Library.Domain;

namespace FinanceApp.Library.Interfaces
{
    public interface IAccountService
    {
        void Deposit(Amount amount);
        void Withdraw(Amount amount);
        void PrintStatement();

    }
}