using System;

namespace DeepDiveIntoOOPPart3
{
    internal class AccountFormsVM
    {
        private Account currentAccount;
        public string AccountType { get; set; }        
        public string AccountNumber { get; set; }
        public string OpeningDate { get; set; }
        public string Balance { get; set; }
        public string InterestRate { get; set; }
        public string DateOfNextAccrual { get; set; }
        public string AccrualInCurrentMonth { get; set; }            

        public AccountFormsVM(Account account)
        {
            AccountType = account.AccountType;
            AccountNumber = account.AccountNumber;
            OpeningDate = (account.OpeningDate).ToShortDateString();
            Balance = account.Balance.ToString();

            if (account is DepositAccount savingsAccount)
            {               
                InterestRate = savingsAccount.InterestRate.ToString();                
                DateOfNextAccrual = (savingsAccount.DateOfNextAccrual).ToShortDateString();
                AccrualInCurrentMonth = savingsAccount.AccrualInCurrentMonth.ToString();
            }

            currentAccount = account;        
        }

        private void TransferForm_Closed(object sender, EventArgs e)
        {
            Repository.CurrentClientFormsVM.RefreshBankAccounts();

            Repository.ResetTransferInformation();
        }

        private void CallTransferForm(Account senderAccount, bool transferToYourself)
        {
            TransferForm transferForm = new TransferForm(senderAccount, transferToYourself);
            transferForm.Show();

            transferForm.Closed += TransferForm_Closed;

            Repository.CloseAccountWindow();
        }

        private RelayCommand transferCommand;
        public RelayCommand TransferCommand
        {
            get
            {
                return transferCommand ??
                  (transferCommand = new RelayCommand(obj =>
                  {
                      CallTransferForm(currentAccount, false);
                  }));
            }
        }

        private RelayCommand transferToYourselfCommand;
        public RelayCommand TransferToYourselfCommand
        {
            get
            {
                return transferToYourselfCommand ??
                  (transferToYourselfCommand = new RelayCommand(obj =>
                  {
                      CallTransferForm(currentAccount, true);

                  }));
            }
        }

        private RelayCommand replenishCommand;
        public RelayCommand ReplenishCommand
        {
            get
            {
                return replenishCommand ??
                  (replenishCommand = new RelayCommand(obj =>
                  {
                      CreateAccountForm createAccountForm
                      = new CreateAccountForm(currentAccount.OwnerId, currentAccount.AccountType);

                      createAccountForm.Show();

                  }));
            }
        }
    }
}
