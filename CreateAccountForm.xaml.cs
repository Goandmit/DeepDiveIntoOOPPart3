using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static DeepDiveIntoOOPPart3.CreateAccountFormVM;

namespace DeepDiveIntoOOPPart3
{
    /// <summary>
    /// Логика взаимодействия для CreateBankAccountForm.xaml
    /// </summary>
    public partial class CreateAccountForm : Window
    {
        internal CreateAccountForm(string orgForm, string clientId, bool vIP)
        {
            InitializeComponent();
            DataContext = new CreateAccountFormVM(orgForm, clientId, vIP);
        }

        internal CreateAccountForm(string clientId, string accountType)
        {
            InitializeComponent();
            DataContext = new CreateAccountFormVM(clientId, accountType);
        }
    }
}
