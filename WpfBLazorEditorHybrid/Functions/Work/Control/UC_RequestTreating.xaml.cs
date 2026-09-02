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
using WpfBLazorHybridClient.Functions.Work.AVM;

namespace WpfBLazorHybridClient.Functions.Work.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_RequestTreating.xaml
    /// </summary>
    public partial class UC_RequestTreating : UserControl
    {
        public UC_RequestTreating(RequestTreatingVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
        }

        RequestTreatingVM model;
        public RequestTreatingVM Model { get { return model; } }
    }
}
