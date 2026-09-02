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
using WpfBLazorHybridClient.Functions.UserHome.AVM;

namespace WpfBLazorHybridClient.Functions.UserHome.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_UserHome.xaml
    /// </summary>
    public partial class UC_UserHome : UserControl
    {
        public UC_UserHome(UserHomeVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
        }

        UserHomeVM model;
        public UserHomeVM Model 
        {
            get { return model; }
        }
    }
}
