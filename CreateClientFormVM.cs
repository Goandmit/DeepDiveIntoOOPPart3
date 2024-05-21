using System;
using System.Windows;

namespace DeepDiveIntoOOPPart3
{
    internal class CreateClientFormVM
    {
        public bool JuridicalPerson { get; set; }
        public bool PhysicalPerson { get; set; }
        public bool IndividualBusinessman { get; set; }

        private Window GetClientForm()
        {
            Window clientForm = null;

            if (JuridicalPerson == true)
            {
                Repository.CurrentOrgForm = "Юридическое лицо";
                clientForm = new JuridicalPersonForm(String.Empty);
            }

            if (PhysicalPerson == true)
            {
                Repository.CurrentOrgForm = "Физическое лицо";
                clientForm = new PhysicalPersonForm(String.Empty);
            }

            if (IndividualBusinessman == true)
            {
                Repository.CurrentOrgForm = "Индивидуальный предприниматель";
                clientForm = new PhysicalPersonForm(String.Empty);
            }

            return clientForm;
        }

        private RelayCommand orgFormIsSelectedCommand;
        public RelayCommand OrgFormIsSelectedCommand
        {
            get
            {
                return orgFormIsSelectedCommand ??
                  (orgFormIsSelectedCommand = new RelayCommand(obj =>
                  {                      
                      Window clientForm = GetClientForm();

                      if (clientForm != null)
                      {
                          clientForm.Show();
                          
                          Repository.CurrentClientListVM.BindToClientList(clientForm);
                      }                      

                      if (JuridicalPerson == true ||
                      PhysicalPerson == true ||
                      IndividualBusinessman == true)
                      {                          
                          foreach (Window window in App.Current.Windows)
                          {
                              if (window is CreateClientForm)
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
