using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace DeepDiveIntoOOPPart3
{
    internal class ClientListVM
    {
        public ObservableCollection<ClientListItem> ClientListItems { get; set; }
        public ClientListItem SelectedСlientListItem { get; set; }

        public ClientListVM()
        {
            ClientListItems = new ObservableCollection<ClientListItem>();
            LoadClientList();

            foreach (Window window in App.Current.Windows)
            {
                if (window is ClientList)
                {
                    window.Closed += ClientList_Closed;
                }
            }
        }

        private void LoadClientList()
        {
            List<ClientListItem> list = Repository.GetСlientListItems();

            foreach (ClientListItem listItem in list)
            {
                ClientListItems.Add(listItem);
            }
        }

        private void RefreshClientList()
        {
            ClientListItems.Clear();
            LoadClientList();
        }

        public void BindToClientList(Window clientForm)
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window is ClientList)
                {
                    clientForm.Owner = window;
                }
            }

            clientForm.Closed += ClientForm_Closed;
        }

        private void ClientList_Closed(object sender, EventArgs e)
        {
            foreach (Window window in App.Current.Windows)
            {
                window.Close();
            }
        }        

        private void ClientForm_Closed(object sender, EventArgs e)
        {
            RefreshClientList();

            foreach (Window window in App.Current.Windows)
            {
                if (window is ClientList)
                {
                    window.Activate();
                }
            }
        }

        private void CreateClientForm_Closed(object sender, EventArgs e)
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window is JuridicalPersonForm ||
                window is PhysicalPersonForm)
                {
                    window.Closed += ClientForm_Closed;
                }
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
                      CreateClientForm window = new CreateClientForm();
                      window.Show();

                      window.Closed += CreateClientForm_Closed;                      

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
                      if (!string.IsNullOrEmpty($"{SelectedСlientListItem}"))
                      {                          
                          Repository.DeleteClient(SelectedСlientListItem.Id);
                          RefreshClientList();
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
                      if (!string.IsNullOrEmpty($"{SelectedСlientListItem}"))
                      {
                          Window clientForm;

                          if (SelectedСlientListItem.OrgForm == "Юридическое лицо")
                          {
                              clientForm = new JuridicalPersonForm(SelectedСlientListItem.Id);                              
                          }
                          else
                          {
                              clientForm = new PhysicalPersonForm(SelectedСlientListItem.Id);                              
                          }

                          clientForm.Show();

                          BindToClientList(clientForm);
                      }

                  }));
            }
        }
    }
}
