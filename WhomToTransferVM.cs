using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace DeepDiveIntoOOPPart3
{
    internal class WhomToTransferVM
    {
        public ObservableCollection<ClientListItem> ClientListItems { get; set; }
        public ClientListItem SelectedСlientListItem { get; set; }

        public WhomToTransferVM(string senderId)
        {
            ClientListItems = new ObservableCollection<ClientListItem>();

            List<ClientListItem> list = Repository.GetСlientListItems();

            foreach (ClientListItem listItem in list)
            {
                if (listItem.Id == senderId)
                {
                    continue;
                }
                
                ClientListItems.Add(listItem);                               
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
                      if (!string.IsNullOrEmpty($"{SelectedСlientListItem}"))
                      {
                          Repository.RecipientId = SelectedСlientListItem.Id;
                          Repository.RecipientName = SelectedСlientListItem.FullName;

                          foreach (Window window in App.Current.Windows)
                          {
                              if (window is WhomToTransfer)
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
