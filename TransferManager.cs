using System.Windows;

namespace DeepDiveIntoOOPPart3
{
    internal class TransferManager<T> : ITransfer<T>
        where T : Account
    {
        public void Transfer(T senderAccount, T recipientAccount, decimal amount)
        {
            if (amount <= senderAccount.Balance)
            {
                senderAccount.Withdraw(amount);
                recipientAccount.Deposit(amount);

                Repository.OwerwriteAccount(senderAccount);
                Repository.OwerwriteAccount(recipientAccount);

                foreach (Window window in App.Current.Windows)
                {
                    if (window is TransferForm)
                    {
                        window.Close();
                    }
                }
            }
            else
            {
                Repository.Message("Недостаточно средств");
            }
        }
    }
}
