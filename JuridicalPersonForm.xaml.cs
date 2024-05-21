using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DeepDiveIntoOOPPart3
{
    /// <summary>
    /// Логика взаимодействия для JuridicalPersonForm.xaml
    /// </summary>
    public partial class JuridicalPersonForm : Window
    {
        internal JuridicalPersonForm(string id)
        {
            InitializeComponent();
            ClientFormsVM clientFormsVM = new ClientFormsVM(id);
            Repository.CurrentClientFormsVM = clientFormsVM;
            DataContext = clientFormsVM;
        }
    }
}
