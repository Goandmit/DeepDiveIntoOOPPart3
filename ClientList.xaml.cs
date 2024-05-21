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
    /// Логика взаимодействия для ClientList.xaml
    /// </summary>
    public partial class ClientList : Window
    {
        public ClientList()
        {
            InitializeComponent();
            ClientListVM clientListVM = new ClientListVM();
            Repository.CurrentClientListVM = clientListVM;
            DataContext = clientListVM;
        }
    }
}
