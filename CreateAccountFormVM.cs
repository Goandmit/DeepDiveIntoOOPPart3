using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace DeepDiveIntoOOPPart3
{
    internal class CreateAccountFormVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public ObservableCollection<AccountType> AccountTypes { get; set; }
        public AccountType SelectedAccountType { get; set; }        
        public int SelectedIndex { get; set; }

        public string AccountTypeText { get; set; }
        public string BalanceText { get; set; }
        public string Title { get; set; }

        public string OrgForm { get; set; }
        public string ClientId { get; set; }
        public bool VIP { get; set; }       

        private string balance;
        public string Balance
        {
            get { return balance; }
            set
            {
                balance = Repository.GetOnlyConvertibleIntoDecimal(value);
                OnPropertyChanged("Balance");                
            }
        }      
        
        public class AccountType
        {
            public string Type { get; set; }            
        }

        public CreateAccountFormVM(string orgForm, string clientId, bool vIP)
        {
            OrgForm = orgForm;
            ClientId = clientId;
            VIP = vIP;

            Title = "Создание счета";
            AccountTypeText = "Выберите тип счета";
            BalanceText = "Введите начальный баланс";            

            AccountTypes = new ObservableCollection<AccountType>();

            AccountType type = new AccountType() { Type = "Депозитный" };

            AccountTypes.Add(type);

            type = new AccountType() { Type = "Недепозитный" };           

            AccountTypes.Add(type);            
        }

        public CreateAccountFormVM(string clientId, string accountType)
        {
            ClientId = clientId;

            Title = "Пополнение счета";
            AccountTypeText = "Пополняемый счет";
            BalanceText = "Введите сумму";                       

            AccountTypes = new ObservableCollection<AccountType>();

            AccountType type = new AccountType() { Type = accountType };

            AccountTypes.Add(type);

            SelectedIndex = 0;
        }

        private void AccountReplenishment()
        {
            if (SelectedAccountType.Type == "Депозитный")
            {
                DepositAccount account = Repository.DepositAccountManager.DepositAccount
                (ClientId, Convert.ToDecimal(Balance));

                Repository.DepositAccountManager.OwerwriteAccount(account);
            }
            else
            {
                NonDepositAccount account = Repository.NonDepositAccountManager.DepositAccount
                (ClientId, Convert.ToDecimal(Balance));

                Repository.NonDepositAccountManager.OwerwriteAccount(account);
            }            
        }

        private void AccountOpening()
        {
            if (SelectedAccountType.Type == "Депозитный")
            {
                DepositAccount account = new DepositAccount(SelectedAccountType.Type,
                    ClientId, OrgForm, Convert.ToDecimal(Balance), VIP);

                Repository.DepositAccountManager.OpenAccount(account);
            }
            else
            {
                NonDepositAccount account = new NonDepositAccount(SelectedAccountType.Type,
                    ClientId, OrgForm, Convert.ToDecimal(Balance));

                Repository.NonDepositAccountManager.OpenAccount(account);
            }
        }

        private RelayCommand oKCommand;
        public RelayCommand OKCommand
        {
            get
            {
                return oKCommand ??
                  (oKCommand = new RelayCommand(obj =>
                  {
                      if (SelectedAccountType is AccountType)                          
                      {
                          if (Balance == null)
                          {
                              Balance = "0,00";
                          }

                          if (decimal.TryParse(Balance, out var result))
                          {
                              if (!Balance.Contains(','))
                              {
                                  Balance += ",00";
                              }
                              
                              if (Title == "Пополнение счета")
                              {
                                  AccountReplenishment();
                              }
                              else
                              {
                                  AccountOpening();
                              }                                                           

                              foreach (Window window in App.Current.Windows)
                              {
                                  if (window is CreateAccountForm)
                                  {
                                      window.Close();
                                  }
                              }

                              Repository.CloseAccountWindow();

                              Repository.CurrentClientFormsVM.RefreshBankAccounts();
                          }                          
                      }
                  }));
            }
        }        
    }
}
