﻿using System;
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
    /// Логика взаимодействия для FundsTransfer.xaml
    /// </summary>
    public partial class TransferForm : Window
    {
        internal TransferForm(Account senderAccount, bool transferToYourself)
        {
            InitializeComponent();
            DataContext = new TransferFormVM(senderAccount, transferToYourself);
        }        
    }
}
