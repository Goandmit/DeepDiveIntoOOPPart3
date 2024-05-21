using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace DeepDiveIntoOOPPart3
{
    internal class TransferFormVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }        

        private string whom;
        public string Whom
        {
            get { return whom; }
            set
            {
                whom = value;
                OnPropertyChanged("Whom");                
            }
        }
        
        private string where;
        public string Where
        {
            get { return where; }
            set
            {
                where = value;
                OnPropertyChanged("Where");
            }
        }

        private string amount;
        public string Amount
        {
            get { return amount; }
            set
            {
                amount = Repository.GetOnlyConvertibleIntoDecimal(value);
                OnPropertyChanged("Amount");                
            }
        }

        public bool EditingIsAllowed { get; set; }        

        private Account senderAccount;
        private Account recipientAccount;

        public TransferFormVM(Account senderAccount, bool transferToYourself)
        {
            this.senderAccount = senderAccount;

            if (transferToYourself == true)
            {
                Whom = "Себе";
                Where = "На второй счет";
                EditingIsAllowed = false;                
            }
            else
            {
                EditingIsAllowed = true;                              
            }
        }       

        private void WhomToTransfer_Closed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty($"{Repository.RecipientName}"))
            {
                Whom = Repository.RecipientName;
                Where = String.Empty;
            }
        }

        private void WhereToTransfer_Closed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty($"{Repository.RecipientAccountNumber}"))
            {
                Where = Repository.RecipientAccountNumber;
            }
        }

        private RelayCommand whomCommand;
        public RelayCommand WhomCommand
        {
            get
            {
                return whomCommand ??
                  (whomCommand = new RelayCommand(obj =>
                  {
                      WhomToTransfer whomToTransfer = new WhomToTransfer(senderAccount.OwnerId);
                      whomToTransfer.Show();

                      whomToTransfer.Closed += WhomToTransfer_Closed;

                  }));
            }
        }

        private RelayCommand whereCommand;
        public RelayCommand WhereCommand
        {
            get
            {
                return whereCommand ??
                  (whereCommand = new RelayCommand(obj =>
                  {
                      if (!string.IsNullOrEmpty($"{Repository.RecipientId}"))
                      {
                          WhereToTransfer whereToTransfer= new WhereToTransfer(Repository.RecipientId);
                          whereToTransfer.Show();

                          whereToTransfer.Closed += WhereToTransfer_Closed;
                      }

                  }));
            }
        }

        private RelayCommand transferCommand;
        public RelayCommand TransferCommand
        {
            get
            {
                return transferCommand ??
                  (transferCommand = new RelayCommand(obj =>
                  {
                      if (!string.IsNullOrEmpty($"{Whom}") &&
                          !string.IsNullOrEmpty($"{Where}") &&
                          !string.IsNullOrEmpty($"{Amount}"))
                      {
                          if (decimal.TryParse(Amount, out var result))
                          {
                              decimal amount = Convert.ToDecimal(Amount);

                              if (Whom == "Себе")
                              {
                                  if (senderAccount is DepositAccount savingsAccount)
                                  {
                                      Repository.DepositAccountManager.TransferToYourself(savingsAccount, amount);
                                  }

                                  if (senderAccount is NonDepositAccount paymentAccount)
                                  {
                                      Repository.NonDepositAccountManager.TransferToYourself(paymentAccount, amount);
                                  }
                              }
                              else
                              {
                                  if (Repository.RecipientAcountType == "Депозитный")
                                  {
                                      recipientAccount = Repository.DepositAccountManager.GetAccountFromFile
                                          (Repository.RecipientId);                                      
                                  }
                                  else
                                  {
                                      recipientAccount = Repository.NonDepositAccountManager.GetAccountFromFile
                                          (Repository.RecipientId);
                                  }

                                  TransferManager<Account> transferManager = new TransferManager<Account>();

                                  transferManager.Transfer(senderAccount, recipientAccount, amount);
                              }

                              foreach (Window window in App.Current.Windows)
                              {
                                  if (window is TransferForm)
                                  {
                                      window.Close();
                                  }
                              }
                          }                          
                      }
                  }));
            }
        }
    }
}
