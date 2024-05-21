using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace DeepDiveIntoOOPPart3
{
    internal class ClientFormsVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private string id;
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private string fullName;
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
            }
        }

        public string OrgForm { get; set; }       
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string INN { get; set; }
        public string KPP { get; set; }
        public string PhoneNumber { get; set; }
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public bool VIP { get; set; }       

        public ObservableCollection<Account> Accounts { get; set; }
        public Account SelectedAccount { get; set; }       

        public ClientFormsVM(string id)
        {
            OrgForm = Repository.CurrentOrgForm;
            Accounts = new ObservableCollection<Account>();            

            if (!string.IsNullOrEmpty(id))
            {
                bool status = Repository.CheckBeforeReading(Repository.ClientsFilePath);

                if (status == true)
                {
                    Client client = Repository.GetClientFromFile(id);

                    if (client != null)
                    {
                        Id = client.Id;
                        OrgForm = client.OrgForm;
                        Name = client.Name;
                        Surname = client.Surname;
                        Patronymic = client.Patronymic;                        
                        INN = client.INN;
                        KPP = client.KPP;                        
                        PhoneNumber = client.PhoneNumber;                        
                        PassportSeries = client.PassportSeries;
                        PassportNumber = client.PassportNumber;
                        VIP = client.VIP;
                        FullName = client.FullName;

                        LoadBankAccounts();
                    }
                }
            }
        }

        public void LoadBankAccounts()
        {           
            DepositAccount savingsAccount = Repository.DepositAccountManager.GetAccountFromFile(Id);

            if (savingsAccount.Id != null)
            {
                if (savingsAccount.OverwritingIsRequired == true)
                {
                    Repository.DepositAccountManager.OwerwriteAccount(savingsAccount);
                }

                Accounts.Add(savingsAccount);
            }
            
            NonDepositAccount paymentAccount = Repository.NonDepositAccountManager.GetAccountFromFile(Id);

            if (paymentAccount.Id != null)
            {
                Accounts.Add(paymentAccount);
            }            
        }

        public void RefreshBankAccounts()
        {
            Accounts.Clear();
            LoadBankAccounts();
        }        

        private Client GetClientFromForm()
        {
            Client client = new Client(Repository.AssignOrReadClientId(Id), OrgForm,
                    Surname.Trim(), Name.Trim(), Patronymic.Trim(),
                    INN.Trim(), KPP.Trim(), PhoneNumber.Trim(),
                    PassportSeries.Trim(), PassportNumber.Trim(), VIP);

            Id = client.Id;                    

            return client;
        }

        private void EliminateNull()
        {
            if (Surname == null) { Surname = String.Empty; }
            if (Name == null) { Name = String.Empty; }
            if (Patronymic == null) { Patronymic = String.Empty; }
            if (INN == null) { INN = String.Empty; }
            if (KPP == null) { KPP = String.Empty; }
            if (PhoneNumber == null) { PhoneNumber = String.Empty; }
            if (PassportSeries == null) { PassportSeries = String.Empty; }
            if (PassportNumber == null) { PassportNumber = String.Empty; }
        }           

        private bool CheckRequiredFields()
        {
            bool status = true;

            EliminateNull();

            if (OrgForm == "Физическое лицо")
            {
                if (Surname.Trim().Length == 0 ||
                Name.Trim().Length == 0 ||                
                PhoneNumber.Trim().Length == 0 ||
                PassportSeries.Trim().Length == 0 ||
                PassportNumber.Trim().Length == 0)
                {
                    Repository.Message("Пустыми могут быть только поля \"Отчество\" и \"ИНН\"");
                    status = false;
                }
            }

            if (OrgForm == "Индивидуальный предприниматель")
            {
                if (Surname.Trim().Length == 0 ||
                Name.Trim().Length == 0 ||
                INN.Trim().Length == 0 ||
                PhoneNumber.Trim().Length == 0 ||
                PassportSeries.Trim().Length == 0 ||
                PassportNumber.Trim().Length == 0)
                {
                    Repository.Message("Пустым может быть только поле \"Отчество\"");
                    status = false;
                }
            }

            if (OrgForm == "Юридическое лицо")
            {
                if (Name.Trim().Length == 0 ||
                INN.Trim().Length == 0 ||
                KPP.Trim().Length == 0 ||
                PhoneNumber.Trim().Length == 0)
                {
                    Repository.Message("Все поля должны быть заполнены");
                    status = false;
                }
            }

            return status;
        }

        private void CreateBankAccountForm_Closed(object sender, EventArgs e)
        {
            RefreshBankAccounts();
        }        

        private RelayCommand writeCommand;
        public RelayCommand WriteCommand
        {
            get
            {
                return writeCommand ??
                  (writeCommand = new RelayCommand(obj =>
                  {
                      bool status = CheckRequiredFields();

                      if (status == true)
                      {
                          Client client = GetClientFromForm();
                          Repository.OwerwriteOrWriteClient(client);
                      }

                  }));
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
                      bool status = CheckRequiredFields();

                      if (status == true)
                      {
                          Client client = GetClientFromForm();
                          Repository.OwerwriteOrWriteClient(client);

                          foreach (Window window in App.Current.Windows)
                          {
                              if (window is JuridicalPersonForm || window is PhysicalPersonForm)
                              {
                                  window.Close();
                              }
                          }
                      }

                  }));
            }
        }

        private RelayCommand createCommand;
        public RelayCommand CreateCommand
        {
            get
            {
                return createCommand ??
                  (createCommand = new RelayCommand(obj =>
                  {
                      if (!string.IsNullOrEmpty(Id))
                      {
                          CreateAccountForm window = new CreateAccountForm(OrgForm, Id, VIP);
                          window.Show();
                          window.Closed += CreateBankAccountForm_Closed;
                      }

                  }));
            }
        }

        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand(obj =>
                  {
                      if (!string.IsNullOrEmpty(Id))
                      {
                          if (!string.IsNullOrEmpty($"{SelectedAccount}"))
                          {
                              if (SelectedAccount.AccountType == "Депозитный")
                              {
                                  Repository.DepositAccountManager.CloseAccount((DepositAccount)SelectedAccount);
                              }
                              else
                              {
                                  Repository.NonDepositAccountManager.CloseAccount((NonDepositAccount)SelectedAccount);
                              }

                              RefreshBankAccounts();                              
                          }
                      }

                  }));
            }
        }

        private RelayCommand openCommand;
        public RelayCommand OpenCommand
        {
            get
            {
                return openCommand ??
                  (openCommand = new RelayCommand(obj =>
                  {
                      if (!string.IsNullOrEmpty($"{SelectedAccount}"))
                      {
                          if (SelectedAccount.AccountType == "Депозитный")
                          {
                              DepositAccountForm window = new DepositAccountForm(SelectedAccount);
                              window.Show();
                          }
                          else
                          {
                              NonDepositAccountForm window = new NonDepositAccountForm(SelectedAccount);
                              window.Show();
                          }
                      }                          

                  }));
            }
        }
    }
}
