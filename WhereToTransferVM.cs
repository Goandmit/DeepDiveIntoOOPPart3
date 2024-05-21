using System.Collections.ObjectModel;
using System.Windows;

namespace DeepDiveIntoOOPPart3
{
    internal class WhereToTransferVM
    {
        public ObservableCollection<Account> Accounts { get; set; }
        public Account SelectedAccount { get; set; }

        public WhereToTransferVM(string recipientId)
        {
            Accounts = new ObservableCollection<Account>();

            DepositAccount savingsAccount = Repository.DepositAccountManager.GetAccountFromFile(recipientId);

            if (savingsAccount.Id != null)
            {
                Accounts.Add(savingsAccount);
            }

            NonDepositAccount paymentAccount = Repository.NonDepositAccountManager.GetAccountFromFile(recipientId);

            if (paymentAccount.Id != null)
            {
                Accounts.Add(paymentAccount);
            }
        }

        private RelayCommand selectionCommand;
        public RelayCommand SelectionCommand
        {
            get
            {
                return selectionCommand ??
                  (selectionCommand = new RelayCommand(obj =>
                  {
                      if (!string.IsNullOrEmpty($"{SelectedAccount}"))
                      {
                          Repository.RecipientAcountType = SelectedAccount.AccountType;
                          Repository.RecipientAccountNumber = SelectedAccount.AccountNumber;                          

                          foreach (Window window in App.Current.Windows)
                          {
                              if (window is WhereToTransfer)
                              {
                                  window.Close();
                              }
                          }
                      }

                  }));
            }
        }
    }
}
