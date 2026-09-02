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
using WpfBLazorHybridClient.Functions.Progect.AVM;

namespace WpfBLazorHybridClient.Functions.Progect.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_Progects.xaml
    /// </summary>
    public partial class UC_Progects : UserControl
    {
        ProgectVM model;
        public ProgectVM Model { get { return model; } }
        public UC_Progects(ProgectVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
        }

        

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterPanel.Visibility = Visibility.Visible;
            FilterButton.Visibility = Visibility.Hidden;
        }

        private void FilterHideButton_Click(object sender, RoutedEventArgs e)
        {
            FilterPanel.Visibility = Visibility.Hidden;
            FilterButton.Visibility = Visibility.Visible;
        }
    }
}
