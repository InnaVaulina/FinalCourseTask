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
using WpfBLazorHybridClient.Functions.Service.AVM;

namespace WpfBLazorHybridClient.Functions.Service.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_Services.xaml
    /// </summary>
    public partial class UC_Services : UserControl
    {
        ServiceVM model;
        public UC_Services(ServiceVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
        }
        public ServiceVM Model { get { return model; } }
    }
}
