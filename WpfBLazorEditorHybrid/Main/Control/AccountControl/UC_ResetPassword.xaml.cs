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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfBLazorHybridClient.Functions.Admin.AVM;
using WpfBLazorHybridClient.Main.AVM.Account;

namespace WpfBLazorHybridClient.Main.Control.AccountControl
{
    /// <summary>
    /// Логика взаимодействия для UC_ResetPassword.xaml
    /// </summary>
    public partial class UC_ResetPassword : UserControl
    {
        ResetUserPasswordVM model;
        public ResetUserPasswordVM Model { get { return model; } }

        public UC_ResetPassword(ResetUserPasswordVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
        }
    }
}
